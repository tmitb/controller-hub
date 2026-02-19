using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ControllerHub.PluginContract;

namespace ControllerHub.Http;

public sealed class HttpPlugin : IActionPlugin
{
    public string TypeName => "http";
    private HttpClient? _client;
    private IPluginLogger? _logger;

    public Task InitializeAsync(JsonElement? pluginConfig, IPluginLogger logger, CancellationToken ct)
    {
        _logger = logger;
        _client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
        return Task.CompletedTask;
    }

    public Task ExecuteAsync(ActionMapping action, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(action.Url))
        {
            _logger?.LogWarning("HTTP action has no URL; skipping.");
            return Task.CompletedTask;
        }
        _ = SendAsync(action);
        return Task.CompletedTask;
    }

    private async Task SendAsync(ActionMapping action)
    {
        try
        {
            var method = new HttpMethod(action.Method ?? "GET");
            var request = new HttpRequestMessage(method, action.Url);

            if (action.Headers != null)
            {
                foreach (var header in action.Headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            if (action.Body.HasValue && (method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Patch))
            {
                var body = action.Body.Value;
                if (body.ValueKind == JsonValueKind.Object || body.ValueKind == JsonValueKind.Array)
                {
                    var json = body.GetRawText();
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }
                else if (body.ValueKind == JsonValueKind.String)
                {
                    request.Content = new StringContent(body.GetString() ?? "", Encoding.UTF8, "text/plain");
                }
            }

            var response = await _client!.SendAsync(request);
            var statusCode = (int)response.StatusCode;
            if (statusCode >= 200 && statusCode < 300)
                _logger?.LogInfo($"HTTP {method} {action.Url} -> {statusCode}");
            else
                _logger?.LogWarning($"HTTP {method} {action.Url} -> {statusCode}");
        }
        catch (Exception ex)
        {
            _logger?.LogError($"HTTP request failed: {ex.Message}");
        }
    }

    public ValueTask DisposeAsync()
    {
        _client?.Dispose();
        return ValueTask.CompletedTask;
    }
}
