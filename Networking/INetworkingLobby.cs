using System;

namespace Backbone.Networking
{
    public interface INetworkingLobby
    {
        void Initialize(int maxConnections);
        void HostLobby();
        void JoinLobby(ulong lobbyId);
        void FindLobbies();
        bool SendLobbyChatMessage(string message);
        bool SetLobbyOwnerData(string key, string value);
        void SetLobbyMemberData(string key, string value);
        bool RequestLobbyData(ulong lobbyId);

        Action<string> OnLobbyChatMessage { get; set; }

    }
}
