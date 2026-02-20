# Controller Hub – Software

## Overview

This directory contains the source code for the **Controller Hub** desktop application. The app reads input from any Windows‑visible gamepad (including BLE devices such as the custom ESP32‑C3 pad described in the top‑level README) and dispatches configurable actions based on a JSON mapping file.

The input layer is abstracted behind an `IGamepadProvider` interface, so alternative input sources can be added without touching the action dispatch logic. Currently the only implemented backend is **OBS Studio**, controlled via its WebSocket v5 API.

* `software/` – a .NET 8 console application in the **ObsController** solution. It handles:
  * Loading a JSON mapping file (`mapping.json`).
  * Discovering and connecting to a gamepad through `Windows.Gaming.Input.RawGameController`.
  * Computing button‑state deltas at ~30 Hz and raising events only for changed inputs.
  * Dispatching configured actions to the active backend (currently OBS via WebSocket).

---

## Architecture

```
mapping.json
     │
     ▼
ConfigLoader ──► Mapping (host / port / password / buttonMap / …)
                      │
          ┌───────────┘
          │
          ▼
IGamepadProvider                          ObsBridge (OBS WebSocket v5)
   ├── RealRawGamepadProvider    ──────────────────────────────────────┐
   │   (Windows.Gaming.Input polling at 30 Hz)                        │
   └── NullGamepadProvider                                             │
       (no‑op fallback)                                               │
          │                                                            │
          │ StateChanged (ControllerDelta)                             │
          ▼                                                            │
     Program.HandleStateChanges                                        │
          │                                                            │
          └── switch(action) → bridge.StartStreamingAsync() / … ──────┘
```

---

## Prerequisites

* **Windows 10/11** – the controller uses `Windows.Gaming.Input.RawGameController`, which is Windows‑only.
* **.NET 8 SDK** – required to build the project (`dotnet build`).
* **OBS Studio** with the WebSocket plugin enabled (default in OBS 28+). Note the port and password.
* (Optional) **ESP32‑C3 board** flashed with `hardware/ble-gamepad/ble-gamepad.ino` to use the custom hardware.

---

## Getting Started

### 1. Clone the repository

```powershell
git clone <repo-url>
cd controller-hub/software
```

### 2. Build the .NET application

```powershell
dotnet restore                # restore NuGet packages
dotnet build -c Release       # produces ObsController.exe under bin/Release/net8.0-windows
```

For a standalone single‑file executable:

```powershell
dotnet publish -c Release /p:PublishSingleFile=true /p:SelfContained=true
```

---

## Configuration (`mapping.json`)

`mapping.json` lives next to the executable and defines the device to listen to, the OBS connection settings, and the button‑to‑action mapping.

```json
{
  "deviceIdentifier": "<NonRoamableId of your gamepad>",
  "host": "localhost",
  "port": 4455,
  "password": "your-obs-websocket-password",
  "buttonMap": {
    "0": { "action": "StartStreaming" },
    "1": { "action": "StopStreaming" },
    "2": { "action": "ToggleRecording" },
    "3": { "action": "SwitchScene", "parameter": "Game" }
  }
}
```

### Fields

| Field | Description |
|---|---|
| `deviceIdentifier` | The `NonRoamableId` string Windows assigns to the gamepad. Run the app once without a valid identifier to see all detected devices printed to the console, then copy the relevant ID. |
| `host` | OBS WebSocket host (default `localhost`). |
| `port` | OBS WebSocket port (default `4455`). |
| `password` | OBS WebSocket password. Leave empty if OBS is configured without authentication. |
| `buttonMap` | Maps zero‑based button indices to actions. Keys are stringified integers. |

> **Note:** Button indices are zero‑based. The Windows "Game Controllers" test dialog uses 1‑based numbering, so subtract 1 when reading from there.

### Supported actions

| Action | `parameter` | Description |
|---|---|---|
| `StartStreaming` | – | Start the OBS stream. |
| `StopStreaming` | – | Stop the OBS stream. |
| `ToggleRecording` | – | Toggle OBS recording on or off. |
| `SwitchScene` | Scene name | Switch OBS to the named programme scene. |

> The `switchMap` and `axisMap` keys are recognised by the data model but are not yet acted upon by the application.

---

## Running the controller

```powershell
# Use all settings from mapping.json
.\ObsController.exe

# Override connection settings on the command line
.\ObsController.exe --host localhost --port 4455 --password "mySecret"
```

All three command‑line options are optional and override the corresponding values in `mapping.json`. On startup the app will:

1. Load `mapping.json`.
2. Connect to OBS via WebSocket and authenticate if a password is set.
3. Enumerate all detected gamepads and print their IDs to the console (useful for finding the right `deviceIdentifier`).
4. Begin polling the selected gamepad at ~30 Hz and dispatching actions on button press.
5. Keep running until **Ctrl+C**.

---

## Extending the application

### Adding a new OBS action

1. Add a `case` for the new action name in `Program.HandleStateChanges`.
2. Add a corresponding method to `ObsBridge` that calls `SendRequestAsync` with the appropriate OBS WebSocket v5 request type (see the [official protocol docs](https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#requests)).

### Adding a new input provider

Implement `IGamepadProvider` (defined in `Services/IGamepadProvider.cs`) and wire it up in `Program.cs`. The rest of the application is agnostic to the input source.

---

## Contributing

Please follow the repository guidelines in `AGENTS.md`:

* Use **4‑space indentation** and standard C# naming conventions.
* Keep commit messages concise (≤ 50 characters subject) with an optional body.
* Do not commit real OBS passwords – use the command‑line overrides instead.

Open a PR that references an existing issue, includes a clear description of the changes, and lists any new dependencies.

---

## License

This project is licensed under the **MIT License** – see `LICENSE` for details.
