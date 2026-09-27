using UnityEngine;

namespace RallyRacing.Data
{
    [CreateAssetMenu(menuName = "Rally Racing/Race Settings", fileName = "RaceSettings")]
    public sealed class RaceSettings : ScriptableObject
    {
        [Min(1)] public int laps = 3;
        [Range(2, 8)] public int maximumPlayers = 8;
        [Min(0)] public int countdownSeconds = 3;
    }
}
