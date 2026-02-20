# Repository Guidelines

## Project Structure & Module Organization

```
.
├─ hardware/                              # 3‑D models and Arduino sketch
│   ├─ 3d-models/                         # STL / PNG files for the shell
│   └─ ble-gamepad/ble-gamepad.ino        # ESP32‑C3 BLE gamepad firmware
├─ images/                                # Photos used in documentation
└─ software/                              # Controller Hub – C# console application
    └─ ObsController/
        ├─ Models/
        │   ├─ Mapping.cs                 # Data model for mapping.json (device ID, OBS settings, button/switch/axis maps)
        │   └─ ControllerState.cs         # Controller state snapshot and delta diffing
        ├─ Services/
        │   ├─ IGamepadProvider.cs        # Input abstraction interface
        │   ├─ RealRawGamepadProvider.cs  # Windows.Gaming.Input polling implementation (~30 Hz)
        │   ├─ NullGamepadProvider.cs     # No‑op fallback provider
        │   ├─ RawGamepadProvider.cs      # Factory that discovers and wraps the target controller
        │   ├─ ObsBridge.cs              # OBS WebSocket v5 client
        │   └─ ConfigLoader.cs           # Loads mapping.json and applies CLI overrides
        ├─ Program.cs                     # Entry point: CLI parsing, wiring, action dispatch loop
        └─ mapping.json                   # User configuration (not committed with real credentials)
```

Source code lives under `software/`; tests (if added) should mirror this layout.

## Coding Style & Naming Conventions

* **Indentation**: 4 spaces, no tabs.
* **Naming**: PascalCase for types and public members; camelCase for local variables and parameters. Follow standard C# conventions throughout.
* Keep methods short and focused; prefer descriptive names over inline comments.
* Do not commit real OBS passwords or device identifiers – use the command‑line overrides.

## Commit Messages

* Subject line ≤ 50 characters, written in the imperative mood (e.g. `Add SwitchScene action`).
* Optional body separated by a blank line; wrap at 72 characters.

---

These guidelines aim to keep contributions consistent and the repository easy to navigate. Feel free to suggest improvements via issues or PRs.
