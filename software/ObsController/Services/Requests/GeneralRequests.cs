using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ObsController.Services.Requests;
public class GeneralRequests
{
    private readonly ObsBridge _bridge;
    public GeneralRequests(ObsBridge bridge) => _bridge = bridge;
    /// <summary>Gets data about the current plugin and RPC version. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetVersionAsync()
    => _bridge.SendRequestAsync("GetVersion", new JObject());
    /// <summary>Gets statistics about OBS, obs-websocket, and the current session. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetStatsAsync()
    => _bridge.SendRequestAsync("GetStats", new JObject());
    /// <summary>Broadcasts a `CustomEvent` to all WebSocket clients. Receivers are clients which are identified and subscribed. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="eventData">Data payload to emit to all receivers</param>
    public Task<JObject> BroadcastCustomEventAsync(JObject eventData)
    {
        var data = new JObject();
        data["eventData"] = JToken.FromObject(eventData);
        return _bridge.SendRequestAsync("BroadcastCustomEvent", data);
    }
    /// <summary>Call a request registered to a vendor. A vendor is a unique name registered by a third-party plugin or script, which allows for custom requests and events to be added to obs-websocket. If a plugin or script implements vendor requests or events, documentation is expected to be provided with them. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="vendorName">Name of the vendor to use</param>
    /// <param name="requestType">The request type to call</param>
    /// <param name="requestData">Object containing appropriate request data</param>
    public Task<JObject> CallVendorRequestAsync(string vendorName, string requestType, JObject requestData = null)
    {
        var data = new JObject();
        data["vendorName"] = JToken.FromObject(vendorName);
        data["requestType"] = JToken.FromObject(requestType);
        if (requestData != null) data["requestData"] = JToken.FromObject(requestData);
        return _bridge.SendRequestAsync("CallVendorRequest", data);
    }
    /// <summary>Gets an array of all hotkey names in OBS. Note: Hotkey functionality in obs-websocket comes as-is, and we do not guarantee support if things are broken. In 9/10 usages of hotkey requests, there exists a better, more reliable method via other requests. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetHotkeyListAsync()
    => _bridge.SendRequestAsync("GetHotkeyList", new JObject());
    /// <summary>Triggers a hotkey using its name. See `GetHotkeyList`. Note: Hotkey functionality in obs-websocket comes as-is, and we do not guarantee support if things are broken. In 9/10 usages of hotkey requests, there exists a better, more reliable method via other requests. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="hotkeyName">Name of the hotkey to trigger</param>
    /// <param name="contextName">Name of context of the hotkey to trigger</param>
    public Task<JObject> TriggerHotkeyByNameAsync(string hotkeyName, string contextName = null)
    {
        var data = new JObject();
        data["hotkeyName"] = JToken.FromObject(hotkeyName);
        if (contextName != null) data["contextName"] = JToken.FromObject(contextName);
        return _bridge.SendRequestAsync("TriggerHotkeyByName", data);
    }
    /// <summary>Triggers a hotkey using a sequence of keys. Note: Hotkey functionality in obs-websocket comes as-is, and we do not guarantee support if things are broken. In 9/10 usages of hotkey requests, there exists a better, more reliable method via other requests. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="keyId">The OBS key ID to use. See https://github.com/obsproject/obs-studio/blob/master/libobs/obs-hotkeys.h</param>
    /// <param name="keyModifiers">Object containing key modifiers to apply</param>
    public Task<JObject> TriggerHotkeyByKeySequenceAsync(string keyId = null, JObject keyModifiers = null)
    {
        var data = new JObject();
        if (keyId != null) data["keyId"] = JToken.FromObject(keyId);
        if (keyModifiers != null) data["keyModifiers"] = JToken.FromObject(keyModifiers);
        if (keyModifiers.shift != null) data["keyModifiers.shift"] = JToken.FromObject(keyModifiers.shift);
        if (keyModifiers.control != null) data["keyModifiers.control"] = JToken.FromObject(keyModifiers.control);
        if (keyModifiers.alt != null) data["keyModifiers.alt"] = JToken.FromObject(keyModifiers.alt);
        if (keyModifiers.command != null) data["keyModifiers.command"] = JToken.FromObject(keyModifiers.command);
        return _bridge.SendRequestAsync("TriggerHotkeyByKeySequence", data);
    }
    /// <summary>Sleeps for a time duration or number of frames. Only available in request batches with types `SERIAL_REALTIME` or `SERIAL_FRAME`. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sleepMillis">Number of milliseconds to sleep for (if `SERIAL_REALTIME` mode)</param>
    /// <param name="sleepFrames">Number of frames to sleep for (if `SERIAL_FRAME` mode)</param>
    public Task<JObject> SleepAsync(double? sleepMillis = null, double? sleepFrames = null)
    {
        var data = new JObject();
        if (sleepMillis != null) data["sleepMillis"] = JToken.FromObject(sleepMillis);
        if (sleepFrames != null) data["sleepFrames"] = JToken.FromObject(sleepFrames);
        return _bridge.SendRequestAsync("Sleep", data);
    }
}