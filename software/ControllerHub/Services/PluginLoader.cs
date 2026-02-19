using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ControllerHub.Models;
using ControllerHub.PluginContract;

namespace ControllerHub.Services;

public static class PluginLoader
{
    // Phase 1: scan plugins/*.dll, instantiate, check for duplicate TypeNames.
    // Does NOT call InitializeAsync.
    public static List<IActionPlugin> Discover()
    {
        var plugins = new List<IActionPlugin>();
        var pluginsDir = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
            "plugins");

        if (!Directory.Exists(pluginsDir))
        {
            Console.WriteLine("[INFO] No plugins/ directory found; running with no plugins.");
            return plugins;
        }

        foreach (var dllPath in Directory.GetFiles(pluginsDir, "*.dll", SearchOption.TopDirectoryOnly))
        {
            Assembly asm;
            try
            {
                asm = Assembly.LoadFrom(dllPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARN] Failed to load assembly '{dllPath}': {ex.Message}");
                continue;
            }

            Type[] types;
            try
            {
                types = asm.GetExportedTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                Console.WriteLine($"[WARN] Failed to enumerate types in '{dllPath}':");
                foreach (var le in ex.LoaderExceptions ?? Array.Empty<Exception>())
                    Console.WriteLine($"  {le?.Message}");
                continue;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARN] Failed to enumerate types in '{dllPath}': {ex.Message}");
                continue;
            }

            foreach (var type in types)
            {
                if (type.IsAbstract || type.IsInterface)
                    continue;
                if (!typeof(IActionPlugin).IsAssignableFrom(type))
                    continue;

                IActionPlugin instance;
                try
                {
                    instance = (IActionPlugin)Activator.CreateInstance(type)!;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[WARN] Failed to instantiate plugin type '{type.FullName}': {ex.Message}");
                    continue;
                }

                // Check for duplicate TypeName
                foreach (var existing in plugins)
                {
                    if (string.Equals(existing.TypeName, instance.TypeName, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.Error.WriteLine($"[FATAL] Duplicate plugin TypeName '{instance.TypeName}' from '{dllPath}'. Aborting.");
                        Environment.Exit(1);
                    }
                }

                plugins.Add(instance);
            }
        }

        return plugins;
    }

    // Phase 2: call InitializeAsync on each plugin using (possibly CLI-enriched) configs.
    // Config lookup: enrichedConfigs[plugin.TypeName] takes priority, then mapping.PluginConfigs[plugin.TypeName].
    // Failed plugins are removed from the list and disposed.
    public static async Task InitializeAllAsync(
        List<IActionPlugin> plugins,
        Mapping mapping,
        IReadOnlyDictionary<string, JsonElement> enrichedConfigs,
        CancellationToken ct)
    {
        var toRemove = new List<IActionPlugin>();

        foreach (var instance in plugins)
        {
            JsonElement? cfg = null;
            if (enrichedConfigs.TryGetValue(instance.TypeName, out var enriched))
                cfg = enriched;
            else if (mapping.PluginConfigs != null &&
                     mapping.PluginConfigs.TryGetValue(instance.TypeName, out var cfgElement))
                cfg = cfgElement;

            try
            {
                await instance.InitializeAsync(cfg, new PluginLogger(instance.TypeName), ct);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARN] Plugin '{instance.TypeName}' failed to initialize: {ex.Message}");
                await instance.DisposeAsync();
                toRemove.Add(instance);
            }
        }

        foreach (var failed in toRemove)
            plugins.Remove(failed);
    }
}
