namespace ControllerHub.PluginContract;

public interface IPluginLogger
{
    void LogInfo(string message);
    void LogWarning(string message);
    void LogError(string message);
}
