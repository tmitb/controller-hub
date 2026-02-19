using ControllerHub.Obs;

namespace ControllerHub.Obs.Requests;

public abstract class BaseRequests
{
    protected readonly ObsBridge _bridge;
    protected BaseRequests(ObsBridge bridge) => _bridge = bridge;
}
