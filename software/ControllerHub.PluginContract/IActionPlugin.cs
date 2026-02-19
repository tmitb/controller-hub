using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ControllerHub.PluginContract;

public interface IActionPlugin : IAsyncDisposable
{
    string TypeName { get; }
    Task InitializeAsync(JsonElement? pluginConfig, IPluginLogger logger, CancellationToken ct);
    Task ExecuteAsync(ActionMapping action, CancellationToken ct);
}
