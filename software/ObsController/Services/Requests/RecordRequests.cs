using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ObsController.Services.Requests;
public class RecordRequests : BaseRequests
{
    public RecordRequests(ObsBridge bridge) : base(bridge) {}
    /// <summary>Gets the status of the record output. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetRecordStatusAsync()
    => _bridge.SendRequestAsync("GetRecordStatus", new JObject());
    /// <summary>Toggles the status of the record output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<bool> ToggleRecordAsync()
    => _bridge.SendRequestAsync("ToggleRecord", new JObject());
    /// <summary>Starts the record output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0 ---</summary>
    public Task<JObject> StartRecordAsync()
    => _bridge.SendRequestAsync("StartRecord", new JObject());
    /// <summary>Stops the record output. - Complexity Rating: `1/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<string> StopRecordAsync()
    => _bridge.SendRequestAsync("StopRecord", new JObject());
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
    /// <summary>Adds a new chapter marker to the file currently being recorded. Note: As of OBS 30.2.0, the only file format supporting this feature is Hybrid MP4. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.5.0</summary>
    /// <param name="chapterName">Name of the new chapter</param>
    public Task<JObject> CreateRecordChapterAsync(string chapterName = null)
    {
        var data = new JObject();
        if (chapterName != null) data["chapterName"] = JToken.FromObject(chapterName);
        return _bridge.SendRequestAsync("CreateRecordChapter", data);
    }
}