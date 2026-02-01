using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ObsController.Services.Requests;
public class MediaInputsRequests : BaseRequests
{
    public MediaInputsRequests(ObsBridge bridge) : base(bridge) {}
    /// <summary>Gets the status of a media input. Media States: - `OBS_MEDIA_STATE_NONE` - `OBS_MEDIA_STATE_PLAYING` - `OBS_MEDIA_STATE_OPENING` - `OBS_MEDIA_STATE_BUFFERING` - `OBS_MEDIA_STATE_PAUSED` - `OBS_MEDIA_STATE_STOPPED` - `OBS_MEDIA_STATE_ENDED` - `OBS_MEDIA_STATE_ERROR` - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of the media input</param>
    /// <param name="inputUuid">UUID of the media input</param>
    public Task<JObject> GetMediaInputStatusAsync(string inputName = null, string inputUuid = null)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        return _bridge.SendRequestAsync("GetMediaInputStatus", data);
    }
    /// <summary>Sets the cursor position of a media input. This request does not perform bounds checking of the cursor position. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of the media input</param>
    /// <param name="inputUuid">UUID of the media input</param>
    /// <param name="mediaCursor">New cursor position to set</param>
    public Task<JObject> SetMediaInputCursorAsync(string inputName = null, string inputUuid = null, double mediaCursor)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["mediaCursor"] = JToken.FromObject(mediaCursor);
        return _bridge.SendRequestAsync("SetMediaInputCursor", data);
    }
    /// <summary>Offsets the current cursor position of a media input by the specified value. This request does not perform bounds checking of the cursor position. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of the media input</param>
    /// <param name="inputUuid">UUID of the media input</param>
    /// <param name="mediaCursorOffset">Value to offset the current cursor position by</param>
    public Task<JObject> OffsetMediaInputCursorAsync(string inputName = null, string inputUuid = null, double mediaCursorOffset)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["mediaCursorOffset"] = JToken.FromObject(mediaCursorOffset);
        return _bridge.SendRequestAsync("OffsetMediaInputCursor", data);
    }
    /// <summary>Triggers an action on a media input. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="inputName">Name of the media input</param>
    /// <param name="inputUuid">UUID of the media input</param>
    /// <param name="mediaAction">Identifier of the `ObsMediaInputAction` enum</param>
    public Task<JObject> TriggerMediaInputActionAsync(string inputName = null, string inputUuid = null, string mediaAction)
    {
        var data = new JObject();
        if (inputName != null) data["inputName"] = JToken.FromObject(inputName);
        if (inputUuid != null) data["inputUuid"] = JToken.FromObject(inputUuid);
        data["mediaAction"] = JToken.FromObject(mediaAction);
        return _bridge.SendRequestAsync("TriggerMediaInputAction", data);
    }
}
