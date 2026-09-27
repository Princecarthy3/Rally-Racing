using System;
namespace RallyRacing.Multiplayer
{
    /// <summary>Transport/provider seam. Implement with a host-authoritative Unity networking solution later.</summary>
    public interface IRaceNetwork
    {
        bool IsServer { get; }
        event Action<int> PlayerJoined;
        event Action<int> PlayerLeft;
        event Action<int, byte[]> RaceStateReceived;
        void SendInput(int playerId, byte[] input);
        void SendAuthoritativeState(byte[] state);
    }
}
