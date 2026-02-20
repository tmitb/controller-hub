using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ControllerHub.PluginContract;

namespace ControllerHub.Text;

public sealed class TextPlugin : IActionPlugin
{
    public string TypeName => "text";
    private IPluginLogger? _logger;

    public Task InitializeAsync(JsonElement? pluginConfig, IPluginLogger logger, CancellationToken ct)
    {
        _logger = logger;
        return Task.CompletedTask;
    }

    public Task ExecuteAsync(ActionMapping action, CancellationToken ct)
    {
        string? text = null;
        if (action.ExtraData != null &&
            action.ExtraData.TryGetValue("text", out var el) &&
            el.ValueKind == JsonValueKind.String)
        {
            text = el.GetString();
        }

        if (string.IsNullOrEmpty(text))
        {
            _logger?.LogWarning("Text action has no 'text' value; skipping.");
            return Task.CompletedTask;
        }

        _ = TypeAsync(text, ct);
        return Task.CompletedTask;
    }

    private async Task TypeAsync(string text, CancellationToken ct)
    {
        try
        {
            foreach (char ch in text)
            {
                NativeMethods.SendUnicodeChar(ch);
                await Task.Delay(10, ct);
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            _logger?.LogError($"Text plugin failed while typing: {ex.Message}");
        }
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
