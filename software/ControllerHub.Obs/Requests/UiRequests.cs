using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ControllerHub.Obs.Requests;
public class UiRequests : BaseRequests
{
    public UiRequests(ObsBridge bridge) : base(bridge) {}
    /// <summary>Gets whether studio is enabled. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public async Task<bool> GetStudioModeEnabledAsync()
    {
        var resp = await _bridge.SendRequestAsync("GetStudioModeEnabled", new JObject());
        return resp["studioModeEnabled"].ToObject<bool>();
    }
    /// <summary>Enables or disables studio mode - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> SetStudioModeEnabledAsync(bool studioModeEnabled)
    {
        var data = new JObject();
        data["studioModeEnabled"] = JToken.FromObject(studioModeEnabled);
        return _bridge.SendRequestAsync("SetStudioModeEnabled", data);
    }
    /// <summary>Opens the properties dialog of an input. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> OpenInputPropertiesDialogAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        return _bridge.SendRequestAsync("OpenInputPropertiesDialog", data);
    }
    /// <summary>Opens the filters dialog of an input. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> OpenInputFiltersDialogAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        return _bridge.SendRequestAsync("OpenInputFiltersDialog", data);
    }
    /// <summary>Opens the interact dialog of an input. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> OpenInputInteractDialogAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        return _bridge.SendRequestAsync("OpenInputInteractDialog", data);
    }
    /// <summary>Gets a list of connected monitors and information about them. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetMonitorListAsync()
    => _bridge.SendRequestAsync("GetMonitorList", new JObject());
    /// <summary>Opens a projector for a specific output video mix. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> OpenVideoMixProjectorAsync(string videoMixType, double? monitorIndex = null, string projectorGeometry = null)
    {
        var data = new JObject();
        data["videoMixType"] = JToken.FromObject(videoMixType);
        if (monitorIndex != null) data["monitorIndex"] = JToken.FromObject(monitorIndex);
        if (projectorGeometry != null) data["projectorGeometry"] = JToken.FromObject(projectorGeometry);
        return _bridge.SendRequestAsync("OpenVideoMixProjector", data);
    }
    /// <summary>Opens a projector for a source. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> OpenSourceProjectorAsync(string sourceName = null, string sourceUuid = null, double? monitorIndex = null, string projectorGeometry = null)
    {
        var data = new JObject();
        if (sourceName != null) data["sourceName"] = JToken.FromObject(sourceName);
        if (sourceUuid != null) data["sourceUuid"] = JToken.FromObject(sourceUuid);
        if (monitorIndex != null) data["monitorIndex"] = JToken.FromObject(monitorIndex);
        if (projectorGeometry != null) data["projectorGeometry"] = JToken.FromObject(projectorGeometry);
        return _bridge.SendRequestAsync("OpenSourceProjector", data);
    }
}
