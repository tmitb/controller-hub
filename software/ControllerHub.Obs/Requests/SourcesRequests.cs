using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ControllerHub.Obs.Requests;
public class SourcesRequests : BaseRequests
{
    public SourcesRequests(ObsBridge bridge) : base(bridge) {}
    /// <summary>Gets the active and show state of a source. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetSourceActiveAsync(string sourceName = null, string sourceUuid = null)
    {
        var data = new JObject();
        if (sourceName != null) data["sourceName"] = JToken.FromObject(sourceName);
        if (sourceUuid != null) data["sourceUuid"] = JToken.FromObject(sourceUuid);
        return _bridge.SendRequestAsync("GetSourceActive", data);
    }
    /// <summary>Gets a Base64-encoded screenshot of a source. - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public async Task<string> GetSourceScreenshotAsync(string imageFormat, string sourceName = null, string sourceUuid = null, double? imageWidth = null, double? imageHeight = null, double? imageCompressionQuality = null)
    {
        var data = new JObject();
        if (sourceName != null) data["sourceName"] = JToken.FromObject(sourceName);
        if (sourceUuid != null) data["sourceUuid"] = JToken.FromObject(sourceUuid);
        data["imageFormat"] = JToken.FromObject(imageFormat);
        if (imageWidth != null) data["imageWidth"] = JToken.FromObject(imageWidth);
        if (imageHeight != null) data["imageHeight"] = JToken.FromObject(imageHeight);
        if (imageCompressionQuality != null) data["imageCompressionQuality"] = JToken.FromObject(imageCompressionQuality);
        var resp = await _bridge.SendRequestAsync("GetSourceScreenshot", data);
        return resp["imageData"].ToObject<string>();
    }
    /// <summary>Saves a screenshot of a source to the filesystem. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> SaveSourceScreenshotAsync(string imageFormat, string imageFilePath, string sourceName = null, string sourceUuid = null, double? imageWidth = null, double? imageHeight = null, double? imageCompressionQuality = null)
    {
        var data = new JObject();
        if (sourceName != null) data["sourceName"] = JToken.FromObject(sourceName);
        if (sourceUuid != null) data["sourceUuid"] = JToken.FromObject(sourceUuid);
        data["imageFormat"] = JToken.FromObject(imageFormat);
        data["imageFilePath"] = JToken.FromObject(imageFilePath);
        if (imageWidth != null) data["imageWidth"] = JToken.FromObject(imageWidth);
        if (imageHeight != null) data["imageHeight"] = JToken.FromObject(imageHeight);
        if (imageCompressionQuality != null) data["imageCompressionQuality"] = JToken.FromObject(imageCompressionQuality);
        return _bridge.SendRequestAsync("SaveSourceScreenshot", data);
    }
}
