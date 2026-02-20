using System;
using ControllerHub.PluginContract;

namespace ControllerHub.Services;

public sealed class PluginLogger : IPluginLogger
{
    private readonly string _prefix;
    public PluginLogger(string pluginTypeName) => _prefix = $"[{pluginTypeName.ToUpperInvariant()}]";
    public void LogInfo(string msg)    => Console.WriteLine($"[INFO]{_prefix} {msg}");
    public void LogWarning(string msg) => Console.WriteLine($"[WARN]{_prefix} {msg}");
    public void LogError(string msg)   => Console.Error.WriteLine($"[ERROR]{_prefix} {msg}");
}
