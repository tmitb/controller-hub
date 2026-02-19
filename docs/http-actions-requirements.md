# Requirements: Plugin Architecture & HTTP Actions

**Status:** Draft
**Date:** 2026-02-18

---

## 1. Motivation

The controller currently supports only OBS WebSocket actions. Users want to trigger other systems (HTTP APIs, home automation, webhooks, etc.) from the same physical device. To support this without hard-coding every integration, action types are modelled as **runtime plugins**: self-contained DLLs that the host loads at startup. HTTP support is the first non-OBS plugin built on top of this foundation.

---

## 2. Goals

- Introduce a plugin system so that any action type can be added by dropping a DLL next to the exe — no recompilation of the host required.
- Refactor the existing OBS integration into the plugin model alongside the new HTTP plugin.
- Allow any button/switch/axis mapping to call an HTTP endpoint fully independently of OBS.
- Keep configuration simple and backward-compatible.

## 3. Non-Goals (out of scope for this effort)

- Dynamic value injection / templating in URL or body (e.g. `{{value}}` from axis position).
- Named credential store or auth beyond raw headers.
- Retry logic on HTTP failure.
- Shell/process execution, MQTT, WebSocket, or any action type beyond OBS and HTTP.
- Parsing or acting on HTTP response bodies.
- A plugin package manager or versioning/compatibility system.

---

## 4. Plugin System  *(predecessor — must be completed before HTTP plugin)*

### 4.1 Concept

An **action plugin** is a .NET class library (`.dll`) that implements `IActionPlugin`. The host (`ObsController.exe`) scans a `plugins/` folder at startup, loads every DLL it finds, locates all `IActionPlugin` implementations, and builds a runtime registry keyed by `TypeName`. When a button is pressed, the host looks up the plugin for that action's `type` value and delegates execution to it.

### 4.2 `IActionPlugin` Interface

The interface lives in a shared library (`ObsController.PluginContract`) that both the host and every plugin reference. It must **not** be defined inside `ObsController.exe` itself, so plugins can be compiled independently.

```csharp
namespace ObsController.PluginContract;

public interface IActionPlugin : IAsyncDisposable
{
    /// <summary>
    /// The type name this plugin handles (matched against the "type" field
    /// in mapping.json). Must be unique across all loaded plugins.
    /// Example: "obs", "http".
    /// </summary>
    string TypeName { get; }

    /// <summary>
    /// Called once at host startup. Receives the raw plugin config block
    /// from mapping.json (null if no block exists for this plugin), and a
    /// logger provided by the host for uniform console output formatting.
    /// The plugin should establish any persistent connections here.
    /// </summary>
    Task InitializeAsync(JsonElement? pluginConfig, IPluginLogger logger, CancellationToken ct);

    /// <summary>
    /// Executes a single action. Called each time a mapped button is pressed.
    /// Must never throw; errors must be caught and logged internally.
    /// </summary>
    Task ExecuteAsync(ActionMapping action, CancellationToken ct);
}
```

`JsonElement?` is used for `pluginConfig` so the contract has no dependency on any concrete config model — each plugin interprets its own block.

The host supplies an `IPluginLogger` implementation so that all console output is formatted consistently regardless of which plugin produces it. The interface is defined in `ObsController.PluginContract`:

```csharp
namespace ObsController.PluginContract;

public interface IPluginLogger
{
    void LogInfo(string message);
    void LogWarning(string message);
    void LogError(string message);
}
```

Plugins store the logger received in `InitializeAsync` and use it exclusively — no direct `Console` calls.

### 4.3 Plugin Discovery

1. The host looks for a directory named `plugins/` adjacent to `ObsController.exe`. This path is **fixed** — it is not configurable via CLI or config file.
2. It enumerates every `*.dll` in that directory (non-recursive).
3. Each DLL is loaded with `Assembly.LoadFrom`.
4. The host scans for all non-abstract types that implement `IActionPlugin`, instantiates each with a parameterless constructor, and registers them in a `Dictionary<string, IActionPlugin>` keyed by `TypeName`.
5. If two plugins claim the same `TypeName`, the host logs a fatal error and exits.
6. If a DLL fails to load (bad format, missing dependency, etc.), the host logs a warning and continues with the remaining DLLs.
7. If a plugin's `InitializeAsync` throws, the host logs a warning, unregisters that plugin, and continues — button presses mapped to that `type` will be logged as unhandled.

### 4.4 Plugin Lifecycle

