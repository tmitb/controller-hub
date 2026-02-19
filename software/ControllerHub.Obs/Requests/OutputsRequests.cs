using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ControllerHub.Obs.Requests;
public class OutputsRequests : BaseRequests
{
    public OutputsRequests(ObsBridge bridge) : base(bridge) {}
    /// <summary>Gets the status of the virtualcam output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public async Task<bool> GetVirtualCamStatusAsync()
    {
        var resp = await _bridge.SendRequestAsync("GetVirtualCamStatus", new JObject());
        return resp["outputActive"].ToObject<bool>();
    }
    /// <summary>Toggles the state of the virtualcam output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public async Task<bool> ToggleVirtualCamAsync()
    {
        var resp = await _bridge.SendRequestAsync("ToggleVirtualCam", new JObject());
        return resp["outputActive"].ToObject<bool>();
    }
    /// <summary>Starts the virtualcam output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0 ---</summary>
    public Task<JObject> StartVirtualCamAsync()
    => _bridge.SendRequestAsync("StartVirtualCam", new JObject());
    /// <summary>Stops the virtualcam output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0 ---</summary>
    public Task<JObject> StopVirtualCamAsync()
    => _bridge.SendRequestAsync("StopVirtualCam", new JObject());
    /// <summary>Gets the status of the replay buffer output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public async Task<bool> GetReplayBufferStatusAsync()
    {
        var resp = await _bridge.SendRequestAsync("GetReplayBufferStatus", new JObject());
        return resp["outputActive"].ToObject<bool>();
    }
    /// <summary>Toggles the state of the replay buffer output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public async Task<bool> ToggleReplayBufferAsync()
    {
        var resp = await _bridge.SendRequestAsync("ToggleReplayBuffer", new JObject());
        return resp["outputActive"].ToObject<bool>();
    }
    /// <summary>Starts the replay buffer output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0 ---</summary>
    public Task<JObject> StartReplayBufferAsync()
    => _bridge.SendRequestAsync("StartReplayBuffer", new JObject());
    /// <summary>Stops the replay buffer output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0 ---</summary>
    public Task<JObject> StopReplayBufferAsync()
    => _bridge.SendRequestAsync("StopReplayBuffer", new JObject());
    /// <summary>Saves the contents of the replay buffer output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0 ---</summary>
    public Task<JObject> SaveReplayBufferAsync()
    => _bridge.SendRequestAsync("SaveReplayBuffer", new JObject());
    /// <summary>Gets the filename of the last replay buffer save file. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public async Task<string> GetLastReplayBufferReplayAsync()
    {
        var resp = await _bridge.SendRequestAsync("GetLastReplayBufferReplay", new JObject());
        return resp["savedReplayPath"].ToObject<string>();
    }
    /// <summary>Gets the list of available outputs. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetOutputListAsync()
    => _bridge.SendRequestAsync("GetOutputList", new JObject());
    /// <summary>Gets the status of an output. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetOutputStatusAsync(string outputName)
    {
        var data = new JObject();
        data["outputName"] = JToken.FromObject(outputName);
        return _bridge.SendRequestAsync("GetOutputStatus", data);
    }
    /// <summary>Toggles the status of an output. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public async Task<bool> ToggleOutputAsync(string outputName)
    {
        var data = new JObject();
        data["outputName"] = JToken.FromObject(outputName);
        var resp = await _bridge.SendRequestAsync("ToggleOutput", data);
        return resp["outputActive"].ToObject<bool>();
    }
    /// <summary>Starts an output. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> StartOutputAsync(string outputName)
    {
        var data = new JObject();
        data["outputName"] = JToken.FromObject(outputName);
        return _bridge.SendRequestAsync("StartOutput", data);
    }
    /// <summary>Stops an output. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> StopOutputAsync(string outputName)
    {
        var data = new JObject();
        data["outputName"] = JToken.FromObject(outputName);
        return _bridge.SendRequestAsync("StopOutput", data);
    }
    /// <summary>Gets the settings of an output. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetOutputSettingsAsync(string outputName)
    {
        var data = new JObject();
        data["outputName"] = JToken.FromObject(outputName);
        return _bridge.SendRequestAsync("GetOutputSettings", data);
    }
    /// <summary>Sets the settings of an output. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> SetOutputSettingsAsync(string outputName, JObject outputSettings)
    {
        var data = new JObject();
        data["outputName"] = JToken.FromObject(outputName);
        data["outputSettings"] = JToken.FromObject(outputSettings);
        return _bridge.SendRequestAsync("SetOutputSettings", data);
    }
}
