using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ControllerHub.Obs.Requests;
public class FiltersRequests : BaseRequests
{
    public FiltersRequests(ObsBridge bridge) : base(bridge) {}
    /// <summary>Gets an array of all available source filter kinds. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.4.0</summary>
    public Task<JObject> GetSourceFilterKindListAsync()
    => _bridge.SendRequestAsync("GetSourceFilterKindList", new JObject());
    /// <summary>Gets an array of all of a source's filters. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetSourceFilterListAsync(string sourceName = null, string sourceUuid = null)
    {
        var data = new JObject();
        if (sourceName != null) data["sourceName"] = JToken.FromObject(sourceName);
        if (sourceUuid != null) data["sourceUuid"] = JToken.FromObject(sourceUuid);
        return _bridge.SendRequestAsync("GetSourceFilterList", data);
    }
    /// <summary>Gets the default settings for a filter kind. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetSourceFilterDefaultSettingsAsync(string filterKind)
    {
        var data = new JObject();
        data["filterKind"] = JToken.FromObject(filterKind);
        return _bridge.SendRequestAsync("GetSourceFilterDefaultSettings", data);
    }
    /// <summary>Creates a new filter, adding it to the specified source. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> CreateSourceFilterAsync(string filterName, string filterKind, string sourceName = null, string sourceUuid = null, JObject filterSettings = null)
    {
        var data = new JObject();
        if (sourceName != null) data["sourceName"] = JToken.FromObject(sourceName);
        if (sourceUuid != null) data["sourceUuid"] = JToken.FromObject(sourceUuid);
        data["filterName"] = JToken.FromObject(filterName);
        data["filterKind"] = JToken.FromObject(filterKind);
        if (filterSettings != null) data["filterSettings"] = JToken.FromObject(filterSettings);
        return _bridge.SendRequestAsync("CreateSourceFilter", data);
    }
    /// <summary>Removes a filter from a source. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> RemoveSourceFilterAsync(string filterName, string sourceName = null, string sourceUuid = null)
    {
        var data = new JObject();
        if (sourceName != null) data["sourceName"] = JToken.FromObject(sourceName);
        if (sourceUuid != null) data["sourceUuid"] = JToken.FromObject(sourceUuid);
        data["filterName"] = JToken.FromObject(filterName);
        return _bridge.SendRequestAsync("RemoveSourceFilter", data);
    }
    /// <summary>Sets the name of a source filter (rename). - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> SetSourceFilterNameAsync(string filterName, string newFilterName, string sourceName = null, string sourceUuid = null)
    {
        var data = new JObject();
        if (sourceName != null) data["sourceName"] = JToken.FromObject(sourceName);
        if (sourceUuid != null) data["sourceUuid"] = JToken.FromObject(sourceUuid);
        data["filterName"] = JToken.FromObject(filterName);
        data["newFilterName"] = JToken.FromObject(newFilterName);
        return _bridge.SendRequestAsync("SetSourceFilterName", data);
    }
    /// <summary>Gets the info for a specific source filter. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> GetSourceFilterAsync(string filterName, string sourceName = null, string sourceUuid = null)
    {
        var data = new JObject();
        if (sourceName != null) data["sourceName"] = JToken.FromObject(sourceName);
        if (sourceUuid != null) data["sourceUuid"] = JToken.FromObject(sourceUuid);
        data["filterName"] = JToken.FromObject(filterName);
        return _bridge.SendRequestAsync("GetSourceFilter", data);
    }
    /// <summary>Sets the index position of a filter on a source. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> SetSourceFilterIndexAsync(string filterName, double filterIndex, string sourceName = null, string sourceUuid = null)
    {
        var data = new JObject();
        if (sourceName != null) data["sourceName"] = JToken.FromObject(sourceName);
        if (sourceUuid != null) data["sourceUuid"] = JToken.FromObject(sourceUuid);
        data["filterName"] = JToken.FromObject(filterName);
        data["filterIndex"] = JToken.FromObject(filterIndex);
        return _bridge.SendRequestAsync("SetSourceFilterIndex", data);
    }
    /// <summary>Sets the settings of a source filter. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> SetSourceFilterSettingsAsync(string filterName, JObject filterSettings, string sourceName = null, string sourceUuid = null, bool? overlay = null)
    {
        var data = new JObject();
        if (sourceName != null) data["sourceName"] = JToken.FromObject(sourceName);
        if (sourceUuid != null) data["sourceUuid"] = JToken.FromObject(sourceUuid);
        data["filterName"] = JToken.FromObject(filterName);
        data["filterSettings"] = JToken.FromObject(filterSettings);
        if (overlay != null) data["overlay"] = JToken.FromObject(overlay);
        return _bridge.SendRequestAsync("SetSourceFilterSettings", data);
    }
    /// <summary>Sets the enable state of a source filter. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    public Task<JObject> SetSourceFilterEnabledAsync(string filterName, bool filterEnabled, string sourceName = null, string sourceUuid = null)
    {
        var data = new JObject();
        if (sourceName != null) data["sourceName"] = JToken.FromObject(sourceName);
        if (sourceUuid != null) data["sourceUuid"] = JToken.FromObject(sourceUuid);
        data["filterName"] = JToken.FromObject(filterName);
        data["filterEnabled"] = JToken.FromObject(filterEnabled);
        return _bridge.SendRequestAsync("SetSourceFilterEnabled", data);
    }
}
