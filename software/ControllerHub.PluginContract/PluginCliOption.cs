namespace ControllerHub.PluginContract;

public sealed record PluginCliOption(
    string Name,          // e.g. "--obs-host"  (include the "--" prefix)
    string Description,
    Type   ValueType,     // typeof(string), typeof(int?), typeof(bool?)
    bool   IsRequired = false);
