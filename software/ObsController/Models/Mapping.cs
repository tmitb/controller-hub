using System.Collections.Generic;

namespace ObsController.Models;

/// <summary>
/// Represents the JSON structure used by the original Python project.
/// Example (mapping.json):
/// {
///   "deviceIdentifier": "my-controller-id",
///   "host": "localhost",
///   "port": 4455,
///   "password": "secret",
///   "buttonMap": { "0": {"action":"StartStreaming"} },
///   "switchMap": { "0": {"action":"SetSourceVisibility", "parameter":"Camera,true"} },
///   "axisMap":   { "0": {"action":"SetVolume", "parameter":"Mic,{{value}}"} }
/// }
/// </summary>
public class Mapping
{
    // Identifier that matches RawGameController.NonRoamableId. This is a plain character string.
    public string DeviceIdentifier { get; set; }

    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 4455;
    public string Password { get; set; }

    // Index‑based dictionaries for buttons, switches and axes (keys are stringified integers)
    public Dictionary<string, ActionMapping> ButtonMap   { get; set; }
    public Dictionary<string, ActionMapping> SwitchMap   { get; set; }
    public Dictionary<string, ActionMapping> AxisMap     { get; set; }
}

/// <summary>
/// Represents a configurable action.
/// "Category" selects the request class (e.g., "Scenes").
/// "Action" is the method name to invoke (without the Async suffix).
/// "Parameters" provides named arguments for that method.
/// </summary>
public class ActionMapping
{
    public string Category { get; set; }
    public string Action { get; set; }
    // Simple key/value pairs; values are kept as strings and converted when invoking.
    public Dictionary<string, string> Parameters { get; set; }
}
