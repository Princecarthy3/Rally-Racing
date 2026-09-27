using System.Threading.Tasks;
namespace RallyRacing.Backend
{
    public interface IRallyBackend
    {
        Task<PlayerProfile> AuthenticatePlayer();
        Task<PlayerProfile> GetPlayerProfile();
        Task<int> GetRallyCoins();
        Task SaveRaceResult(RaceResult result);
        Task AddXP(int amount);
        Task<int> AddRallyCoins(int amount);
        Task<PlayerStats> GetPlayerStats();
    }
    public sealed class PlayerProfile { public string Id; public string DisplayName; public int Level; public int XP; }
    public sealed class PlayerStats { public int Played; public int Won; public int Podiums; }
    public sealed class RaceResult { public int Position; public float TimeSeconds; public int Laps; }
}
