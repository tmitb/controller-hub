using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ControllerHub.Obs.Requests;
public class SceneItemsRequests : BaseRequests
{
    public SceneItemsRequests(ObsBridge bridge) : base(bridge) {}
    /// <summary>Gets a list of all scene items in a scene. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetSceneItemListAsync(string sceneName = null, string sceneUuid = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        return _bridge.SendRequestAsync("GetSceneItemList", data);
    }
    /// <summary>Gets a list of all scene items in a group. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetGroupSceneItemListAsync(string sceneName = null, string sceneUuid = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        return _bridge.SendRequestAsync("GetGroupSceneItemList", data);
    }
    /// <summary>Searches a scene for a source, and returns its id. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public async Task<double> GetSceneItemIdAsync(string sourceName, string sceneName = null, string sceneUuid = null, double? searchOffset = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sourceName"] = JToken.FromObject(sourceName);
        if (searchOffset != null) data["searchOffset"] = JToken.FromObject(searchOffset);
        var resp = await _bridge.SendRequestAsync("GetSceneItemId", data);
        return resp["sceneItemId"].ToObject<double>();
    }
    /// <summary>Gets the source associated with a scene item. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.4.0</summary>
    public Task<JObject> GetSceneItemSourceAsync(double sceneItemId, string sceneName = null, string sceneUuid = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        return _bridge.SendRequestAsync("GetSceneItemSource", data);
    }
    /// <summary>Creates a new scene item using a source. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public async Task<double> CreateSceneItemAsync(string sceneName = null, string sceneUuid = null, string sourceName = null, string sourceUuid = null, bool? sceneItemEnabled = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        if (sourceName != null) data["sourceName"] = JToken.FromObject(sourceName);
        if (sourceUuid != null) data["sourceUuid"] = JToken.FromObject(sourceUuid);
        if (sceneItemEnabled != null) data["sceneItemEnabled"] = JToken.FromObject(sceneItemEnabled);
        var resp = await _bridge.SendRequestAsync("CreateSceneItem", data);
        return resp["sceneItemId"].ToObject<double>();
    }
    /// <summary>Removes a scene item from a scene. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> RemoveSceneItemAsync(double sceneItemId, string sceneName = null, string sceneUuid = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        return _bridge.SendRequestAsync("RemoveSceneItem", data);
    }
    /// <summary>Duplicates a scene item, copying all transform and crop info. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public async Task<double> DuplicateSceneItemAsync(double sceneItemId, string sceneName = null, string sceneUuid = null, string destinationSceneName = null, string destinationSceneUuid = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        if (destinationSceneName != null) data["destinationSceneName"] = JToken.FromObject(destinationSceneName);
        if (destinationSceneUuid != null) data["destinationSceneUuid"] = JToken.FromObject(destinationSceneUuid);
        var resp = await _bridge.SendRequestAsync("DuplicateSceneItem", data);
        return resp["sceneItemId"].ToObject<double>();
    }
    /// <summary>Gets the transform and crop info of a scene item. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetSceneItemTransformAsync(double sceneItemId, string sceneName = null, string sceneUuid = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        return _bridge.SendRequestAsync("GetSceneItemTransform", data);
    }
    /// <summary>Sets the transform and crop info of a scene item. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> SetSceneItemTransformAsync(double sceneItemId, JObject sceneItemTransform, string sceneName = null, string sceneUuid = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        data["sceneItemTransform"] = JToken.FromObject(sceneItemTransform);
        return _bridge.SendRequestAsync("SetSceneItemTransform", data);
    }
    /// <summary>Gets the enable state of a scene item. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public async Task<bool> GetSceneItemEnabledAsync(double sceneItemId, string sceneName = null, string sceneUuid = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        var resp = await _bridge.SendRequestAsync("GetSceneItemEnabled", data);
        return resp["sceneItemEnabled"].ToObject<bool>();
    }
    /// <summary>Sets the enable state of a scene item. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> SetSceneItemEnabledAsync(double sceneItemId, bool sceneItemEnabled, string sceneName = null, string sceneUuid = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        data["sceneItemEnabled"] = JToken.FromObject(sceneItemEnabled);
        return _bridge.SendRequestAsync("SetSceneItemEnabled", data);
    }
    /// <summary>Gets the lock state of a scene item. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public async Task<bool> GetSceneItemLockedAsync(double sceneItemId, string sceneName = null, string sceneUuid = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        var resp = await _bridge.SendRequestAsync("GetSceneItemLocked", data);
        return resp["sceneItemLocked"].ToObject<bool>();
    }
    /// <summary>Sets the lock state of a scene item. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> SetSceneItemLockedAsync(double sceneItemId, bool sceneItemLocked, string sceneName = null, string sceneUuid = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        data["sceneItemLocked"] = JToken.FromObject(sceneItemLocked);
        return _bridge.SendRequestAsync("SetSceneItemLocked", data);
    }
    /// <summary>Gets the index position of a scene item in a scene. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public async Task<double> GetSceneItemIndexAsync(double sceneItemId, string sceneName = null, string sceneUuid = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        var resp = await _bridge.SendRequestAsync("GetSceneItemIndex", data);
        return resp["sceneItemIndex"].ToObject<double>();
    }
    /// <summary>Sets the index position of a scene item in a scene. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> SetSceneItemIndexAsync(double sceneItemId, double sceneItemIndex, string sceneName = null, string sceneUuid = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        data["sceneItemIndex"] = JToken.FromObject(sceneItemIndex);
        return _bridge.SendRequestAsync("SetSceneItemIndex", data);
    }
    /// <summary>Gets the blend mode of a scene item. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public async Task<string> GetSceneItemBlendModeAsync(double sceneItemId, string sceneName = null, string sceneUuid = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        var resp = await _bridge.SendRequestAsync("GetSceneItemBlendMode", data);
        return resp["sceneItemBlendMode"].ToObject<string>();
    }
    /// <summary>Sets the blend mode of a scene item. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> SetSceneItemBlendModeAsync(double sceneItemId, string sceneItemBlendMode, string sceneName = null, string sceneUuid = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        data["sceneItemBlendMode"] = JToken.FromObject(sceneItemBlendMode);
        return _bridge.SendRequestAsync("SetSceneItemBlendMode", data);
    }
}
