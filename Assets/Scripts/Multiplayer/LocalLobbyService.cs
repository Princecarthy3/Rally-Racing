using System;
namespace RallyRacing.Multiplayer
{
    public sealed class LocalLobbyService : ILobbyService
    {
        public event Action<int, int> PlayerCountChanged;
        public int PlayerCount { get; private set; }
        public int MaximumPlayers => 8;
        public void JoinLocalLobby(string playerName) { PlayerCount = 1; PlayerCountChanged?.Invoke(PlayerCount, MaximumPlayers); }
        public void SetReady(bool ready) { }
    }
}