| Phase | Host action |
|---|---|
| **Startup** | Discover DLLs → instantiate plugins → call `InitializeAsync` for each in registration order. |
| **Runtime** | On each button press, resolve plugin by `type`, call `ExecuteAsync`. |
| **Shutdown** | Call `DisposeAsync` on every successfully initialised plugin (reverse registration order). |

### 4.5 Per-Plugin Config Block in `mapping.json`

Each plugin may have a top-level config block in `mapping.json` under a key that matches its `TypeName`. The host extracts that key (as a raw `JsonElement`) and passes it to `InitializeAsync`. Plugins that need no shared config simply ignore a `null` argument.

```json
{
  "obs": {
    "host": "localhost",
    "port": 4455,
    "password": ""
  },
  "buttonMap": { ... }
}
```

The `http` plugin currently has no shared config and therefore no top-level block. This may change in a future iteration.

### 4.6 Shipping Plan

Both plugins are built in this effort:

| Plugin DLL | TypeName | Scope |
|---|---|---|
| `ObsController.Obs.dll` | `"obs"` | Refactor of existing `ObsBridge` + `ActionExecutor` OBS path. |
| `ObsController.Http.dll` | `"http"` | New implementation; see §6. |

The existing `ObsBridge`, `ActionExecutor`, and `*Requests` classes move into `ObsController.Obs` and are removed from the main exe.

### 4.7 Backward Compatibility

- A `mapping.json` entry with no `type` field defaults to `"obs"` — identical to the current behaviour.
- The `obs` plugin still reads `host`, `port`, and `password` from the top-level `"obs"` block, so existing `mapping.json` files that use those top-level fields continue to work after the OBS block is renamed from the root to `"obs": { ... }`.

> **Migration note:** The `host`, `port`, and `password` fields move from the root of `mapping.json` into an `"obs": { }` block. The host should emit a clear warning if it finds them at the root level to guide users during the transition.

---

## 5. Config Schema

### 5.1 `type` Discriminator

| `type` value | Plugin |
|---|---|
| `"obs"` (or omitted) | OBS WebSocket plugin |
| `"http"` | HTTP plugin |
| Any other string | Resolved against loaded plugins; error if not found |

### 5.2 Full `mapping.json` Example (post-migration)

```json
{
  "deviceIdentifier": "{wgi/nrid/...}",
  "obs": {
    "host": "localhost",
    "port": 4455,
    "password": ""
  },
  "buttonMap": {
    "0": {
      "type": "obs",
      "category": "Scenes",
      "action": "SetCurrentProgramScene",
      "parameters": { "sceneName": "Intro" }
    },
    "1": {
      "category": "Scenes",
      "action": "SetCurrentProgramScene",
      "parameters": { "sceneName": "Main" }
    },
    "5": {
      "type": "http",
      "url": "http://homelab/lights/on",
      "method": "POST",
      "headers": {
        "Authorization": "Bearer abc123"
      },
      "body": { "state": true, "brightness": 80 }
    },
    "6": {
      "type": "http",
      "url": "http://homelab/lights/off",
      "method": "POST",
      "body": { "state": false }
    },
    "7": {
      "type": "http",
      "url": "http://homelab/status"
    }
  }
}
```

---

## 6. HTTP Plugin (`ObsController.Http`)

### 6.1 Action Fields

All fields are inline in the button entry. There is no shared top-level `"http"` config block at this time.

| Field | Type | Required | Description |
|---|---|---|---|
| `type` | `string` | Yes | Must be `"http"`. |
| `url` | `string` | Yes | Fully-qualified URL including scheme, e.g. `http://homelab/lights/on`. |
| `method` | `string` | No | HTTP verb: `GET`, `POST`, `PUT`, or `DELETE`. Defaults to `GET`. Case-insensitive. |
| `headers` | `object` | No | Key/value string pairs added to the request, e.g. `{"Authorization": "Bearer token123"}`. |
| `body` | `string` or `object` | No | Request body. If an object, serialized to JSON with `Content-Type: application/json` set automatically (unless overridden in `headers`). If a string, sent as-is. Ignored for `GET` and `DELETE`. |

### 6.2 Execution Model

- HTTP actions are **fire-and-forget**: the request is dispatched asynchronously and `ExecuteAsync` returns immediately without waiting for a response.
- The response body is **discarded**; only the status code is examined for logging.

### 6.3 Error Handling

- A **non-2xx status code** is a soft error: log the URL, method, and status code, then return. Must not throw.
- A **network error** (timeout, DNS, connection refused) is a soft error: log the URL and exception message, then return. Must not throw.
- **No automatic retry**. Each button press triggers exactly one attempt.

