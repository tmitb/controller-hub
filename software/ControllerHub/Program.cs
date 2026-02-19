using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ControllerHub.Models;
using ControllerHub.PluginContract;
using ControllerHub.Services;

namespace ControllerHub;

public class Program
{
    private sealed class BoundCliOption
    {
        public string PluginTypeName { get; }
        public string JsonKey { get; }
        public Option Option { get; }
        public Func<ParseResult, object?> GetValue { get; }

        private BoundCliOption(string pluginTypeName, string jsonKey, Option option, Func<ParseResult, object?> getValue)
        {
            PluginTypeName = pluginTypeName;
            JsonKey = jsonKey;
            Option = option;
            GetValue = getValue;
        }

        public static BoundCliOption From(PluginCliOption pco, string typeName)
        {
            // Build Option<T> via reflection (T = pco.ValueType)
            var optType = typeof(Option<>).MakeGenericType(pco.ValueType);
            var option = (Option)optType.GetConstructor([typeof(string), typeof(string)])!
                                        .Invoke([pco.Name, pco.Description]);
            if (pco.IsRequired) option.IsRequired = true;

            // Build Func<ParseResult, object?> that calls GetValueForOption<T>
            var getValueForOption = typeof(ParseResult)
                .GetMethod(nameof(ParseResult.GetValueForOption))!
                .MakeGenericMethod(pco.ValueType);
            Func<ParseResult, object?> getValue = pr => getValueForOption.Invoke(pr, [option]);

            // Derive JSON key: strip "--{typeName}-" prefix, e.g. "--obs-host" -> "host"
            var prefix = $"--{typeName}-";
            var jsonKey = pco.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                ? pco.Name[prefix.Length..]
                : pco.Name.TrimStart('-');

            return new BoundCliOption(typeName, jsonKey, option, getValue);
        }
    }

    private static JsonElement MergeIntoConfig(
        JsonElement? existing,
        IEnumerable<(string key, object? value)> overrides)
    {
        var dict = existing?.ValueKind == JsonValueKind.Object
            ? JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(existing.Value.GetRawText())!
            : new Dictionary<string, JsonElement>();

        foreach (var (key, value) in overrides)
            if (value is not null)
                dict[key] = JsonSerializer.SerializeToElement(value);

        return JsonSerializer.SerializeToElement(dict);
    }

    public static async Task<int> Main(string[] args)
    {
        // Phase 1: Discover plugins (instantiate without initialising)
        var plugins = PluginLoader.Discover();

        // Collect CLI options from all discovered plugins
        var bound = plugins
            .SelectMany(p => p.GetCliOptions().Select(pco => BoundCliOption.From(pco, p.TypeName)))
            .ToList();

        var rootCommand = new RootCommand("BLE\u2011OBS controller \u2013 reads a configurable gamepad and triggers OBS actions.");
        foreach (var b in bound)
            rootCommand.AddOption(b.Option);

        rootCommand.SetHandler(async (InvocationContext ctx) =>
        {
            // Load configuration from mapping.json
            Mapping mapping = ConfigLoader.Load();

            // Build enriched configs from CLI overrides
            var enriched = new Dictionary<string, JsonElement>(StringComparer.OrdinalIgnoreCase);
            foreach (var typeName in bound.Select(b => b.PluginTypeName).Distinct(StringComparer.OrdinalIgnoreCase))
            {
                var overrides = new List<(string key, object? value)>();
                foreach (var b in bound.Where(b => string.Equals(b.PluginTypeName, typeName, StringComparison.OrdinalIgnoreCase)))
                {
                    var value = b.GetValue(ctx.ParseResult);
                    if (value is not null)
                        overrides.Add((b.JsonKey, value));
                }

                if (overrides.Count > 0)
                {
                    JsonElement? existingCfg = null;
                    if (mapping.PluginConfigs != null && mapping.PluginConfigs.TryGetValue(typeName, out var ec))
                        existingCfg = ec;
                    enriched[typeName] = MergeIntoConfig(existingCfg, overrides);
                }
            }

            using var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            // Phase 2: Initialise plugins with merged config
            await PluginLoader.InitializeAllAsync(plugins, mapping, enriched, cts.Token);

            var pluginMap = new Dictionary<string, IActionPlugin>(StringComparer.OrdinalIgnoreCase);
            foreach (var p in plugins)
                pluginMap[p.TypeName] = p;

            // Initialise controller provider using the RawGameController API (covers BLE devices)
            IGamepadProvider gp = RawGamepadProvider.TryCreate(mapping.DeviceIdentifier);
            ListAvailableGamepads();

            if (gp == null)
            {
                Console.WriteLine("[WARN] No Windows.Gaming.Input gamepad detected \u2013 controller will be idle.");
                gp = new NullGamepadProvider();
            }

            gp.StateChanged += states => HandleStateChanges(states, mapping, pluginMap);
            gp.Start();
            Console.WriteLine($"Listening on gamepad device {mapping.DeviceIdentifier}\u2026 Press Ctrl+C to exit.");

            try
            {
                await Task.Delay(Timeout.Infinite, cts.Token);
            }
            catch (OperationCanceledException) { /* normal shutdown */ }

            gp.Stop();
            gp.Dispose();
            for (int i = plugins.Count - 1; i >= 0; i--)
                await plugins[i].DisposeAsync();
        });

        return await rootCommand.InvokeAsync(args);
    }

    private static void HandleStateChanges(ControllerDelta changes, Mapping mapping, Dictionary<string, IActionPlugin> pluginMap)
    {
        if (mapping.ButtonMap == null)
            return;

        foreach (var key in changes.ButtonsChanged.Keys)
        {
            if (!changes.ButtonsChanged[key])
                continue;

            if (!mapping.ButtonMap.ContainsKey(key.ToString()))
                continue;

            var mappingEntry = mapping.ButtonMap[key.ToString()];
            if (mappingEntry == null) continue;

            var typeName = mappingEntry.Type ?? "obs";
            if (!pluginMap.TryGetValue(typeName, out var plugin))
            {
                Console.WriteLine($"[WARN] No plugin registered for type '{typeName}' (button {key}).");
                continue;
            }

            _ = plugin.ExecuteAsync(mappingEntry, CancellationToken.None);
        }
    }

    private static void ListAvailableGamepads()
    {
        try
        {
            if (Windows.Gaming.Input.RawGameController.RawGameControllers.Count == 0)
            {
                Console.WriteLine("[INFO] No Gamepad objects reported by Windows.Gaming.Input.");
                return;
            }

            Console.WriteLine("[INFO] Detected Gamepads:");
            int index = 0;
            foreach (var gp in Windows.Gaming.Input.RawGameController.RawGameControllers)
            {
                Console.WriteLine($"   [{index}] Id: {gp.NonRoamableId.Replace("\0", "")}");
                index++;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Failed to enumerate Gamepads: {ex.Message}");
        }
    }
}
