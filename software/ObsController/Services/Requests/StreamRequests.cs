using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ObsController.Services.Requests;
public class StreamRequests : BaseRequests
{
    public StreamRequests(ObsBridge bridge) : base(bridge) {}
    /// <summary>Gets the status of the stream output. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetStreamStatusAsync()
    => _bridge.SendRequestAsync("GetStreamStatus", new JObject());
    /// <summary>Toggles the status of the stream output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public async Task<bool> ToggleStreamAsync()
    {
        var resp = await _bridge.SendRequestAsync("ToggleStream", new JObject());
        return resp["outputActive"].ToObject<bool>();
    }
    /// <summary>Starts the stream output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0 ---</summary>
    public Task<JObject> StartStreamAsync()
    => _bridge.SendRequestAsync("StartStream", new JObject());
    /// <summary>Stops the stream output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0 ---</summary>
    public Task<JObject> StopStreamAsync()
    => _bridge.SendRequestAsync("StopStream", new JObject());
    /// <summary>Sends CEA-608 caption text over the stream output. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="captionText">Caption text</param>
    public Task<JObject> SendStreamCaptionAsync(string captionText)
    {
        var data = new JObject();
        data["captionText"] = JToken.FromObject(captionText);
        return _bridge.SendRequestAsync("SendStreamCaption", data);
    }
}