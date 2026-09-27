# Rally Racing

Rally Racing is a standalone Unity prototype for the Rally ecosystem. This repository is the game client; it does not contain or modify the Rally web application.

## Open and play

1. Install Unity 6 LTS with the Windows build support modules you need.
2. Open this repository in Unity Hub. Unity will resolve packages from `Packages/manifest.json`.
3. Open `Assets/Scenes/Bootstrap.unity` and press Play.

The prototype builds its menus, fictional cars, and simple circuit from code on launch. Use **WASD** or arrow keys to drive, **Space** for handbrake, and **R** to reset the car. Finish three laps by passing the two gates in order on each circuit. It runs as a local single-player slice; online play requires a real networking provider and authoritative host/server.

## Unity version and packages

Project version: Unity 6.0 (`6000.0.32f1` baseline). Packages: URP, Input System, and Unity Test Framework. If Unity Hub offers a newer Unity 6 LTS patch, opening with it is supported.

## Scenes and folders

`Assets/Scenes/Bootstrap.unity` starts the runtime composition root. Runtime-generated views make this starter runnable without hand-authored scene references. Scripts live under `Assets/Scripts` by domain; configurable cars and race settings live in `Assets/ScriptableObjects`.

## Systems

- `GameBootstrap` composes the app services and switches menu, garage, lobby, race, and results views.
- `ArcadeCarController` owns local input and vehicle movement; `FollowCamera` follows the controlled car.
- `RaceSession` owns countdown, ordered gates, laps, timer, finish state, and results.
- `ILobbyService` and `IRaceNetwork` are provider boundaries. The included local implementations are explicitly offline prototypes.
- `IRallyBackend` isolates account/profile/economy operations. `MockRallyBackend` supplies a local test profile and mock rewards.

## Add content

Create a `CarDefinition` asset through **Assets > Create > Rally Racing > Car Definition**, set fictional name and handling values, then add it to the bootstrap car list. A new track can be added as a `TrackDefinition` asset with ordered gate and waypoint data; the current demo track is generated in `DemoTrackBuilder` so its geometry can be iterated without imported art. Cosmetic content should use the `CosmeticDefinition` model and be unlocked through a backend service rather than client-trusted currency.

## Multiplayer and Rally integration

Read [ARCHITECTURE.md](ARCHITECTURE.md) for ownership and event flow, and [RALLY_INTEGRATION.md](RALLY_INTEGRATION.md) for the eventual platform connection. The local lobby is a development seam, not fake network multiplayer. Do not issue real currency, XP, purchases, leaderboard results, or inventory grants from a trusted client. Those decisions must be validated by a server/backend.

## Validation status

Unity was not installed in the environment used to create this repository. The project files have not been imported, compiled, or played in the Unity Editor yet. Open the project in Unity 6 LTS to resolve packages and inspect any editor/compiler diagnostics before building.
