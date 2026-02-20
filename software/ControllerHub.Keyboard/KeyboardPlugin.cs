using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ControllerHub.PluginContract;

namespace ControllerHub.Keyboard;

public sealed class KeyboardPlugin : IActionPlugin
{
    public string TypeName => "keyboard";
    private IPluginLogger? _logger;

    public Task InitializeAsync(JsonElement? pluginConfig, IPluginLogger logger, CancellationToken ct)
    {
        _logger = logger;
        return Task.CompletedTask;
    }

    public Task ExecuteAsync(ActionMapping action, CancellationToken ct)
    {
        var extra = action.ExtraData;

        string? keyName = null;
        if (extra != null && extra.TryGetValue("key", out var keyEl) &&
            keyEl.ValueKind == JsonValueKind.String)
        {
            keyName = keyEl.GetString();
        }

        if (string.IsNullOrWhiteSpace(keyName))
        {
            _logger?.LogWarning("Keyboard action has no key; skipping.");
            return Task.CompletedTask;
        }

        var vk = VkMap.Lookup(keyName);
        if (vk is null)
        {
            _logger?.LogWarning($"Keyboard action: unknown key '{keyName}'; skipping.");
            return Task.CompletedTask;
        }

        bool ctrl  = GetBool(extra, "ctrl");
        bool shift = GetBool(extra, "shift");
        bool alt   = GetBool(extra, "alt");

        _ = Task.Run(() => NativeMethods.SendKeyCombo(vk.Value, ctrl, shift, alt), ct);
        return Task.CompletedTask;
    }

    private static bool GetBool(System.Collections.Generic.Dictionary<string, JsonElement>? d, string key)
        => d != null && d.TryGetValue(key, out var e) && e.ValueKind == JsonValueKind.True;

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
