using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ControllerHub.PluginContract;

namespace ControllerHub.Obs;

public sealed class ObsPlugin : IActionPlugin
{
    public string TypeName => "obs";
    private ObsBridge? _bridge;
    private IPluginLogger? _logger;
    private bool _available;

    public async Task InitializeAsync(JsonElement? pluginConfig, IPluginLogger logger, CancellationToken ct)
    {
        _logger = logger;
        var host = "localhost";
        var port = 4455;
        var password = "";

        if (pluginConfig.HasValue)
        {
            if (pluginConfig.Value.TryGetProperty("host", out var h))
                host = h.GetString() ?? host;
            if (pluginConfig.Value.TryGetProperty("port", out var p))
                port = p.GetInt32();
            if (pluginConfig.Value.TryGetProperty("password", out var pw))
                password = pw.GetString() ?? password;
        }

        try
        {
            _bridge = new ObsBridge(host, port, password);
            await _bridge.ConnectAsync(ct);
            _available = true;
            logger.LogInfo($"Connected to OBS at {host}:{port}");
        }
        catch (Exception ex)
        {
            logger.LogError($"Failed to connect to OBS: {ex.Message}");
            _available = false;
            // Do NOT rethrow — plugin stays registered but marks itself unavailable
        }
    }

    public async Task ExecuteAsync(ActionMapping action, CancellationToken ct)
    {
        if (!_available || _bridge == null)
        {
            _logger?.LogWarning("OBS unavailable; skipping action.");
            return;
        }
        try { await ActionExecutor.ExecuteAsync(action, _bridge); }
        catch (Exception ex) { _logger?.LogError($"OBS action failed: {ex.Message}"); }
    }

    public async ValueTask DisposeAsync()
    {
        if (_bridge != null) await _bridge.DisposeAsync();
    }
}
