using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using ControllerHub.PluginContract;

namespace ControllerHub.Models;

public class Mapping
{
    [JsonPropertyName("deviceIdentifier")]
    public string? DeviceIdentifier { get; set; }

    [JsonPropertyName("buttonMap")]
    public Dictionary<string, ActionMapping>? ButtonMap { get; set; }

    [JsonPropertyName("switchMap")]
    public Dictionary<string, ActionMapping>? SwitchMap { get; set; }

    [JsonPropertyName("axisMap")]
    public Dictionary<string, ActionMapping>? AxisMap { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? PluginConfigs { get; set; }
}
