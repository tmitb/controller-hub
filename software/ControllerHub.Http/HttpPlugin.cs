using System;
using System.Collections.Generic;
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

    private static string? GetStr(Dictionary<string, JsonElement>? d, string key)
        => d != null && d.TryGetValue(key, out var e)
           ? (e.ValueKind == JsonValueKind.String ? e.GetString() : e.GetRawText())
           : null;

    public Task InitializeAsync(JsonElement? pluginConfig, IPluginLogger logger, CancellationToken ct)
    {
        _logger = logger;
        _client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
        return Task.CompletedTask;
    }

    public Task ExecuteAsync(ActionMapping action, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(GetStr(action.ExtraData, "url")))
        {
            _logger?.LogWarning("HTTP action has no URL; skipping.");
            return Task.CompletedTask;
        }
        _ = SendAsync(action);
        return Task.CompletedTask;
    }

    private async Task SendAsync(ActionMapping action)
    {
        var extra = action.ExtraData;
        try
        {
            var url = GetStr(extra, "url");
            var method = new HttpMethod(GetStr(extra, "method") ?? "GET");
            var request = new HttpRequestMessage(method, url);

            if (extra != null && extra.TryGetValue("headers", out var headersEl) &&
                headersEl.ValueKind == JsonValueKind.Object)
            {
                foreach (var prop in headersEl.EnumerateObject())
                {
                    var val = prop.Value.ValueKind == JsonValueKind.String
                        ? prop.Value.GetString()!
                        : prop.Value.GetRawText();
                    request.Headers.TryAddWithoutValidation(prop.Name, val);
                }
            }

            if (extra != null && extra.TryGetValue("body", out var bodyEl) &&
                (method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Patch))
            {
                if (bodyEl.ValueKind == JsonValueKind.Object || bodyEl.ValueKind == JsonValueKind.Array)
                {
                    request.Content = new StringContent(bodyEl.GetRawText(), Encoding.UTF8, "application/json");
                }
                else if (bodyEl.ValueKind == JsonValueKind.String)
                {
                    request.Content = new StringContent(bodyEl.GetString() ?? "", Encoding.UTF8, "text/plain");
                }
            }

            var response = await _client!.SendAsync(request);
            var statusCode = (int)response.StatusCode;
            if (statusCode >= 200 && statusCode < 300)
                _logger?.LogInfo($"HTTP {method} {url} -> {statusCode}");
            else
                _logger?.LogWarning($"HTTP {method} {url} -> {statusCode}");
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
