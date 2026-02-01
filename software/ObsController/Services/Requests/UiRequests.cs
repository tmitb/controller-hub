using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ObsController.Services.Requests;
public class UiRequests
{
    private readonly ObsBridge _bridge;
    public UiRequests(ObsBridge bridge) => _bridge = bridge;
    /// <summary>Gets whether studio is enabled. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<bool> GetStudioModeEnabledAsync()
    => _bridge.SendRequestAsync("GetStudioModeEnabled", new JObject());
    /// <summary>Enables or disables studio mode - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="studioModeEnabled">True == Enabled, False == Disabled</param>
    public Task<JObject> SetStudioModeEnabledAsync(bool studioModeEnabled)
    {
        var data = new JObject();
        data["studioModeEnabled"] = JToken.FromObject(studioModeEnabled);
        return _bridge.SendRequestAsync("SetStudioModeEnabled", data);
    }
    /// <summary>Opens the properties dialog of an input. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of the input to open the dialog of</param>
    /// <param name="inputUuid">UUID of the input to open the dialog of</param>
    public Task<JObject> OpenInputPropertiesDialogAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        return _bridge.SendRequestAsync("OpenInputPropertiesDialog", data);
    }
    /// <summary>Opens the filters dialog of an input. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of the input to open the dialog of</param>
    /// <param name="inputUuid">UUID of the input to open the dialog of</param>
    public Task<JObject> OpenInputFiltersDialogAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        return _bridge.SendRequestAsync("OpenInputFiltersDialog", data);
    }
    /// <summary>Opens the interact dialog of an input. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of the input to open the dialog of</param>
    /// <param name="inputUuid">UUID of the input to open the dialog of</param>
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
    /// <summary>Opens a projector for a specific output video mix. Mix types: - `OBS_WEBSOCKET_VIDEO_MIX_TYPE_PREVIEW` - `OBS_WEBSOCKET_VIDEO_MIX_TYPE_PROGRAM` - `OBS_WEBSOCKET_VIDEO_MIX_TYPE_MULTIVIEW` Note: This request serves to provide feature parity with 4.x. It is very likely to be changed/deprecated in a future release. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="videoMixType">Type of mix to open</param>
    /// <param name="monitorIndex">Monitor index, use `GetMonitorList` to obtain index</param>
    /// <param name="projectorGeometry">Size/Position data for a windowed projector, in Qt Base64 encoded format. Mutually exclusive with `monitorIndex`</param>
    public Task<JObject> OpenVideoMixProjectorAsync(string videoMixType, double? monitorIndex = null, string projectorGeometry = null)
    {
        var data = new JObject();
        data["videoMixType"] = JToken.FromObject(videoMixType);
        if (monitorIndex != null) data["monitorIndex"] = JToken.FromObject(monitorIndex);
        if (projectorGeometry != null) data["projectorGeometry"] = JToken.FromObject(projectorGeometry);
        return _bridge.SendRequestAsync("OpenVideoMixProjector", data);
    }
    /// <summary>Opens a projector for a source. Note: This request serves to provide feature parity with 4.x. It is very likely to be changed/deprecated in a future release. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sourceName">Name of the source to open a projector for</param>
    /// <param name="sourceUuid">UUID of the source to open a projector for</param>
    /// <param name="monitorIndex">Monitor index, use `GetMonitorList` to obtain index</param>
    /// <param name="projectorGeometry">Size/Position data for a windowed projector, in Qt Base64 encoded format. Mutually exclusive with `monitorIndex`</param>
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