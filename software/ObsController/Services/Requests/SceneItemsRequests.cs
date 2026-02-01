using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ObsController.Services.Requests;
public class SceneItemsRequests : BaseRequests
{
    public SceneItemsRequests(ObsBridge bridge) : base(bridge) {}
    /// <summary>Gets a list of all scene items in a scene. Scenes only - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneName">Name of the scene to get the items of</param>
    /// <param name="sceneUuid">UUID of the scene to get the items of</param>
    public Task<JObject> GetSceneItemListAsync(string sceneName = null, string sceneUuid = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        return _bridge.SendRequestAsync("GetSceneItemList", data);
    }
    /// <summary>Basically GetSceneItemList, but for groups. Using groups at all in OBS is discouraged, as they are very broken under the hood. Please use nested scenes instead. Groups only - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneName">Name of the group to get the items of</param>
    /// <param name="sceneUuid">UUID of the group to get the items of</param>
    public Task<JObject> GetGroupSceneItemListAsync(string sceneName = null, string sceneUuid = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        return _bridge.SendRequestAsync("GetGroupSceneItemList", data);
    }
    /// <summary>Searches a scene for a source, and returns its id. Scenes and Groups - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneName">Name of the scene or group to search in</param>
    /// <param name="sceneUuid">UUID of the scene or group to search in</param>
    /// <param name="sourceName">Name of the source to find</param>
    /// <param name="searchOffset">Number of matches to skip during search. >= 0 means first forward. -1 means last (top) item</param>
    public Task<double> GetSceneItemIdAsync(string sceneName = null, string sceneUuid = null, string sourceName, double? searchOffset = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sourceName"] = JToken.FromObject(sourceName);
        if (searchOffset != null) data["searchOffset"] = JToken.FromObject(searchOffset);
        return _bridge.SendRequestAsync("GetSceneItemId", data);
    }
    /// <summary>Gets the source associated with a scene item. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.4.0</summary>
    /// <param name="sceneName">Name of the scene the item is in</param>
    /// <param name="sceneUuid">UUID of the scene the item is in</param>
    /// <param name="sceneItemId">Numeric ID of the scene item</param>
    public Task<JObject> GetSceneItemSourceAsync(string sceneName = null, string sceneUuid = null, double sceneItemId)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        return _bridge.SendRequestAsync("GetSceneItemSource", data);
    }
    /// <summary>Creates a new scene item using a source. Scenes only - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneName">Name of the scene to create the new item in</param>
    /// <param name="sceneUuid">UUID of the scene to create the new item in</param>
    /// <param name="sourceName">Name of the source to add to the scene</param>
    /// <param name="sourceUuid">UUID of the source to add to the scene</param>
    /// <param name="sceneItemEnabled">Enable state to apply to the scene item on creation</param>
    public Task<double> CreateSceneItemAsync(string sceneName = null, string sceneUuid = null, string sourceName = null, string sourceUuid = null, bool? sceneItemEnabled = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        if (sourceName != null) data["sourceName"] = JToken.FromObject(sourceName);
        if (sourceUuid != null) data["sourceUuid"] = JToken.FromObject(sourceUuid);
        if (sceneItemEnabled != null) data["sceneItemEnabled"] = JToken.FromObject(sceneItemEnabled);
        return _bridge.SendRequestAsync("CreateSceneItem", data);
    }
    /// <summary>Removes a scene item from a scene. Scenes only - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneName">Name of the scene the item is in</param>
    /// <param name="sceneUuid">UUID of the scene the item is in</param>
    /// <param name="sceneItemId">Numeric ID of the scene item</param>
    public Task<JObject> RemoveSceneItemAsync(string sceneName = null, string sceneUuid = null, double sceneItemId)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        return _bridge.SendRequestAsync("RemoveSceneItem", data);
    }
    /// <summary>Duplicates a scene item, copying all transform and crop info. Scenes only - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneName">Name of the scene the item is in</param>
    /// <param name="sceneUuid">UUID of the scene the item is in</param>
    /// <param name="sceneItemId">Numeric ID of the scene item</param>
    /// <param name="destinationSceneName">Name of the scene to create the duplicated item in</param>
    /// <param name="destinationSceneUuid">UUID of the scene to create the duplicated item in</param>
    public Task<double> DuplicateSceneItemAsync(string sceneName = null, string sceneUuid = null, double sceneItemId, string destinationSceneName = null, string destinationSceneUuid = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        if (destinationSceneName != null) data["destinationSceneName"] = JToken.FromObject(destinationSceneName);
        if (destinationSceneUuid != null) data["destinationSceneUuid"] = JToken.FromObject(destinationSceneUuid);
        return _bridge.SendRequestAsync("DuplicateSceneItem", data);
    }
    /// <summary>Gets the transform and crop info of a scene item. Scenes and Groups - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneName">Name of the scene the item is in</param>
    /// <param name="sceneUuid">UUID of the scene the item is in</param>
    /// <param name="sceneItemId">Numeric ID of the scene item</param>
    public Task<JObject> GetSceneItemTransformAsync(string sceneName = null, string sceneUuid = null, double sceneItemId)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        return _bridge.SendRequestAsync("GetSceneItemTransform", data);
    }
    /// <summary>Sets the transform and crop info of a scene item. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneName">Name of the scene the item is in</param>
    /// <param name="sceneUuid">UUID of the scene the item is in</param>
    /// <param name="sceneItemId">Numeric ID of the scene item</param>
    /// <param name="sceneItemTransform">Object containing scene item transform info to update</param>
    public Task<JObject> SetSceneItemTransformAsync(string sceneName = null, string sceneUuid = null, double sceneItemId, JObject sceneItemTransform)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        data["sceneItemTransform"] = JToken.FromObject(sceneItemTransform);
        return _bridge.SendRequestAsync("SetSceneItemTransform", data);
    }
    /// <summary>Gets the enable state of a scene item. Scenes and Groups - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneName">Name of the scene the item is in</param>
    /// <param name="sceneUuid">UUID of the scene the item is in</param>
    /// <param name="sceneItemId">Numeric ID of the scene item</param>
    public Task<bool> GetSceneItemEnabledAsync(string sceneName = null, string sceneUuid = null, double sceneItemId)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        return _bridge.SendRequestAsync("GetSceneItemEnabled", data);
    }
    /// <summary>Sets the enable state of a scene item. Scenes and Groups - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneName">Name of the scene the item is in</param>
    /// <param name="sceneUuid">UUID of the scene the item is in</param>
    /// <param name="sceneItemId">Numeric ID of the scene item</param>
    /// <param name="sceneItemEnabled">New enable state of the scene item</param>
    public Task<JObject> SetSceneItemEnabledAsync(string sceneName = null, string sceneUuid = null, double sceneItemId, bool sceneItemEnabled)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        data["sceneItemEnabled"] = JToken.FromObject(sceneItemEnabled);
        return _bridge.SendRequestAsync("SetSceneItemEnabled", data);
    }
    /// <summary>Gets the lock state of a scene item. Scenes and Groups - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneName">Name of the scene the item is in</param>
    /// <param name="sceneUuid">UUID of the scene the item is in</param>
    /// <param name="sceneItemId">Numeric ID of the scene item</param>
    public Task<bool> GetSceneItemLockedAsync(string sceneName = null, string sceneUuid = null, double sceneItemId)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        return _bridge.SendRequestAsync("GetSceneItemLocked", data);
    }
    /// <summary>Sets the lock state of a scene item. Scenes and Group - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneName">Name of the scene the item is in</param>
    /// <param name="sceneUuid">UUID of the scene the item is in</param>
    /// <param name="sceneItemId">Numeric ID of the scene item</param>
    /// <param name="sceneItemLocked">New lock state of the scene item</param>
    public Task<JObject> SetSceneItemLockedAsync(string sceneName = null, string sceneUuid = null, double sceneItemId, bool sceneItemLocked)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        data["sceneItemLocked"] = JToken.FromObject(sceneItemLocked);
        return _bridge.SendRequestAsync("SetSceneItemLocked", data);
    }
    /// <summary>Gets the index position of a scene item in a scene. An index of 0 is at the bottom of the source list in the UI. Scenes and Groups - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneName">Name of the scene the item is in</param>
    /// <param name="sceneUuid">UUID of the scene the item is in</param>
    /// <param name="sceneItemId">Numeric ID of the scene item</param>
    public Task<double> GetSceneItemIndexAsync(string sceneName = null, string sceneUuid = null, double sceneItemId)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        return _bridge.SendRequestAsync("GetSceneItemIndex", data);
    }
    /// <summary>Sets the index position of a scene item in a scene. Scenes and Groups - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneName">Name of the scene the item is in</param>
    /// <param name="sceneUuid">UUID of the scene the item is in</param>
    /// <param name="sceneItemId">Numeric ID of the scene item</param>
    /// <param name="sceneItemIndex">New index position of the scene item</param>
    public Task<JObject> SetSceneItemIndexAsync(string sceneName = null, string sceneUuid = null, double sceneItemId, double sceneItemIndex)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        data["sceneItemIndex"] = JToken.FromObject(sceneItemIndex);
        return _bridge.SendRequestAsync("SetSceneItemIndex", data);
    }
    /// <summary>Gets the blend mode of a scene item. Blend modes: - `OBS_BLEND_NORMAL` - `OBS_BLEND_ADDITIVE` - `OBS_BLEND_SUBTRACT` - `OBS_BLEND_SCREEN` - `OBS_BLEND_MULTIPLY` - `OBS_BLEND_LIGHTEN` - `OBS_BLEND_DARKEN` Scenes and Groups - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneName">Name of the scene the item is in</param>
    /// <param name="sceneUuid">UUID of the scene the item is in</param>
    /// <param name="sceneItemId">Numeric ID of the scene item</param>
    public Task<string> GetSceneItemBlendModeAsync(string sceneName = null, string sceneUuid = null, double sceneItemId)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        return _bridge.SendRequestAsync("GetSceneItemBlendMode", data);
    }
    /// <summary>Sets the blend mode of a scene item. Scenes and Groups - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneName">Name of the scene the item is in</param>
    /// <param name="sceneUuid">UUID of the scene the item is in</param>
    /// <param name="sceneItemId">Numeric ID of the scene item</param>
    /// <param name="sceneItemBlendMode">New blend mode</param>
    public Task<JObject> SetSceneItemBlendModeAsync(string sceneName = null, string sceneUuid = null, double sceneItemId, string sceneItemBlendMode)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["sceneItemId"] = JToken.FromObject(sceneItemId);
        data["sceneItemBlendMode"] = JToken.FromObject(sceneItemBlendMode);
        return _bridge.SendRequestAsync("SetSceneItemBlendMode", data);
    }
}
