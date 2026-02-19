using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ControllerHub.PluginContract;

public class ActionMapping
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraData { get; set; }
}
