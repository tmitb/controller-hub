using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ObsController.Services.Requests;
public class InputsRequests : BaseRequests
{
    public InputsRequests(ObsBridge bridge) : base(bridge) {}
    /// <summary>Gets an array of all inputs in OBS. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputKind">Restrict the array to only inputs of the specified kind</param>
    public Task<JObject> GetInputListAsync(string inputKind = null)
    {
        var data = new JObject();
        if (inputKind != null) data["inputKind"] = JToken.FromObject(inputKind);
        return _bridge.SendRequestAsync("GetInputList", data);
    }
    /// <summary>Gets an array of all available input kinds in OBS. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="unversioned">True == Return all kinds as unversioned, False == Return with version suffixes (if available)</param>
    public Task<JObject> GetInputKindListAsync(bool? unversioned = null)
    {
        var data = new JObject();
        if (unversioned != null) data["unversioned"] = JToken.FromObject(unversioned);
        return _bridge.SendRequestAsync("GetInputKindList", data);
    }
    /// <summary>Gets the names of all special inputs. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetSpecialInputsAsync()
    => _bridge.SendRequestAsync("GetSpecialInputs", new JObject());
    /// <summary>Creates a new input, adding it as a scene item to the specified scene. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sceneName">Name of the scene to add the input to as a scene item</param>
    /// <param name="sceneUuid">UUID of the scene to add the input to as a scene item</param>
    /// <param name="inputName">Name of the new input to created</param>
    /// <param name="inputKind">The kind of input to be created</param>
    /// <param name="inputSettings">Settings object to initialize the input with</param>
    /// <param name="sceneItemEnabled">Whether to set the created scene item to enabled or disabled</param>
    public Task<JObject> CreateInputAsync(string inputName, string inputKind, string sceneName = null, string sceneUuid = null, JObject inputSettings = null, bool? sceneItemEnabled = null)
    {
        var data = new JObject();
        if (sceneName != null) data["sceneName"] = JToken.FromObject(sceneName);
        if (sceneUuid != null) data["sceneUuid"] = JToken.FromObject(sceneUuid);
        data["inputName"] = JToken.FromObject(inputName);
        data["inputKind"] = JToken.FromObject(inputKind);
        if (inputSettings != null) data["inputSettings"] = JToken.FromObject(inputSettings);
        if (sceneItemEnabled != null) data["sceneItemEnabled"] = JToken.FromObject(sceneItemEnabled);
        return _bridge.SendRequestAsync("CreateInput", data);
    }
    /// <summary>Removes an existing input. Note: Will immediately remove all associated scene items. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of the input to remove</param>
    /// <param name="inputUuid">UUID of the input to remove</param>
    public Task<JObject> RemoveInputAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        return _bridge.SendRequestAsync("RemoveInput", data);
    }
    /// <summary>Sets the name of an input (rename). - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Current input name</param>
    /// <param name="inputUuid">Current input UUID</param>
    /// <param name="newInputName">New name for the input</param>
    public Task<JObject> SetInputNameAsync(string newInputName, string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["newInputName"] = JToken.FromObject(newInputName);
        return _bridge.SendRequestAsync("SetInputName", data);
    }
    /// <summary>Gets the default settings for an input kind. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputKind">Input kind to get the default settings for</param>
    public Task<JObject> GetInputDefaultSettingsAsync(string inputKind)
    {
        var data = new JObject();
        data["inputKind"] = JToken.FromObject(inputKind);
        return _bridge.SendRequestAsync("GetInputDefaultSettings", data);
    }
    /// <summary>Gets the settings of an input. Note: Does not include defaults. To create the entire settings object, overlay `inputSettings` over the `defaultInputSettings` provided by `GetInputDefaultSettings`. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of the input to get the settings of</param>
    /// <param name="inputUuid">UUID of the input to get the settings of</param>
    public Task<JObject> GetInputSettingsAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        return _bridge.SendRequestAsync("GetInputSettings", data);
    }
    /// <summary>Sets the settings of an input. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of the input to set the settings of</param>
    /// <param name="inputUuid">UUID of the input to set the settings of</param>
    /// <param name="inputSettings">Object of settings to apply</param>
    /// <param name="overlay">True == apply the settings on top of existing ones, False == reset the input to its defaults, then apply settings.</param>
    public Task<JObject> SetInputSettingsAsync(JObject inputSettings, string inputName = null, string inputUuid = null, bool? overlay = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["inputSettings"] = JToken.FromObject(inputSettings);
        if (overlay != null) data["overlay"] = JToken.FromObject(overlay);
        return _bridge.SendRequestAsync("SetInputSettings", data);
    }
    /// <summary>Gets the audio mute state of an input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of input to get the mute state of</param>
    /// <param name="inputUuid">UUID of input to get the mute state of</param>
    public Task<bool> GetInputMuteAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        return _bridge.SendRequestAsync("GetInputMute", data);
    }
    /// <summary>Sets the audio mute state of an input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of the input to set the mute state of</param>
    /// <param name="inputUuid">UUID of the input to set the mute state of</param>
    /// <param name="inputMuted">Whether to mute the input or not</param>
    public Task<JObject> SetInputMuteAsync(bool inputMuted, string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["inputMuted"] = JToken.FromObject(inputMuted);
        return _bridge.SendRequestAsync("SetInputMute", data);
    }
    /// <summary>Toggles the audio mute state of an input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of the input to toggle the mute state of</param>
    /// <param name="inputUuid">UUID of the input to toggle the mute state of</param>
    public Task<bool> ToggleInputMuteAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        return _bridge.SendRequestAsync("ToggleInputMute", data);
    }
    /// <summary>Gets the current volume setting of an input. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of the input to get the volume of</param>
    /// <param name="inputUuid">UUID of the input to get the volume of</param>
    public Task<JObject> GetInputVolumeAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        return _bridge.SendRequestAsync("GetInputVolume", data);
    }
    /// <summary>Sets the volume setting of an input. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of the input to set the volume of</param>
    /// <param name="inputUuid">UUID of the input to set the volume of</param>
    /// <param name="inputVolumeMul">Volume setting in mul</param>
    /// <param name="inputVolumeDb">Volume setting in dB</param>
    public Task<JObject> SetInputVolumeAsync(string inputName = null, string inputUuid = null, double? inputVolumeMul = null, double? inputVolumeDb = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        if (inputVolumeMul != null) data["inputVolumeMul"] = JToken.FromObject(inputVolumeMul);
        if (inputVolumeDb != null) data["inputVolumeDb"] = JToken.FromObject(inputVolumeDb);
        return _bridge.SendRequestAsync("SetInputVolume", data);
    }
    /// <summary>Gets the audio balance of an input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of the input to get the audio balance of</param>
    /// <param name="inputUuid">UUID of the input to get the audio balance of</param>
    public Task<double> GetInputAudioBalanceAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        return _bridge.SendRequestAsync("GetInputAudioBalance", data);
    }
    /// <summary>Sets the audio balance of an input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of the input to set the audio balance of</param>
    /// <param name="inputUuid">UUID of the input to set the audio balance of</param>
    /// <param name="inputAudioBalance">New audio balance value</param>
    public Task<JObject> SetInputAudioBalanceAsync(double inputAudioBalance, string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["inputAudioBalance"] = JToken.FromObject(inputAudioBalance);
        return _bridge.SendRequestAsync("SetInputAudioBalance", data);
    }
    /// <summary>Gets the audio sync offset of an input. Note: The audio sync offset can be negative too! - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of the input to get the audio sync offset of</param>
    /// <param name="inputUuid">UUID of the input to get the audio sync offset of</param>
    public Task<double> GetInputAudioSyncOffsetAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        return _bridge.SendRequestAsync("GetInputAudioSyncOffset", data);
    }
    /// <summary>Sets the audio sync offset of an input. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of the input to set the audio sync offset of</param>
    /// <param name="inputUuid">UUID of the input to set the audio sync offset of</param>
    /// <param name="inputAudioSyncOffset">New audio sync offset in milliseconds</param>
    public Task<JObject> SetInputAudioSyncOffsetAsync(double inputAudioSyncOffset, string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["inputAudioSyncOffset"] = JToken.FromObject(inputAudioSyncOffset);
        return _bridge.SendRequestAsync("SetInputAudioSyncOffset", data);
    }
    /// <summary>Gets the audio monitor type of an input. The available audio monitor types are: - `OBS_MONITORING_TYPE_NONE` - `OBS_MONITORING_TYPE_MONITOR_ONLY` - `OBS_MONITORING_TYPE_MONITOR_AND_OUTPUT` - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of the input to get the audio monitor type of</param>
    /// <param name="inputUuid">UUID of the input to get the audio monitor type of</param>
    public Task<string> GetInputAudioMonitorTypeAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        return _bridge.SendRequestAsync("GetInputAudioMonitorType", data);
    }
    /// <summary>Sets the audio monitor type of an input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of the input to set the audio monitor type of</param>
    /// <param name="inputUuid">UUID of the input to set the audio monitor type of</param>
    /// <param name="monitorType">Audio monitor type</param>
    public Task<JObject> SetInputAudioMonitorTypeAsync(string monitorType, string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["monitorType"] = JToken.FromObject(monitorType);
        return _bridge.SendRequestAsync("SetInputAudioMonitorType", data);
    }
    /// <summary>Gets the enable state of all audio tracks of an input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of the input</param>
    /// <param name="inputUuid">UUID of the input</param>
    public Task<JObject> GetInputAudioTracksAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        return _bridge.SendRequestAsync("GetInputAudioTracks", data);
    }
    /// <summary>Sets the enable state of audio tracks of an input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of the input</param>
    /// <param name="inputUuid">UUID of the input</param>
    /// <param name="inputAudioTracks">Track settings to apply</param>
    public Task<JObject> SetInputAudioTracksAsync(JObject inputAudioTracks, string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["inputAudioTracks"] = JToken.FromObject(inputAudioTracks);
        return _bridge.SendRequestAsync("SetInputAudioTracks", data);
    }
    /// <summary>Gets the deinterlace mode of an input. Deinterlace Modes: - `OBS_DEINTERLACE_MODE_DISABLE` - `OBS_DEINTERLACE_MODE_DISCARD` - `OBS_DEINTERLACE_MODE_RETRO` - `OBS_DEINTERLACE_MODE_BLEND` - `OBS_DEINTERLACE_MODE_BLEND_2X` - `OBS_DEINTERLACE_MODE_LINEAR` - `OBS_DEINTERLACE_MODE_LINEAR_2X` - `OBS_DEINTERLACE_MODE_YADIF` - `OBS_DEINTERLACE_MODE_YADIF_2X` Note: Deinterlacing functionality is restricted to async inputs only. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.6.0</summary>
    /// <param name="inputName">Name of the input</param>
    /// <param name="inputUuid">UUID of the input</param>
    public Task<string> GetInputDeinterlaceModeAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        return _bridge.SendRequestAsync("GetInputDeinterlaceMode", data);
    }
    /// <summary>Sets the deinterlace mode of an input. Note: Deinterlacing functionality is restricted to async inputs only. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.6.0</summary>
    /// <param name="inputName">Name of the input</param>
    /// <param name="inputUuid">UUID of the input</param>
    /// <param name="inputDeinterlaceMode">Deinterlace mode for the input</param>
    public Task<JObject> SetInputDeinterlaceModeAsync(string inputDeinterlaceMode, string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["inputDeinterlaceMode"] = JToken.FromObject(inputDeinterlaceMode);
        return _bridge.SendRequestAsync("SetInputDeinterlaceMode", data);
    }
    /// <summary>Gets the deinterlace field order of an input. Deinterlace Field Orders: - `OBS_DEINTERLACE_FIELD_ORDER_TOP` - `OBS_DEINTERLACE_FIELD_ORDER_BOTTOM` Note: Deinterlacing functionality is restricted to async inputs only. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.6.0</summary>
    /// <param name="inputName">Name of the input</param>
    /// <param name="inputUuid">UUID of the input</param>
    public Task<string> GetInputDeinterlaceFieldOrderAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        return _bridge.SendRequestAsync("GetInputDeinterlaceFieldOrder", data);
    }
    /// <summary>Sets the deinterlace field order of an input. Note: Deinterlacing functionality is restricted to async inputs only. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.6.0</summary>
    /// <param name="inputName">Name of the input</param>
    /// <param name="inputUuid">UUID of the input</param>
    /// <param name="inputDeinterlaceFieldOrder">Deinterlace field order for the input</param>
    public Task<JObject> SetInputDeinterlaceFieldOrderAsync(string inputDeinterlaceFieldOrder, string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["inputDeinterlaceFieldOrder"] = JToken.FromObject(inputDeinterlaceFieldOrder);
        return _bridge.SendRequestAsync("SetInputDeinterlaceFieldOrder", data);
    }
    /// <summary>Gets the items of a list property from an input's properties. Note: Use this in cases where an input provides a dynamic, selectable list of items. For example, display capture, where it provides a list of available displays. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of the input</param>
    /// <param name="inputUuid">UUID of the input</param>
    /// <param name="propertyName">Name of the list property to get the items of</param>
    public Task<JObject> GetInputPropertiesListPropertyItemsAsync(string propertyName, string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["propertyName"] = JToken.FromObject(propertyName);
        return _bridge.SendRequestAsync("GetInputPropertiesListPropertyItems", data);
    }
    /// <summary>Presses a button in the properties of an input. Some known `propertyName` values are: - `refreshnocache` - Browser source reload button Note: Use this in cases where there is a button in the properties of an input that cannot be accessed in any other way. For example, browser sources, where there is a refresh button. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of the input</param>
    /// <param name="inputUuid">UUID of the input</param>
    /// <param name="propertyName">Name of the button property to press</param>
    public Task<JObject> PressInputPropertiesButtonAsync(string propertyName, string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["propertyName"] = JToken.FromObject(propertyName);
        return _bridge.SendRequestAsync("PressInputPropertiesButton", data);
    }
}