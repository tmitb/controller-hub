using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ControllerHub.PluginContract;

namespace ControllerHub.Mouse;

public sealed class MousePlugin : IActionPlugin
{
    public string TypeName => "mouse";
    private IPluginLogger? _logger;

    public Task InitializeAsync(JsonElement? pluginConfig, IPluginLogger logger, CancellationToken ct)
    {
        _logger = logger;
        return Task.CompletedTask;
    }

    public Task ExecuteAsync(ActionMapping action, CancellationToken ct)
    {
        var extra = action.ExtraData;

        if (extra == null ||
            !extra.TryGetValue("steps", out var stepsEl) ||
            stepsEl.ValueKind != JsonValueKind.Array)
        {
            _logger?.LogWarning("Mouse action has no steps array; skipping.");
            return Task.CompletedTask;
        }

        var steps = stepsEl.EnumerateArray().ToArray();

        _ = Task.Run(async () =>
        {
            foreach (var step in steps)
                await ExecuteStepAsync(step);
        }, ct);

        return Task.CompletedTask;
    }

    private async Task ExecuteStepAsync(JsonElement step)
    {
        if (step.ValueKind != JsonValueKind.Object) return;

        if (!step.TryGetProperty("action", out var actionEl) ||
            actionEl.ValueKind != JsonValueKind.String)
        {
            _logger?.LogWarning("Mouse step missing 'action' field; skipping.");
            return;
        }

        string? actionName = actionEl.GetString();

        switch (actionName)
        {
            case "move":
                if (!TryGetInt(step, "x", out int x) || !TryGetInt(step, "y", out int y))
                {
                    _logger?.LogWarning("Mouse 'move' step missing 'x' or 'y'; skipping.");
                    return;
                }
                NativeMethods.MoveTo(x, y);
                break;

            case "down":
                if (!TryGetString(step, "button", out string? btnDown))
                {
                    _logger?.LogWarning("Mouse 'down' step missing 'button'; skipping.");
                    return;
                }
                NativeMethods.ButtonEvent(btnDown!, down: true);
                break;

            case "up":
                if (!TryGetString(step, "button", out string? btnUp))
                {
                    _logger?.LogWarning("Mouse 'up' step missing 'button'; skipping.");
                    return;
                }
                NativeMethods.ButtonEvent(btnUp!, down: false);
                break;

            case "click":
                if (!TryGetString(step, "button", out string? btnClick))
                {
                    _logger?.LogWarning("Mouse 'click' step missing 'button'; skipping.");
                    return;
                }
                NativeMethods.Click(btnClick!);
                break;

            case "delay":
                if (!TryGetInt(step, "ms", out int ms))
                {
                    _logger?.LogWarning("Mouse 'delay' step missing 'ms'; skipping.");
                    return;
                }
                await Task.Delay(ms);
                break;

            default:
                _logger?.LogWarning($"Mouse step has unknown action '{actionName}'; skipping.");
                break;
        }
    }

    private static bool TryGetInt(JsonElement el, string name, out int value)
    {
        if (el.TryGetProperty(name, out var prop) && prop.ValueKind == JsonValueKind.Number)
            return prop.TryGetInt32(out value);
        value = 0;
        return false;
    }

    private static bool TryGetString(JsonElement el, string name, out string? value)
    {
        if (el.TryGetProperty(name, out var prop) && prop.ValueKind == JsonValueKind.String)
        {
            value = prop.GetString();
            return value != null;
        }
        value = null;
        return false;
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
