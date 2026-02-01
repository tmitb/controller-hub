using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ObsController.Services.Requests;
public class ConfigRequests : BaseRequests
{
    public ConfigRequests(ObsBridge bridge) : base(bridge) {}
    /// <summary>Gets the value of a "slot" from the selected persistent data realm. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="realm">The data realm to select. `OBS_WEBSOCKET_DATA_REALM_GLOBAL` or `OBS_WEBSOCKET_DATA_REALM_PROFILE`</param>
    /// <param name="slotName">The name of the slot to retrieve data from</param>
    public Task<JObject> GetPersistentDataAsync(string realm, string slotName)
    {
        var data = new JObject();
        data["realm"] = JToken.FromObject(realm);
        data["slotName"] = JToken.FromObject(slotName);
        return _bridge.SendRequestAsync("GetPersistentData", data);
    }
    /// <summary>Sets the value of a "slot" from the selected persistent data realm. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="realm">The data realm to select. `OBS_WEBSOCKET_DATA_REALM_GLOBAL` or `OBS_WEBSOCKET_DATA_REALM_PROFILE`</param>
    /// <param name="slotName">The name of the slot to retrieve data from</param>
    /// <param name="slotValue">The value to apply to the slot</param>
    public Task<JObject> SetPersistentDataAsync(string realm, string slotName, object slotValue)
    {
        var data = new JObject();
        data["realm"] = JToken.FromObject(realm);
        data["slotName"] = JToken.FromObject(slotName);
        data["slotValue"] = JToken.FromObject(slotValue);
        return _bridge.SendRequestAsync("SetPersistentData", data);
    }
    /// <summary>Gets an array of all scene collections - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetSceneCollectionListAsync()
    => _bridge.SendRequestAsync("GetSceneCollectionList", new JObject());
    /// <summary>Switches to a scene collection. Note: This will block until the collection has finished changing. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneCollectionName">Name of the scene collection to switch to</param>
    public Task<JObject> SetCurrentSceneCollectionAsync(string sceneCollectionName)
    {
        var data = new JObject();
        data["sceneCollectionName"] = JToken.FromObject(sceneCollectionName);
        return _bridge.SendRequestAsync("SetCurrentSceneCollection", data);
    }
    /// <summary>Creates a new scene collection, switching to it in the process. Note: This will block until the collection has finished changing. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneCollectionName">Name for the new scene collection</param>
    public Task<JObject> CreateSceneCollectionAsync(string sceneCollectionName)
    {
        var data = new JObject();
        data["sceneCollectionName"] = JToken.FromObject(sceneCollectionName);
        return _bridge.SendRequestAsync("CreateSceneCollection", data);
    }
    /// <summary>Gets an array of all profiles - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetProfileListAsync()
    => _bridge.SendRequestAsync("GetProfileList", new JObject());
    /// <summary>Switches to a profile. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="profileName">Name of the profile to switch to</param>
    public Task<JObject> SetCurrentProfileAsync(string profileName)
    {
        var data = new JObject();
        data["profileName"] = JToken.FromObject(profileName);
        return _bridge.SendRequestAsync("SetCurrentProfile", data);
    }
    /// <summary>Creates a new profile, switching to it in the process - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="profileName">Name for the new profile</param>
    public Task<JObject> CreateProfileAsync(string profileName)
    {
        var data = new JObject();
        data["profileName"] = JToken.FromObject(profileName);
        return _bridge.SendRequestAsync("CreateProfile", data);
    }
    /// <summary>Removes a profile. If the current profile is chosen, it will change to a different profile first. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="profileName">Name of the profile to remove</param>
    public Task<JObject> RemoveProfileAsync(string profileName)
    {
        var data = new JObject();
        data["profileName"] = JToken.FromObject(profileName);
        return _bridge.SendRequestAsync("RemoveProfile", data);
    }
    /// <summary>Gets a parameter from the current profile's configuration. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="parameterCategory">Category of the parameter to get</param>
    /// <param name="parameterName">Name of the parameter to get</param>
    public Task<JObject> GetProfileParameterAsync(string parameterCategory, string parameterName)
    {
        var data = new JObject();
        data["parameterCategory"] = JToken.FromObject(parameterCategory);
        data["parameterName"] = JToken.FromObject(parameterName);
        return _bridge.SendRequestAsync("GetProfileParameter", data);
    }
    /// <summary>Sets the value of a parameter in the current profile's configuration. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="parameterCategory">Category of the parameter to set</param>
    /// <param name="parameterName">Name of the parameter to set</param>
    /// <param name="parameterValue">Value of the parameter to set. Use `null` to delete</param>
    public Task<JObject> SetProfileParameterAsync(string parameterCategory, string parameterName, string parameterValue)
    {
        var data = new JObject();
        data["parameterCategory"] = JToken.FromObject(parameterCategory);
        data["parameterName"] = JToken.FromObject(parameterName);
        data["parameterValue"] = JToken.FromObject(parameterValue);
        return _bridge.SendRequestAsync("SetProfileParameter", data);
    }
    /// <summary>Gets the current video settings. Note: To get the true FPS value, divide the FPS numerator by the FPS denominator. Example: `60000/1001` - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetVideoSettingsAsync()
    => _bridge.SendRequestAsync("GetVideoSettings", new JObject());
    /// <summary>Sets the current video settings. Note: Fields must be specified in pairs. For example, you cannot set only `baseWidth` without needing to specify `baseHeight`. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="fpsNumerator">Numerator of the fractional FPS value</param>
    /// <param name="fpsDenominator">Denominator of the fractional FPS value</param>
    /// <param name="baseWidth">Width of the base (canvas) resolution in pixels</param>
    /// <param name="baseHeight">Height of the base (canvas) resolution in pixels</param>
    /// <param name="outputWidth">Width of the output resolution in pixels</param>
    /// <param name="outputHeight">Height of the output resolution in pixels</param>
    public Task<JObject> SetVideoSettingsAsync(double? fpsNumerator = null, double? fpsDenominator = null, double? baseWidth = null, double? baseHeight = null, double? outputWidth = null, double? outputHeight = null)
    {
        var data = new JObject();
        if (fpsNumerator != null) data["fpsNumerator"] = JToken.FromObject(fpsNumerator);
        if (fpsDenominator != null) data["fpsDenominator"] = JToken.FromObject(fpsDenominator);
        if (baseWidth != null) data["baseWidth"] = JToken.FromObject(baseWidth);
        if (baseHeight != null) data["baseHeight"] = JToken.FromObject(baseHeight);
        if (outputWidth != null) data["outputWidth"] = JToken.FromObject(outputWidth);
        if (outputHeight != null) data["outputHeight"] = JToken.FromObject(outputHeight);
        return _bridge.SendRequestAsync("SetVideoSettings", data);
    }
    /// <summary>Gets the current stream service settings (stream destination). - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetStreamServiceSettingsAsync()
    => _bridge.SendRequestAsync("GetStreamServiceSettings", new JObject());
    /// <summary>Sets the current stream service settings (stream destination). Note: Simple RTMP settings can be set with type `rtmp_custom` and the settings fields `server` and `key`. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="streamServiceType">Type of stream service to apply. Example: `rtmp_common` or `rtmp_custom`</param>
    /// <param name="streamServiceSettings">Settings to apply to the service</param>
    public Task<JObject> SetStreamServiceSettingsAsync(string streamServiceType, JObject streamServiceSettings)
    {
        var data = new JObject();
        data["streamServiceType"] = JToken.FromObject(streamServiceType);
        data["streamServiceSettings"] = JToken.FromObject(streamServiceSettings);
        return _bridge.SendRequestAsync("SetStreamServiceSettings", data);
    }
    /// <summary>Gets the current directory that the record output is set to. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public async Task<string> GetRecordDirectoryAsync()
    {
        var resp = await _bridge.SendRequestAsync("GetRecordDirectory", new JObject());
        return resp["recordDirectory"].ToObject<string>();
    }
    /// <summary>Sets the current directory that the record output writes files to. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.3.0</summary>
    /// <param name="recordDirectory">Output directory</param>
    public Task<JObject> SetRecordDirectoryAsync(string recordDirectory)
    {
        var data = new JObject();
        data["recordDirectory"] = JToken.FromObject(recordDirectory);
        return _bridge.SendRequestAsync("SetRecordDirectory", data);
    }
}