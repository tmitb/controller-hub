using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ObsController.Services.Requests;
public class TransitionsRequests : BaseRequests
{
    public TransitionsRequests(ObsBridge bridge) : base(bridge) {}
    /// <summary>Gets an array of all available transition kinds. Similar to `GetInputKindList` - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetTransitionKindListAsync()
    => _bridge.SendRequestAsync("GetTransitionKindList", new JObject());
    /// <summary>Gets an array of all scene transitions in OBS. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetSceneTransitionListAsync()
    => _bridge.SendRequestAsync("GetSceneTransitionList", new JObject());
    /// <summary>Gets information about the current scene transition. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetCurrentSceneTransitionAsync()
    => _bridge.SendRequestAsync("GetCurrentSceneTransition", new JObject());
    /// <summary>Sets the current scene transition. Small note: While the namespace of scene transitions is generally unique, that uniqueness is not a guarantee as it is with other resources like inputs. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="transitionName">Name of the transition to make active</param>
    public Task<JObject> SetCurrentSceneTransitionAsync(string transitionName)
    {
        var data = new JObject();
        data["transitionName"] = JToken.FromObject(transitionName);
        return _bridge.SendRequestAsync("SetCurrentSceneTransition", data);
    }
    /// <summary>Sets the duration of the current scene transition, if it is not fixed. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="transitionDuration">Duration in milliseconds</param>
    public Task<JObject> SetCurrentSceneTransitionDurationAsync(double transitionDuration)
    {
        var data = new JObject();
        data["transitionDuration"] = JToken.FromObject(transitionDuration);
        return _bridge.SendRequestAsync("SetCurrentSceneTransitionDuration", data);
    }
    /// <summary>Sets the settings of the current scene transition. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="transitionSettings">Settings object to apply to the transition. Can be `{}`</param>
    /// <param name="overlay">Whether to overlay over the current settings or replace them</param>
    public Task<JObject> SetCurrentSceneTransitionSettingsAsync(JObject transitionSettings, bool? overlay = null)
    {
        var data = new JObject();
        data["transitionSettings"] = JToken.FromObject(transitionSettings);
        if (overlay != null) data["overlay"] = JToken.FromObject(overlay);
        return _bridge.SendRequestAsync("SetCurrentSceneTransitionSettings", data);
    }
    /// <summary>Gets the cursor position of the current scene transition. Note: `transitionCursor` will return 1.0 when the transition is inactive. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<double> GetCurrentSceneTransitionCursorAsync()
    => _bridge.SendRequestAsync("GetCurrentSceneTransitionCursor", new JObject());
    /// <summary>Triggers the current scene transition. Same functionality as the `Transition` button in studio mode. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0 ---</summary>
    public Task<JObject> TriggerStudioModeTransitionAsync()
    => _bridge.SendRequestAsync("TriggerStudioModeTransition", new JObject());
    /// <summary>Sets the position of the TBar. **Very important note**: This will be deprecated and replaced in a future version of obs-websocket. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="position">New position</param>
    /// <param name="release">Whether to release the TBar. Only set `false` if you know that you will be sending another position update</param>
    public Task<JObject> SetTBarPositionAsync(double position, bool? release = null)
    {
        var data = new JObject();
        data["position"] = JToken.FromObject(position);
        if (release != null) data["release"] = JToken.FromObject(release);
        return _bridge.SendRequestAsync("SetTBarPosition", data);
    }
}
