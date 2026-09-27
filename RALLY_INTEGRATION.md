# Rally platform integration plan

Rally Racing remains an independent Unity repository. It should call a versioned Rally service API, never read or write Rally's database directly from the client.

## Boundary

Keep `IRallyBackend` as the game-facing contract. `MockRallyBackend` supports offline iteration. A future `SupabaseRallyBackend` should call trusted Rally endpoints or narrowly scoped Supabase functions using a public client key only; never embed a service-role or private key in the Unity build.

## Account and data flow

1. Authenticate through the Rally account flow and receive a short-lived user session/token.
2. Fetch the profile, progression, inventory, and balance using the authenticated identity.
3. Enter a network lobby using a server-issued race/session identity.
4. Have the trusted race host/server validate race completion and calculate rewards.
5. Submit a signed/authorized result to a backend endpoint; backend validates and persists history, stats, XP, achievements, and Rally Coins.
6. Refresh profile and inventory from the backend after the result is accepted.

Agree with the Rally repository owners on endpoint contracts, token audience, refresh/revocation behavior, schemas, rate limits, idempotency keys, and privacy rules before implementation. No Rally repository files or credentials are required by this prototype.
