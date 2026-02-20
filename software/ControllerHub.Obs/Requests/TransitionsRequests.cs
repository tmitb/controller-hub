using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ControllerHub.Obs.Requests;
public class TransitionsRequests : BaseRequests
{
    public TransitionsRequests(ObsBridge bridge) : base(bridge) {}
    /// <summary>Gets an array of all available transition kinds. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetTransitionKindListAsync()
    => _bridge.SendRequestAsync("GetTransitionKindList", new JObject());
    /// <summary>Gets an array of all scene transitions in OBS. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetSceneTransitionListAsync()
    => _bridge.SendRequestAsync("GetSceneTransitionList", new JObject());
    /// <summary>Gets information about the current scene transition. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetCurrentSceneTransitionAsync()
    => _bridge.SendRequestAsync("GetCurrentSceneTransition", new JObject());
    /// <summary>Sets the current scene transition. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> SetCurrentSceneTransitionAsync(string transitionName)
    {
        var data = new JObject();
        data["transitionName"] = JToken.FromObject(transitionName);
        return _bridge.SendRequestAsync("SetCurrentSceneTransition", data);
    }
    /// <summary>Sets the duration of the current scene transition. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> SetCurrentSceneTransitionDurationAsync(double transitionDuration)
    {
        var data = new JObject();
        data["transitionDuration"] = JToken.FromObject(transitionDuration);
        return _bridge.SendRequestAsync("SetCurrentSceneTransitionDuration", data);
    }
    /// <summary>Sets the settings of the current scene transition. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> SetCurrentSceneTransitionSettingsAsync(JObject transitionSettings, bool? overlay = null)
    {
        var data = new JObject();
        data["transitionSettings"] = JToken.FromObject(transitionSettings);
        if (overlay != null) data["overlay"] = JToken.FromObject(overlay);
        return _bridge.SendRequestAsync("SetCurrentSceneTransitionSettings", data);
    }
    /// <summary>Gets the cursor position of the current scene transition. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public async Task<double> GetCurrentSceneTransitionCursorAsync()
    {
        var resp = await _bridge.SendRequestAsync("GetCurrentSceneTransitionCursor", new JObject());
        return resp["transitionCursor"].ToObject<double>();
    }
    /// <summary>Triggers the current scene transition. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0 ---</summary>
    public Task<JObject> TriggerStudioModeTransitionAsync()
    => _bridge.SendRequestAsync("TriggerStudioModeTransition", new JObject());
    /// <summary>Sets the position of the TBar. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> SetTBarPositionAsync(double position, bool? release = null)
    {
        var data = new JObject();
        data["position"] = JToken.FromObject(position);
        if (release != null) data["release"] = JToken.FromObject(release);
        return _bridge.SendRequestAsync("SetTBarPosition", data);
    }
}
