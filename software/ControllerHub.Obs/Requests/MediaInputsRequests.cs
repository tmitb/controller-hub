using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ControllerHub.Obs.Requests;
public class MediaInputsRequests : BaseRequests
{
    public MediaInputsRequests(ObsBridge bridge) : base(bridge) {}
    /// <summary>Gets the status of a media input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetMediaInputStatusAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        return _bridge.SendRequestAsync("GetMediaInputStatus", data);
    }
    /// <summary>Sets the cursor position of a media input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> SetMediaInputCursorAsync(double mediaCursor, string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["mediaCursor"] = JToken.FromObject(mediaCursor);
        return _bridge.SendRequestAsync("SetMediaInputCursor", data);
    }
    /// <summary>Offsets the current cursor position of a media input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> OffsetMediaInputCursorAsync(double mediaCursorOffset, string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["mediaCursorOffset"] = JToken.FromObject(mediaCursorOffset);
        return _bridge.SendRequestAsync("OffsetMediaInputCursor", data);
    }
    /// <summary>Triggers an action on a media input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> TriggerMediaInputActionAsync(string mediaAction, string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["mediaAction"] = JToken.FromObject(mediaAction);
        return _bridge.SendRequestAsync("TriggerMediaInputAction", data);
    }
}
