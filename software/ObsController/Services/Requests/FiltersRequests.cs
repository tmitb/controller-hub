using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ObsController.Services.Requests;
public class FiltersRequests
{
    private readonly ObsBridge _bridge;
    public FiltersRequests(ObsBridge bridge) => _bridge = bridge;
    /// <summary>Gets an array of all available source filter kinds. Similar to `GetInputKindList` - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.4.0</summary>
    public Task<JObject> GetSourceFilterKindListAsync()
    => _bridge.SendRequestAsync("GetSourceFilterKindList", new JObject());
    /// <summary>Gets an array of all of a source's filters. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sourceName">Name of the source</param>
    /// <param name="sourceUuid">UUID of the source</param>
    public Task<JObject> GetSourceFilterListAsync(string sourceName = null, string sourceUuid = null)
    {
        var data = new JObject();
        if (sourceName != null) data["sourceName"] = JToken.FromObject(sourceName);
        if (sourceUuid != null) data["sourceUuid"] = JToken.FromObject(sourceUuid);
        return _bridge.SendRequestAsync("GetSourceFilterList", data);
    }
    /// <summary>Gets the default settings for a filter kind. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="filterKind">Filter kind to get the default settings for</param>
    public Task<JObject> GetSourceFilterDefaultSettingsAsync(string filterKind)
    {
        var data = new JObject();
        data["filterKind"] = JToken.FromObject(filterKind);
        return _bridge.SendRequestAsync("GetSourceFilterDefaultSettings", data);
    }
    /// <summary>Creates a new filter, adding it to the specified source. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sourceName">Name of the source to add the filter to</param>
    /// <param name="sourceUuid">UUID of the source to add the filter to</param>
    /// <param name="filterName">Name of the new filter to be created</param>
    /// <param name="filterKind">The kind of filter to be created</param>
    /// <param name="filterSettings">Settings object to initialize the filter with</param>
    public Task<JObject> CreateSourceFilterAsync(string sourceName = null, string sourceUuid = null, string filterName, string filterKind, JObject filterSettings = null)
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
    /// <param name="sourceName">Name of the source the filter is on</param>
    /// <param name="sourceUuid">UUID of the source the filter is on</param>
    /// <param name="filterName">Name of the filter to remove</param>
    public Task<JObject> RemoveSourceFilterAsync(string sourceName = null, string sourceUuid = null, string filterName)
    {
        var data = new JObject();
        if (sourceName != null) data["sourceName"] = JToken.FromObject(sourceName);
        if (sourceUuid != null) data["sourceUuid"] = JToken.FromObject(sourceUuid);
        data["filterName"] = JToken.FromObject(filterName);
        return _bridge.SendRequestAsync("RemoveSourceFilter", data);
    }
    /// <summary>Sets the name of a source filter (rename). - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sourceName">Name of the source the filter is on</param>
    /// <param name="sourceUuid">UUID of the source the filter is on</param>
    /// <param name="filterName">Current name of the filter</param>
    /// <param name="newFilterName">New name for the filter</param>
    public Task<JObject> SetSourceFilterNameAsync(string sourceName = null, string sourceUuid = null, string filterName, string newFilterName)
    {
        var data = new JObject();
        if (sourceName != null) data["sourceName"] = JToken.FromObject(sourceName);
        if (sourceUuid != null) data["sourceUuid"] = JToken.FromObject(sourceUuid);
        data["filterName"] = JToken.FromObject(filterName);
        data["newFilterName"] = JToken.FromObject(newFilterName);
        return _bridge.SendRequestAsync("SetSourceFilterName", data);
    }
    /// <summary>Gets the info for a specific source filter. - Complexity Rating: `2/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sourceName">Name of the source</param>
    /// <param name="sourceUuid">UUID of the source</param>
    /// <param name="filterName">Name of the filter</param>
    public Task<JObject> GetSourceFilterAsync(string sourceName = null, string sourceUuid = null, string filterName)
    {
        var data = new JObject();
        if (sourceName != null) data["sourceName"] = JToken.FromObject(sourceName);
        if (sourceUuid != null) data["sourceUuid"] = JToken.FromObject(sourceUuid);
        data["filterName"] = JToken.FromObject(filterName);
        return _bridge.SendRequestAsync("GetSourceFilter", data);
    }
    /// <summary>Sets the index position of a filter on a source. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sourceName">Name of the source the filter is on</param>
    /// <param name="sourceUuid">UUID of the source the filter is on</param>
    /// <param name="filterName">Name of the filter</param>
    /// <param name="filterIndex">New index position of the filter</param>
    public Task<JObject> SetSourceFilterIndexAsync(string sourceName = null, string sourceUuid = null, string filterName, double filterIndex)
    {
        var data = new JObject();
        if (sourceName != null) data["sourceName"] = JToken.FromObject(sourceName);
        if (sourceUuid != null) data["sourceUuid"] = JToken.FromObject(sourceUuid);
        data["filterName"] = JToken.FromObject(filterName);
        data["filterIndex"] = JToken.FromObject(filterIndex);
        return _bridge.SendRequestAsync("SetSourceFilterIndex", data);
    }
    /// <summary>Sets the settings of a source filter. - Complexity Rating: `3/5` - Latest Supported RPC Version: `1` - Added in v5.0.0</summary>
    /// <param name="sourceName">Name of the source the filter is on</param>
    /// <param name="sourceUuid">UUID of the source the filter is on</param>
    /// <param name="filterName">Name of the filter to set the settings of</param>
    /// <param name="filterSettings">Object of settings to apply</param>
    /// <param name="overlay">True == apply the settings on top of existing ones, False == reset the input to its defaults, then apply settings.</param>
    public Task<JObject> SetSourceFilterSettingsAsync(string sourceName = null, string sourceUuid = null, string filterName, JObject filterSettings, bool? overlay = null)
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
    /// <param name="sourceName">Name of the source the filter is on</param>
    /// <param name="sourceUuid">UUID of the source the filter is on</param>
    /// <param name="filterName">Name of the filter</param>
    /// <param name="filterEnabled">New enable state of the filter</param>
    public Task<JObject> SetSourceFilterEnabledAsync(string sourceName = null, string sourceUuid = null, string filterName, bool filterEnabled)
    {
        var data = new JObject();
        if (sourceName != null) data["sourceName"] = JToken.FromObject(sourceName);
        if (sourceUuid != null) data["sourceUuid"] = JToken.FromObject(sourceUuid);
        data["filterName"] = JToken.FromObject(filterName);
        data["filterEnabled"] = JToken.FromObject(filterEnabled);
        return _bridge.SendRequestAsync("SetSourceFilterEnabled", data);
    }
}