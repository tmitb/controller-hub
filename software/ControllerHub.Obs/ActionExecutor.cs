using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ControllerHub.PluginContract;

namespace ControllerHub.Obs;

/// <summary>
/// Executes an action defined in <see cref="ActionMapping"/>.
/// The mapping specifies a <c>Category</c> (request class name without the trailing "Requests"),
/// an <c>Action</c> (method name without the Async suffix) and optional key/value <c>Parameters</c>.
/// This helper resolves the appropriate request class, builds arguments from the parameter dictionary,
/// invokes the async method via reflection and awaits its completion.
/// </summary>
public static class ActionExecutor
{
    /// <summary>
    /// Executes the supplied mapping using the provided <paramref name="bridge"/> to create request objects.
    /// </summary>
    public static async Task ExecuteAsync(ActionMapping entry, ObsBridge bridge)
    {
        if (entry == null) throw new ArgumentNullException(nameof(entry));

        // Determine the target type: either a specific Requests class or fallback to ObsBridge.
        Type targetType;
        object targetInstance;

        if (string.IsNullOrWhiteSpace(entry.Category))
        {
            throw new InvalidOperationException($"Request class must be defined.");
        }
        else
        {
            // Build the full type name: e.g., "ScenesRequests" under the Requests namespace.
            var requestClassName = $"{entry.Category}Requests"; // assume caller provides correct case
            var fullName = $"ControllerHub.Obs.Requests.{requestClassName}";
            targetType = Type.GetType(fullName);
            if (targetType == null)
                throw new InvalidOperationException($"Request class '{fullName}' not found.");

            // All request classes have a constructor that accepts ObsBridge.
            targetInstance = Activator.CreateInstance(targetType, bridge);
        }

        // Resolve the method – we expect an async method ending with Async.
        var methodName = entry.Action + "Async";
        var method = targetType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
        if (method == null)
            throw new InvalidOperationException($"Method '{methodName}' not found on type {targetType.Name}.");

        // Build argument list matching the method signature.
        var parametersInfo = method.GetParameters();
        var args = new object[parametersInfo.Length];
        for (int i = 0; i < parametersInfo.Length; i++)
        {
            var pInfo = parametersInfo[i];
            // Look up a supplied value by parameter name (case‑insensitive).
            string rawValue = null;
            if (entry.Parameters != null)
                entry.Parameters.TryGetValue(pInfo.Name, out rawValue);

            if (rawValue == null)
            {
                // No value supplied – use default for optional parameters or null otherwise.
                args[i] = pInfo.HasDefaultValue ? pInfo.DefaultValue : GetDefault(pInfo.ParameterType);
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

    private static object GetDefault(Type t) => t.IsValueType ? Activator.CreateInstance(t) : null;
}
