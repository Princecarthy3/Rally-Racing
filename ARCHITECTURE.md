# Architecture

## Runtime composition

`Bootstrap.unity` contains a single `GameBootstrap`. It creates the service graph once, retains the composition root across scene changes, and owns the lightweight view flow. The prototype views are generated from code to avoid brittle missing serialized references in an empty repository.

## Race flow

`GameBootstrap` launches `RaceSession` with a selected car and `DemoTrackBuilder` geometry. RaceSession locks the car during 3-2-1-GO, starts its clock on GO, and accepts only the next ordered gate. Gate triggers require forward movement across a gate plane. The finish gate advances a lap only after every intermediate gate has been accepted. Finish state freezes controls and produces immutable result data for the result view.

## Ownership boundaries

- Local input is read by `ArcadeCarController`; a future network owner should feed a command interface into vehicle simulation.
- `IRaceNetwork` describes player and authoritative race events. No online transport is bundled in this offline starter.
- `ILobbyService` exposes lobby membership and ready state; `LocalLobbyService` is single-user scaffolding.
- `IRallyBackend` provides profile, balance, result-save, XP, reward, and stats operations. Replace `MockRallyBackend` with a server-mediated implementation.
- Presentation subscribes to race state/events and does not decide race outcomes.

## Security and future networking

The host/server must validate input bounds, checkpoint order, lap transitions, finish times, placements, rewards, XP, purchases, inventory, and leaderboard writes. Clients must never submit arbitrary coin balances or authoritative finish results. Use a supported Unity networking package/provider behind `IRaceNetwork`; select and configure a transport after deciding hosting, relay, session discovery, and server authority requirements.
