# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run

All commands run from `software/ObsController/` (or pass the `.csproj` path explicitly):

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

The application reads a BLE gamepad via Windows and forwards button presses to OBS Studio over its WebSocket v5 API.

### Data flow

1. `Program.cs` loads `mapping.json` via `ConfigLoader`, connects `ObsBridge` to OBS, creates an `IGamepadProvider`, and subscribes to `StateChanged` events.
2. `RealRawGamepadProvider` polls `RawGameController.GetCurrentReading` at ~30 Hz. Each poll produces a `ControllerState` snapshot; it is diffed against the previous snapshot to produce a `ControllerDelta`.
3. On each delta, `Program.HandleStateChanges` iterates only *pressed* button changes, looks up the button index (zero-based) in `ButtonMap`, and calls `ActionExecutor.ExecuteAsync`.
4. `ActionExecutor` uses **reflection** to dispatch actions: `category` maps to a `*Requests` class under `ObsController.Services.Requests` (e.g., `"Scenes"` → `ScenesRequests`), and `action` maps to an `*Async` method on that class (e.g., `"SetCurrentProgramScene"` → `SetCurrentProgramSceneAsync`). `parameters` are matched by parameter name and converted from string.
5. Each `*Requests` class calls `ObsBridge.SendRequestAsync`, which sends an OBS WebSocket v5 `op=6` message and waits for the matching `op=7` response via a `ConcurrentDictionary` of `TaskCompletionSource` objects. A background receive loop (`ReceiveLoopAsync`) routes responses.

### Key classes

| Class | Role |
|---|---|
| `Program` | Entry point; CLI options (`--host`, `--port`, `--password`) override `mapping.json` |
| `ObsBridge` | OBS WebSocket v5 client; handles auth (SHA-256 challenge/response) |
| `ActionExecutor` | Reflection-based dispatcher; resolves `{Category}Requests` type and invokes `{Action}Async` |
| `RealRawGamepadProvider` | Polls `Windows.Gaming.Input.RawGameController` at 30 Hz |
| `RawGamepadProvider` | Factory; polls `RawGameControllers` for up to 10 s (the collection is async-populated by Windows) |
| `NullGamepadProvider` | No-op fallback when no controller is found |
| `ControllerState` / `ControllerDelta` | Snapshot and diff model for buttons/switches/axes |
| `ConfigLoader` | Loads `mapping.json` from the exe's directory |

### OBS WebSocket protocol op codes used

- `op=1` – Identify (sent after Hello, with optional auth response)
- `op=5` – Event (pushed to `_eventChannel`, currently unused)
- `op=6` – Request
- `op=7` – RequestResponse (matched to pending request by `requestId`)

### Adding a new OBS action

All OBS WebSocket request categories are already wrapped in `Services/Requests/` (generated from `protocol.md`). To expose a new action in `mapping.json`, no code changes are needed — just use the correct `category` and `action` values. The reflection-based `ActionExecutor` will find and call the method automatically.

If a request category is missing, add a new `*Requests` class that extends `BaseRequests` and calls `_bridge.SendRequestAsync(...)`.

## Configuration (`mapping.json`)

The file lives next to the compiled exe. Current schema:

```json
{
  "deviceIdentifier": "<RawGameController.NonRoamableId>",
  "host": "localhost",
  "port": 4455,
  "password": "",
  "buttonMap": {
    "0": { "category": "Scenes", "action": "SetCurrentProgramScene", "parameters": { "sceneName": "Intro" } },
    "5": { "category": "Stream", "action": "StartStream" }
  }
}
```

- Button indices are **zero-based** (Windows test UI shows 1-based numbers).
- `category` must match a class name prefix in `ObsController.Services.Requests` (e.g., `"Stream"` → `StreamRequests`).
- `action` must match a method name without the `Async` suffix.
- `parameters` keys must match method parameter names exactly (case-sensitive).

## Coding style

- 4-space indentation, no tabs.
- Commit messages ≤ 50 characters with an optional body.
- Do not commit real OBS passwords; use `--password` CLI override or `OBS_PASSWORD` env var convention.
