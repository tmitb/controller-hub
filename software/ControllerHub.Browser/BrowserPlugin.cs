using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ControllerHub.PluginContract;

namespace ControllerHub.Browser;

public sealed class BrowserPlugin : IActionPlugin
{
    public string TypeName => "browser";
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
        var url = GetStr(action.ExtraData, "url");
        if (string.IsNullOrWhiteSpace(url))
        {
            _logger?.LogWarning("Browser action has no URL; skipping.");
            return Task.CompletedTask;
        }
        _ = OpenAsync(url);
        return Task.CompletedTask;
    }

    private async Task OpenAsync(string url)
    {
        try
        {
            Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
            _logger?.LogInfo($"Opened URL in browser: {url}");
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger?.LogError($"Failed to open URL '{url}': {ex.Message}");
        }
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
