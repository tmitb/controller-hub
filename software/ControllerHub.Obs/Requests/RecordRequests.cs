using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ControllerHub.Obs.Requests;
public class RecordRequests : BaseRequests
{
    public RecordRequests(ObsBridge bridge) : base(bridge) {}
    /// <summary>Gets the status of the record output. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetRecordStatusAsync()
    => _bridge.SendRequestAsync("GetRecordStatus", new JObject());
    /// <summary>Toggles the status of the record output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public async Task<bool> ToggleRecordAsync()
    {
        var resp = await _bridge.SendRequestAsync("ToggleRecord", new JObject());
        return resp["outputActive"].ToObject<bool>();
    }
    /// <summary>Starts the record output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0 ---</summary>
    public Task<JObject> StartRecordAsync()
    => _bridge.SendRequestAsync("StartRecord", new JObject());
    /// <summary>Stops the record output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public async Task<string> StopRecordAsync()
    {
        var resp = await _bridge.SendRequestAsync("StopRecord", new JObject());
        return resp["outputPath"].ToObject<string>();
    }
    /// <summary>Toggles pause on the record output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0 ---</summary>
    public Task<JObject> ToggleRecordPauseAsync()
    => _bridge.SendRequestAsync("ToggleRecordPause", new JObject());
    /// <summary>Pauses the record output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0 ---</summary>
    public Task<JObject> PauseRecordAsync()
    => _bridge.SendRequestAsync("PauseRecord", new JObject());
    /// <summary>Resumes the record output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0 ---</summary>
    public Task<JObject> ResumeRecordAsync()
    => _bridge.SendRequestAsync("ResumeRecord", new JObject());
    /// <summary>Splits the current file being recorded into a new file. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.5.0 ---</summary>
    public Task<JObject> SplitRecordFileAsync()
    => _bridge.SendRequestAsync("SplitRecordFile", new JObject());
    /// <summary>Adds a new chapter marker to the file currently being recorded. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.5.0</summary>
    public Task<JObject> CreateRecordChapterAsync(string chapterName = null)
    {
        var data = new JObject();
        if (chapterName != null) data["chapterName"] = JToken.FromObject(chapterName);
        return _bridge.SendRequestAsync("CreateRecordChapter", data);
    }
}
