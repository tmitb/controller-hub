using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ObsController.Services.Requests;
public class ScenesRequests : BaseRequests
{
    public ScenesRequests(ObsBridge bridge) : base(bridge) {}
    /// <summary>Gets an array of all scenes in OBS. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetSceneListAsync()
    => _bridge.SendRequestAsync("GetSceneList", new JObject());
    /// <summary>Gets an array of all groups in OBS. Groups in OBS are actually scenes, but renamed and modified. In obs-websocket, we treat them as scenes where we can. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetGroupListAsync()
    => _bridge.SendRequestAsync("GetGroupList", new JObject());
    /// <summary>Gets the current program scene. Note: This request is slated to have the `currentProgram`-prefixed fields removed from in an upcoming RPC version. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetCurrentProgramSceneAsync()
    => _bridge.SendRequestAsync("GetCurrentProgramScene", new JObject());
    /// <summary>Sets the current program scene. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneName">Scene name to set as the current program scene</param>
    /// <param name="sceneUuid">Scene UUID to set as the current program scene</param>
    public Task<JObject> SetCurrentProgramSceneAsync(string sceneName = null, string sceneUuid = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        return _bridge.SendRequestAsync("SetCurrentProgramScene", data);
    }
    /// <summary>Gets the current preview scene. Only available when studio mode is enabled. Note: This request is slated to have the `currentPreview`-prefixed fields removed from in an upcoming RPC version. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetCurrentPreviewSceneAsync()
    => _bridge.SendRequestAsync("GetCurrentPreviewScene", new JObject());
    /// <summary>Sets the current preview scene. Only available when studio mode is enabled. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneName">Scene name to set as the current preview scene</param>
    /// <param name="sceneUuid">Scene UUID to set as the current preview scene</param>
    public Task<JObject> SetCurrentPreviewSceneAsync(string sceneName = null, string sceneUuid = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        return _bridge.SendRequestAsync("SetCurrentPreviewScene", data);
    }
    /// <summary>Creates a new scene in OBS. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneName">Name for the new scene</param>
    public Task<string> CreateSceneAsync(string sceneName)
    {
        var data = new JObject();
        data["sceneName"] = JToken.FromObject(sceneName);
        return _bridge.SendRequestAsync("CreateScene", data);
    }
    /// <summary>Removes a scene from OBS. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneName">Name of the scene to remove</param>
    /// <param name="sceneUuid">UUID of the scene to remove</param>
    public Task<JObject> RemoveSceneAsync(string sceneName = null, string sceneUuid = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        return _bridge.SendRequestAsync("RemoveScene", data);
    }
    /// <summary>Sets the name of a scene (rename). - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneName">Name of the scene to be renamed</param>
    /// <param name="sceneUuid">UUID of the scene to be renamed</param>
    /// <param name="newSceneName">New name for the scene</param>
    public Task<JObject> SetSceneNameAsync(string newSceneName, string sceneName = null, string sceneUuid = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["newSceneName"] = JToken.FromObject(newSceneName);
        return _bridge.SendRequestAsync("SetSceneName", data);
    }
    /// <summary>Gets the scene transition overridden for a scene. Note: A transition UUID response field is not currently able to be implemented as of 2024-1-18. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneName">Name of the scene</param>
    /// <param name="sceneUuid">UUID of the scene</param>
    public Task<JObject> GetSceneSceneTransitionOverrideAsync(string sceneName = null, string sceneUuid = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        return _bridge.SendRequestAsync("GetSceneSceneTransitionOverride", data);
    }
    /// <summary>Sets the scene transition overridden for a scene. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneName">Name of the scene</param>
    /// <param name="sceneUuid">UUID of the scene</param>
    /// <param name="transitionName">Name of the scene transition to use as override. Specify `null` to remove</param>
    /// <param name="transitionDuration">Duration to use for any overridden transition. Specify `null` to remove</param>
    public Task<JObject> SetSceneSceneTransitionOverrideAsync(string sceneName = null, string sceneUuid = null, string transitionName = null, double? transitionDuration = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        if (transitionName != null) data["transitionName"] = JToken.FromObject(transitionName);
        if (transitionDuration != null) data["transitionDuration"] = JToken.FromObject(transitionDuration);
        return _bridge.SendRequestAsync("SetSceneSceneTransitionOverride", data);
    }
}