### 6.4 Timeout

- Default request timeout: **10 seconds**. No per-action override.

### 6.5 Content-Type Auto-Detection

- Body is a JSON object in config → inject `Content-Type: application/json` unless already present in `headers`.
- Body is a plain string → no automatic `Content-Type`.

### 6.6 `HttpClient` Management

- The plugin creates a single `HttpClient` instance in `InitializeAsync` and reuses it for every request.
- `DisposeAsync` disposes the client.
- A new instance must not be created per-request (socket exhaustion).

### 6.7 Logging

The plugin uses the `IPluginLogger` received in `InitializeAsync` — no direct `Console` calls. All log lines use the prefix `[HTTP]`:

```
[HTTP] POST http://homelab/lights/on → 200 OK
[HTTP] POST http://homelab/lights/on → 503 Service Unavailable
[HTTP] GET  http://homelab/status    → ERROR Connection refused
```

---

## 7. OBS Plugin (`ObsController.Obs`)

The OBS plugin is a refactor of the existing code, not new functionality. Its requirements are:

- Implement `IActionPlugin` with `TypeName = "obs"`.
- On `InitializeAsync`, read `host`, `port`, and `password` from the `"obs"` config block and connect `ObsBridge`.
- `ExecuteAsync` delegates to the existing reflection-based `ActionExecutor` logic (moved into the plugin).
- On `DisposeAsync`, close the WebSocket connection gracefully.
- If `ObsBridge` fails to connect during `InitializeAsync`, the plugin logs the error and marks itself as unavailable. Subsequent `ExecuteAsync` calls on OBS-typed actions log a warning and return — they must not throw or crash the host.

---

## 8. Host Architecture Changes (`ObsController.exe`)

### 8.1 New shared contract project

```
ObsController.PluginContract/
├── IActionPlugin.cs
├── IPluginLogger.cs
└── ActionMapping.cs       (moved here from Models/)
```

Both the host and all plugins reference this as a **project reference** within the same solution. It has no dependencies beyond `System.Text.Json`.

### 8.2 Revised startup sequence (`Program.cs`)

```
1. Load mapping.json via ConfigLoader.
2. Discover and load plugins from plugins/ folder (fixed path).
3. Call InitializeAsync on each plugin, passing its top-level config block
   and a host-created IPluginLogger instance.
4. Subscribe to gamepad StateChanged events.
5. On each button press → look up type → call plugin.ExecuteAsync (fire-and-forget).
6. On exit → call DisposeAsync on all plugins.
```

### 8.3 Revised solution structure

```
ObsController.sln
├── ObsController/              Host exe (thin — config, gamepad, plugin loader)
├── ObsController.PluginContract/  Shared interface & ActionMapping model
├── ObsController.Obs/          OBS plugin (refactored from host)
└── ObsController.Http/         HTTP plugin (new)
```

### 8.4 Output layout

```
ObsController.exe
plugins/
├── ObsController.Obs.dll
└── ObsController.Http.dll
```

---

## 9. Validation Rules (config load time)

| Condition | Severity | Action |
|---|---|---|
| `type` resolves to no loaded plugin | Error | Log and skip the entry. |
| `type` is `"http"` and `url` is missing or empty | Error | Log and skip the entry. |
| `method` is not `GET`, `POST`, `PUT`, or `DELETE` | Error | Log and skip the entry. |
| `body` present on `GET` or `DELETE` | Warning | Log and ignore the body field. |
| Two plugins claim the same `TypeName` | Fatal | Log and exit. |
| A plugin DLL fails to load | Warning | Log and continue without that plugin. |
| Root-level `host`/`port`/`password` found (old format) | Warning | Emit migration hint; ignore them. |

---

## 10. Resolved Decisions

| # | Question | Decision |
|---|---|---|
| 1 | Should the `plugins/` path be configurable via CLI? | **No.** Fixed to `plugins/` adjacent to the exe. |
| 2 | Should mapping validation errors skip individual entries or abort startup? | **Skip the bad entry.** Log the problem and continue; other actions remain functional. |
| 3 | Should plugins write to `Console` directly or receive a logger from the host? | **Shared logger abstraction.** The host passes an `IPluginLogger` to each plugin in `InitializeAsync`. Plugins must not call `Console` directly. |
| 4 | Should `ObsController.PluginContract` be distributed as a NuGet package or a project reference? | **Project reference** within the same solution. External plugin authoring is not a current requirement. |
