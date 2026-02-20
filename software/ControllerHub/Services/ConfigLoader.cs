using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using ControllerHub.Models;

namespace ControllerHub.Services;

public static class ConfigLoader
{
    private const string DefaultMappingFile = "mapping.json";

    public static Mapping Load(string customPath = null)
    {
        var path = customPath ?? GetDefaultMapping();
        if (!File.Exists(path))
            throw new FileNotFoundException($"Mapping file not found: {path}");

        var json = File.ReadAllText(path);
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
            };
            return JsonSerializer.Deserialize<Mapping>(json, options) ?? new Mapping();
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Failed to parse mapping file '{path}'.", ex);
        }
    }

    private static string GetDefaultMapping()
    {
        string codeBase = Assembly.GetExecutingAssembly().Location;
        return Path.Combine(Path.GetDirectoryName(codeBase)!, DefaultMappingFile);
    }
}
