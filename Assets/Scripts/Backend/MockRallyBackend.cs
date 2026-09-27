using System.Threading.Tasks;
namespace RallyRacing.Backend
{
    /// <summary>Local-only fixture. Replace with authenticated server calls for real progression and currency.</summary>
    public sealed class MockRallyBackend : IRallyBackend
    {
        private readonly PlayerProfile profile = new PlayerProfile { Id = "local-player", DisplayName = "Rally Driver", Level = 1, XP = 0 };
        private int coins = 250;
        private readonly PlayerStats stats = new PlayerStats();
        public Task<PlayerProfile> AuthenticatePlayer() => Task.FromResult(profile);
        public Task<PlayerProfile> GetPlayerProfile() => Task.FromResult(profile);
        public Task<int> GetRallyCoins() => Task.FromResult(coins);
        public Task SaveRaceResult(RaceResult result) { stats.Played++; if (result.Position == 1) stats.Won++; if (result.Position <= 3) stats.Podiums++; return Task.CompletedTask; }
        public Task AddXP(int amount) { profile.XP += amount; profile.Level = 1 + profile.XP / 500; return Task.CompletedTask; }
        public Task<int> AddRallyCoins(int amount) { coins += amount; return Task.FromResult(coins); }
        public Task<PlayerStats> GetPlayerStats() => Task.FromResult(stats);
    }
}
