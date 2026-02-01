using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ObsController.Services.Requests;
public class OutputsRequests : BaseRequests
{
    public OutputsRequests(ObsBridge bridge) : base(bridge) {}
    /// <summary>Gets the status of the virtualcam output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<bool> GetVirtualCamStatusAsync()
    => _bridge.SendRequestAsync("GetVirtualCamStatus", new JObject());
    /// <summary>Toggles the state of the virtualcam output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<bool> ToggleVirtualCamAsync()
    => _bridge.SendRequestAsync("ToggleVirtualCam", new JObject());
    /// <summary>Starts the virtualcam output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0 ---</summary>
    public Task<JObject> StartVirtualCamAsync()
    => _bridge.SendRequestAsync("StartVirtualCam", new JObject());
    /// <summary>Stops the virtualcam output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0 ---</summary>
    public Task<JObject> StopVirtualCamAsync()
    => _bridge.SendRequestAsync("StopVirtualCam", new JObject());
    /// <summary>Gets the status of the replay buffer output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<bool> GetReplayBufferStatusAsync()
    => _bridge.SendRequestAsync("GetReplayBufferStatus", new JObject());
    /// <summary>Toggles the state of the replay buffer output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<bool> ToggleReplayBufferAsync()
    => _bridge.SendRequestAsync("ToggleReplayBuffer", new JObject());
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
    public Task<string> GetLastReplayBufferReplayAsync()
    => _bridge.SendRequestAsync("GetLastReplayBufferReplay", new JObject());
    /// <summary>Gets the list of available outputs. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetOutputListAsync()
    => _bridge.SendRequestAsync("GetOutputList", new JObject());
    /// <summary>Gets the status of an output. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="outputName">Output name</param>
    public Task<JObject> GetOutputStatusAsync(string outputName)
    {
        var data = new JObject();
        data["outputName"] = JToken.FromObject(outputName);
        return _bridge.SendRequestAsync("GetOutputStatus", data);
    }
    /// <summary>Toggles the status of an output. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="outputName">Output name</param>
    public Task<bool> ToggleOutputAsync(string outputName)
    {
        var data = new JObject();
        data["outputName"] = JToken.FromObject(outputName);
        return _bridge.SendRequestAsync("ToggleOutput", data);
    }
    /// <summary>Starts an output. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="outputName">Output name</param>
    public Task<JObject> StartOutputAsync(string outputName)
    {
        var data = new JObject();
        data["outputName"] = JToken.FromObject(outputName);
        return _bridge.SendRequestAsync("StartOutput", data);
    }
    /// <summary>Stops an output. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="outputName">Output name</param>
    public Task<JObject> StopOutputAsync(string outputName)
    {
        var data = new JObject();
        data["outputName"] = JToken.FromObject(outputName);
        return _bridge.SendRequestAsync("StopOutput", data);
    }
    /// <summary>Gets the settings of an output. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="outputName">Output name</param>
    public Task<JObject> GetOutputSettingsAsync(string outputName)
    {
        var data = new JObject();
        data["outputName"] = JToken.FromObject(outputName);
        return _bridge.SendRequestAsync("GetOutputSettings", data);
    }
    /// <summary>Sets the settings of an output. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="outputName">Output name</param>
    /// <param name="outputSettings">Output settings</param>
    public Task<JObject> SetOutputSettingsAsync(string outputName, JObject outputSettings)
    {
        var data = new JObject();
        data["outputName"] = JToken.FromObject(outputName);
        data["outputSettings"] = JToken.FromObject(outputSettings);
        return _bridge.SendRequestAsync("SetOutputSettings", data);
    }
}