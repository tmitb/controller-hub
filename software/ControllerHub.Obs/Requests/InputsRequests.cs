using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ControllerHub.Obs.Requests;
public class InputsRequests : BaseRequests
{
    public InputsRequests(ObsBridge bridge) : base(bridge) {}
    /// <summary>Gets an array of all inputs in OBS. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetInputListAsync(string inputKind = null)
    {
        var data = new JObject();
        if (inputKind != null) data["inputKind"] = JToken.FromObject(inputKind);
        return _bridge.SendRequestAsync("GetInputList", data);
    }
    /// <summary>Gets an array of all available input kinds in OBS. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetInputKindListAsync(bool? unversioned = null)
    {
        var data = new JObject();
        if (unversioned != null) data["unversioned"] = JToken.FromObject(unversioned);
        return _bridge.SendRequestAsync("GetInputKindList", data);
    }
    /// <summary>Gets the names of all special inputs. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetSpecialInputsAsync()
    => _bridge.SendRequestAsync("GetSpecialInputs", new JObject());
    /// <summary>Creates a new input. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
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
    /// <summary>Removes an existing input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> RemoveInputAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        return _bridge.SendRequestAsync("RemoveInput", data);
    }
    /// <summary>Sets the name of an input (rename). - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> SetInputNameAsync(string newInputName, string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["newInputName"] = JToken.FromObject(newInputName);
        return _bridge.SendRequestAsync("SetInputName", data);
    }
    /// <summary>Gets the default settings for an input kind. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetInputDefaultSettingsAsync(string inputKind)
    {
        var data = new JObject();
        data["inputKind"] = JToken.FromObject(inputKind);
        return _bridge.SendRequestAsync("GetInputDefaultSettings", data);
    }
    /// <summary>Gets the settings of an input. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetInputSettingsAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        return _bridge.SendRequestAsync("GetInputSettings", data);
    }
    /// <summary>Sets the settings of an input. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
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
    public async Task<bool> GetInputMuteAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        var resp = await _bridge.SendRequestAsync("GetInputMute", data);
        return resp["inputMuted"].ToObject<bool>();
    }
    /// <summary>Sets the audio mute state of an input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> SetInputMuteAsync(bool inputMuted, string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["inputMuted"] = JToken.FromObject(inputMuted);
        return _bridge.SendRequestAsync("SetInputMute", data);
    }
    /// <summary>Toggles the audio mute state of an input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public async Task<bool> ToggleInputMuteAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        var resp = await _bridge.SendRequestAsync("ToggleInputMute", data);
        return resp["inputMuted"].ToObject<bool>();
    }
    /// <summary>Gets the current volume setting of an input. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetInputVolumeAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        return _bridge.SendRequestAsync("GetInputVolume", data);
    }
    /// <summary>Sets the volume setting of an input. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
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
    public async Task<double> GetInputAudioBalanceAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        var resp = await _bridge.SendRequestAsync("GetInputAudioBalance", data);
        return resp["inputAudioBalance"].ToObject<double>();
    }
    /// <summary>Sets the audio balance of an input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> SetInputAudioBalanceAsync(double inputAudioBalance, string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["inputAudioBalance"] = JToken.FromObject(inputAudioBalance);
        return _bridge.SendRequestAsync("SetInputAudioBalance", data);
    }
    /// <summary>Gets the audio sync offset of an input. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public async Task<double> GetInputAudioSyncOffsetAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        var resp = await _bridge.SendRequestAsync("GetInputAudioSyncOffset", data);
        return resp["inputAudioSyncOffset"].ToObject<double>();
    }
    /// <summary>Sets the audio sync offset of an input. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> SetInputAudioSyncOffsetAsync(double inputAudioSyncOffset, string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["inputAudioSyncOffset"] = JToken.FromObject(inputAudioSyncOffset);
        return _bridge.SendRequestAsync("SetInputAudioSyncOffset", data);
    }
    /// <summary>Gets the audio monitor type of an input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public async Task<string> GetInputAudioMonitorTypeAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        var resp = await _bridge.SendRequestAsync("GetInputAudioMonitorType", data);
        return resp["monitorType"].ToObject<string>();
    }
    /// <summary>Sets the audio monitor type of an input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> SetInputAudioMonitorTypeAsync(string monitorType, string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["monitorType"] = JToken.FromObject(monitorType);
        return _bridge.SendRequestAsync("SetInputAudioMonitorType", data);
    }
    /// <summary>Gets the enable state of all audio tracks of an input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetInputAudioTracksAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        return _bridge.SendRequestAsync("GetInputAudioTracks", data);
    }
    /// <summary>Sets the enable state of audio tracks of an input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> SetInputAudioTracksAsync(JObject inputAudioTracks, string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["inputAudioTracks"] = JToken.FromObject(inputAudioTracks);
        return _bridge.SendRequestAsync("SetInputAudioTracks", data);
    }
    /// <summary>Gets the deinterlace mode of an input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.6.0</summary>
    public async Task<string> GetInputDeinterlaceModeAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        var resp = await _bridge.SendRequestAsync("GetInputDeinterlaceMode", data);
        return resp["inputDeinterlaceMode"].ToObject<string>();
    }
    /// <summary>Sets the deinterlace mode of an input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.6.0</summary>
    public Task<JObject> SetInputDeinterlaceModeAsync(string inputDeinterlaceMode, string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["inputDeinterlaceMode"] = JToken.FromObject(inputDeinterlaceMode);
        return _bridge.SendRequestAsync("SetInputDeinterlaceMode", data);
    }
    /// <summary>Gets the deinterlace field order of an input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.6.0</summary>
    public async Task<string> GetInputDeinterlaceFieldOrderAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        var resp = await _bridge.SendRequestAsync("GetInputDeinterlaceFieldOrder", data);
        return resp["inputDeinterlaceFieldOrder"].ToObject<string>();
    }
    /// <summary>Sets the deinterlace field order of an input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.6.0</summary>
    public Task<JObject> SetInputDeinterlaceFieldOrderAsync(string inputDeinterlaceFieldOrder, string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["inputDeinterlaceFieldOrder"] = JToken.FromObject(inputDeinterlaceFieldOrder);
        return _bridge.SendRequestAsync("SetInputDeinterlaceFieldOrder", data);
    }
    /// <summary>Gets the items of a list property from an input's properties. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetInputPropertiesListPropertyItemsAsync(string propertyName, string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["propertyName"] = JToken.FromObject(propertyName);
        return _bridge.SendRequestAsync("GetInputPropertiesListPropertyItems", data);
    }
    /// <summary>Presses a button in the properties of an input. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> PressInputPropertiesButtonAsync(string propertyName, string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["propertyName"] = JToken.FromObject(propertyName);
        return _bridge.SendRequestAsync("PressInputPropertiesButton", data);
    }
}
