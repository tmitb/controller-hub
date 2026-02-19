using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ControllerHub.Models;
using ControllerHub.PluginContract;
using ControllerHub.Services;

namespace ControllerHub;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        // Define CLI options (all optional – they override mapping.json values)
        var hostOption = new Option<string>("--host", description: "OBS websocket host (default from mapping.json)");
        var portOption = new Option<int?>("--port", description: "OBS websocket port (default from mapping.json)");
        var passwordOption = new Option<string>("--password", description: "OBS websocket password (overrides mapping.json)");

        var rootCommand = new RootCommand("BLE‑OBS controller – reads a configurable gamepad and triggers OBS actions.")
        {
            hostOption,
            portOption,
            passwordOption
        };

        rootCommand.SetHandler(async (host, port, password) =>
        {
            // Load configuration from mapping.json
            Mapping mapping = ConfigLoader.Load();

            // Apply CLI overrides into the "obs" plugin config block
            if (host != null || port.HasValue || password != null)
            {
                mapping.PluginConfigs ??= new Dictionary<string, JsonElement>();

                string obsHost = "localhost";
                int obsPort = 4455;
                string obsPwd = "";

                if (mapping.PluginConfigs.TryGetValue("obs", out var existing))
                {
                    if (existing.TryGetProperty("host", out var h)) obsHost = h.GetString()!;
                    if (existing.TryGetProperty("port", out var p)) obsPort = p.GetInt32();
                    if (existing.TryGetProperty("password", out var pw)) obsPwd = pw.GetString()!;
                }

                if (host != null) obsHost = host;
                if (port.HasValue) obsPort = port.Value;
                if (password != null) obsPwd = password;

                mapping.PluginConfigs["obs"] = JsonSerializer.SerializeToElement(
                    new { host = obsHost, port = obsPort, password = obsPwd });
            }

            using var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            // Load plugins
            var plugins = await PluginLoader.LoadAsync(mapping, cts.Token);
            var pluginMap = new Dictionary<string, IActionPlugin>(StringComparer.OrdinalIgnoreCase);
            foreach (var p in plugins)
                pluginMap[p.TypeName] = p;

            // Initialise controller provider using the RawGameController API (covers BLE devices)
            IGamepadProvider gp = RawGamepadProvider.TryCreate(mapping.DeviceIdentifier);
            ListAvailableGamepads(); // show what the OS sees so the user can adjust mapping.json

            if (gp == null)
            {
                Console.WriteLine("[WARN] No Windows.Gaming.Input gamepad detected – controller will be idle.");
                gp = new NullGamepadProvider(); // do‑nothing fallback to keep the app alive
            }

            gp.StateChanged += states => HandleStateChanges(states, mapping, pluginMap);
            gp.Start();
            Console.WriteLine($"Listening on gamepad device {mapping.DeviceIdentifier}… Press Ctrl+C to exit.");

            // Wait until cancellation
            try
            {
                await Task.Delay(Timeout.Infinite, cts.Token);
            }
            catch (OperationCanceledException) { /* normal shutdown */ }

            // Shutdown
            gp.Stop();
            gp.Dispose();
            for (int i = plugins.Count - 1; i >= 0; i--)
                await plugins[i].DisposeAsync();

        }, hostOption, portOption, passwordOption);

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
