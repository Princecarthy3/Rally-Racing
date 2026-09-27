using System;
namespace RallyRacing.Multiplayer
{
    public interface ILobbyService
    {
        event Action<int, int> PlayerCountChanged;
        int PlayerCount { get; }
        int MaximumPlayers { get; }
        void JoinLocalLobby(string playerName);
        void SetReady(bool ready);
    }
}
