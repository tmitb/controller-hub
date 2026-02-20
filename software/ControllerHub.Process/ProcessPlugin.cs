using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ControllerHub.PluginContract;

namespace ControllerHub.Process;

public sealed class ProcessPlugin : IActionPlugin
{
    public string TypeName => "process";
    private IPluginLogger? _logger;

    private static string? GetStr(Dictionary<string, JsonElement>? d, string key)
        => d != null && d.TryGetValue(key, out var e)
           ? (e.ValueKind == JsonValueKind.String ? e.GetString() : e.GetRawText())
           : null;

    public Task InitializeAsync(JsonElement? pluginConfig, IPluginLogger logger, CancellationToken ct)
    {
        _logger = logger;
        return Task.CompletedTask;
    }

    public Task ExecuteAsync(ActionMapping action, CancellationToken ct)
    {
        var executable     = GetStr(action.ExtraData, "executable");
        var arguments      = GetStr(action.ExtraData, "arguments");
        var workingDir     = GetStr(action.ExtraData, "workingDirectory");

        if (string.IsNullOrWhiteSpace(executable))
        {
            _logger?.LogWarning("Process action has no executable; skipping.");
            return Task.CompletedTask;
        }
        if (arguments == null)
        {
            _logger?.LogWarning("Process action has no arguments; skipping.");
            return Task.CompletedTask;
        }
        if (string.IsNullOrWhiteSpace(workingDir))
        {
            _logger?.LogWarning("Process action has no workingDirectory; skipping.");
            return Task.CompletedTask;
        }
        _ = LaunchAsync(executable, arguments, workingDir);
        return Task.CompletedTask;
    }

    private async Task LaunchAsync(string executable, string arguments, string workingDirectory)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = executable,
                Arguments = arguments,
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            using var process = System.Diagnostics.Process.Start(psi);
            _logger?.LogInfo($"Launched process: {executable}");
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger?.LogError($"Failed to launch process '{executable}': {ex.Message}");
        }
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
