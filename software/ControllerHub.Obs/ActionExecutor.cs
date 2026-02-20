using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using ControllerHub.PluginContract;

namespace ControllerHub.Obs;

/// <summary>
/// Executes an action defined in <see cref="ActionMapping"/>.
/// The mapping's ExtraData must contain a <c>category</c> (request class name without the trailing "Requests"),
/// an <c>action</c> (method name without the Async suffix) and an optional <c>parameters</c> object.
/// This helper resolves the appropriate request class, builds arguments from the parameter dictionary,
/// invokes the async method via reflection and awaits its completion.
/// </summary>
public static class ActionExecutor
{
    private static string? GetStr(Dictionary<string, JsonElement>? d, string key)
        => d != null && d.TryGetValue(key, out var e)
           ? (e.ValueKind == JsonValueKind.String ? e.GetString() : e.GetRawText())
           : null;

    /// <summary>
    /// Executes the supplied mapping using the provided <paramref name="bridge"/> to create request objects.
    /// </summary>
    public static async Task ExecuteAsync(ActionMapping entry, ObsBridge bridge)
    {
        if (entry == null) throw new ArgumentNullException(nameof(entry));

        var category = GetStr(entry.ExtraData, "category");
        var action   = GetStr(entry.ExtraData, "action");

        if (string.IsNullOrWhiteSpace(category))
            throw new InvalidOperationException("Request class must be defined.");

        // Build the full type name: e.g., "ScenesRequests" under the Requests namespace.
        var requestClassName = $"{category}Requests";
        var fullName = $"ControllerHub.Obs.Requests.{requestClassName}";
        var targetType = Type.GetType(fullName);
        if (targetType == null)
            throw new InvalidOperationException($"Request class '{fullName}' not found.");

        // All request classes have a constructor that accepts ObsBridge.
        var targetInstance = Activator.CreateInstance(targetType, bridge);

        // Resolve the method – we expect an async method ending with Async.
        var methodName = action + "Async";
        var method = targetType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
        if (method == null)
            throw new InvalidOperationException($"Method '{methodName}' not found on type {targetType.Name}.");

        // Build parameters dict from ExtraData["parameters"] (JsonElement Object).
        Dictionary<string, string>? parameters = null;
        if (entry.ExtraData != null &&
            entry.ExtraData.TryGetValue("parameters", out var paramsEl) &&
            paramsEl.ValueKind == JsonValueKind.Object)
        {
            parameters = [];
            foreach (var prop in paramsEl.EnumerateObject())
                parameters[prop.Name] = prop.Value.ValueKind == JsonValueKind.String
                    ? prop.Value.GetString()!
                    : prop.Value.GetRawText();
        }

        // Build argument list matching the method signature.
        var parametersInfo = method.GetParameters();
        var args = new object[parametersInfo.Length];
        for (int i = 0; i < parametersInfo.Length; i++)
        {
            var pInfo = parametersInfo[i];
            // Look up a supplied value by parameter name (case-insensitive).
            string? rawValue = null;
            if (parameters != null)
                parameters.TryGetValue(pInfo.Name!, out rawValue);

            if (rawValue == null)
            {
                // No value supplied – use default for optional parameters or null otherwise.
                args[i] = pInfo.HasDefaultValue ? pInfo.DefaultValue! : GetDefault(pInfo.ParameterType)!;
                continue;
            }

            // Convert the string to the required parameter type, handling nullable types.
            var targetParamType = Nullable.GetUnderlyingType(pInfo.ParameterType) ?? pInfo.ParameterType;
            try
            {
                args[i] = Convert.ChangeType(rawValue, targetParamType);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to convert parameter '{pInfo.Name}' value '{rawValue}' to type {targetParamType}.", ex);
            }
        }

        // Invoke the async method.
        var result = method.Invoke(targetInstance, args);
        if (result is Task task)
        {
            await task.ConfigureAwait(false);
        }
        else
        {
            throw new InvalidOperationException($"Method '{methodName}' did not return a Task.");
        }
    }

    private static object? GetDefault(Type t) => t.IsValueType ? Activator.CreateInstance(t) : null;
}
