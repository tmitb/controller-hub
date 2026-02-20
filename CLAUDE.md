# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run

All commands run from `software/ControllerHub/` (or pass the `.csproj` path explicitly):

```powershell
dotnet restore
dotnet build                          # Debug build
dotnet build -c Release               # Release build
dotnet run                            # Run with mapping.json from the exe output dir
dotnet publish -c Release /p:PublishSingleFile=true /p:SelfContained=true  # Single-file exe
```

**Windows-only**: the project targets `net8.0-windows10.0.22621.0` and uses the WinRT `Windows.Gaming.Input` API. It cannot be compiled or run on Linux/macOS.

There are no automated tests.

## Architecture

The application reads a BLE gamepad via Windows and dispatches button presses to action plugins.

### Data flow

1. `Program.cs` loads `mapping.json` via `ConfigLoader`, loads plugins from `plugins/` via `PluginLoader`, creates an `IGamepadProvider`, and subscribes to `StateChanged` events.
2. `RealRawGamepadProvider` polls `RawGameController.GetCurrentReading` at ~30 Hz. Each poll produces a `ControllerState` snapshot; it is diffed against the previous snapshot to produce a `ControllerDelta`.
3. On each delta, `Program.HandleStateChanges` iterates only *pressed* button changes, looks up the button index (zero-based) in `ButtonMap`, and calls `IActionPlugin.ExecuteAsync` on the matching plugin (selected by `"type"` in the button mapping).
4. Each plugin handles its own action dispatch. For example, the OBS plugin (`ControllerHub.Obs`) uses `ActionExecutor` with **reflection**: `category` maps to a `*Requests` class under `ControllerHub.Obs.Requests` (e.g., `"Scenes"` → `ScenesRequests`), and `action` maps to an `*Async` method. The HTTP plugin (`ControllerHub.Http`) sends an HTTP request to a configured URL.
5. The OBS plugin's `*Requests` classes call `ObsBridge.SendRequestAsync`, which sends an OBS WebSocket v5 `op=6` message and waits for the matching `op=7` response via a `ConcurrentDictionary` of `TaskCompletionSource` objects. A background receive loop (`ReceiveLoopAsync`) routes responses.

### Key classes

| Class | Role |
|---|---|
| `Program` | Entry point; CLI options (`--host`, `--port`, `--password`) override `mapping.json` |
| `PluginLoader` | Loads `IActionPlugin` implementations from DLLs in the `plugins/` subdirectory |
| `IActionPlugin` | Plugin contract: `TypeName` (string key), `InitializeAsync`, `ExecuteAsync` |
| `RealRawGamepadProvider` | Polls `Windows.Gaming.Input.RawGameController` at 30 Hz |
| `RawGamepadProvider` | Factory; polls `RawGameControllers` for up to 10 s (the collection is async-populated by Windows) |
| `NullGamepadProvider` | No-op fallback when no controller is found |
| `ControllerState` / `ControllerDelta` | Snapshot and diff model for buttons/switches/axes |
| `ConfigLoader` | Loads `mapping.json` from the exe's directory |
| `ObsBridge` | OBS WebSocket v5 client; handles auth (SHA-256 challenge/response) — lives in `ControllerHub.Obs` |
| `ActionExecutor` | Reflection-based dispatcher for OBS requests — lives in `ControllerHub.Obs` |

### OBS WebSocket protocol op codes used

- `op=1` – Identify (sent after Hello, with optional auth response)
- `op=5` – Event (pushed to `_eventChannel`, currently unused)
- `op=6` – Request
- `op=7` – RequestResponse (matched to pending request by `requestId`)

### Adding a new OBS action

All OBS WebSocket request categories are already wrapped in `ControllerHub.Obs/Requests/` (generated from `protocol.md`). To expose a new action in `mapping.json`, no code changes are needed — just use the correct `category` and `action` values. The reflection-based `ActionExecutor` will find and call the method automatically.

If a request category is missing, add a new `*Requests` class that extends `BaseRequests` and calls `_bridge.SendRequestAsync(...)`.

## Configuration (`mapping.json`)

The file lives next to the compiled exe (in `ControllerHub/bin/...`). Current schema:

```json
{
  "deviceIdentifier": "<RawGameController.NonRoamableId>",
  "buttonMap": {
    "0": { "type": "obs", "category": "Scenes", "action": "SetCurrentProgramScene", "parameters": { "sceneName": "Intro" } },
    "5": { "type": "obs", "category": "Stream", "action": "StartStream" },
    "7": { "type": "http", "url": "http://localhost:8080/trigger" }
  },
  "obs": {
    "host": "localhost",
    "port": 4455,
    "password": ""
  }
}
```

- Button indices are **zero-based** (Windows test UI shows 1-based numbers).
- `type` must match the `TypeName` of a loaded plugin (e.g., `"obs"`, `"http"`).
- For the OBS plugin: `category` must match a class name prefix in `ControllerHub.Obs.Requests` (e.g., `"Stream"` → `StreamRequests`), and `action` must match a method name without the `Async` suffix. `parameters` keys must match method parameter names exactly (case-sensitive).

## Coding style

- 4-space indentation, no tabs.
- Commit messages ≤ 50 characters with an optional body.
- Do not commit real OBS passwords; use `--password` CLI override or `OBS_PASSWORD` env var convention.
