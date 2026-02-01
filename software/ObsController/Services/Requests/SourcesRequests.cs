using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ObsController.Services.Requests;
public class SourcesRequests
{
    private readonly ObsBridge _bridge;
    public SourcesRequests(ObsBridge bridge) => _bridge = bridge;
    /// <summary>Gets the active and show state of a source. **Compatible with inputs and scenes.** - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sourceName">Name of the source to get the active state of</param>
    /// <param name="sourceUuid">UUID of the source to get the active state of</param>
    public Task<JObject> GetSourceActiveAsync(string sourceName = null, string sourceUuid = null)
    {
        var data = new JObject();
        if (sourceName != null) data["sourceName"] = JToken.FromObject(sourceName);
        if (sourceUuid != null) data["sourceUuid"] = JToken.FromObject(sourceUuid);
        return _bridge.SendRequestAsync("GetSourceActive", data);
    }
    /// <summary>Gets a Base64-encoded screenshot of a source. The `imageWidth` and `imageHeight` parameters are treated as "scale to inner", meaning the smallest ratio will be used and the aspect ratio of the original resolution is kept. If `imageWidth` and `imageHeight` are not specified, the compressed image will use the full resolution of the source. **Compatible with inputs and scenes.** - Complexity Rating: `4/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sourceName">Name of the source to take a screenshot of</param>
    /// <param name="sourceUuid">UUID of the source to take a screenshot of</param>
    /// <param name="imageFormat">Image compression format to use. Use `GetVersion` to get compatible image formats</param>
    /// <param name="imageWidth">Width to scale the screenshot to</param>
    /// <param name="imageHeight">Height to scale the screenshot to</param>
    /// <param name="imageCompressionQuality">Compression quality to use. 0 for high compression, 100 for uncompressed. -1 to use "default" (whatever that means, idk)</param>
    public Task<string> GetSourceScreenshotAsync(string sourceName = null, string sourceUuid = null, string imageFormat, double? imageWidth = null, double? imageHeight = null, double? imageCompressionQuality = null)
    {
        var data = new JObject();
        if (sourceName != null) data["sourceName"] = JToken.FromObject(sourceName);
        if (sourceUuid != null) data["sourceUuid"] = JToken.FromObject(sourceUuid);
        data["imageFormat"] = JToken.FromObject(imageFormat);
        if (imageWidth != null) data["imageWidth"] = JToken.FromObject(imageWidth);
        if (imageHeight != null) data["imageHeight"] = JToken.FromObject(imageHeight);
        if (imageCompressionQuality != null) data["imageCompressionQuality"] = JToken.FromObject(imageCompressionQuality);
        return _bridge.SendRequestAsync("GetSourceScreenshot", data);
    }
    /// <summary>Saves a screenshot of a source to the filesystem. The `imageWidth` and `imageHeight` parameters are treated as "scale to inner", meaning the smallest ratio will be used and the aspect ratio of the original resolution is kept. If `imageWidth` and `imageHeight` are not specified, the compressed image will use the full resolution of the source. **Compatible with inputs and scenes.** - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sourceName">Name of the source to take a screenshot of</param>
    /// <param name="sourceUuid">UUID of the source to take a screenshot of</param>
    /// <param name="imageFormat">Image compression format to use. Use `GetVersion` to get compatible image formats</param>
    /// <param name="imageFilePath">Path to save the screenshot file to. Eg. `C:\Users\user\Desktop\screenshot.png`</param>
    /// <param name="imageWidth">Width to scale the screenshot to</param>
    /// <param name="imageHeight">Height to scale the screenshot to</param>
    /// <param name="imageCompressionQuality">Compression quality to use. 0 for high compression, 100 for uncompressed. -1 to use "default" (whatever that means, idk)</param>
    public Task<JObject> SaveSourceScreenshotAsync(string sourceName = null, string sourceUuid = null, string imageFormat, string imageFilePath, double? imageWidth = null, double? imageHeight = null, double? imageCompressionQuality = null)
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