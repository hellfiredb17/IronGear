using System;
using Hawkeye.NetMessages;

namespace Hawkeye
{
    //---------- ILobby Dedi Listener ------------
    // Listener that handles messages from clients
    //--------------------------------------------
    public interface ILobbyDediListener : INetworkListener
    {
        void OnCreateLobby(CreateLobby createLobby);
        void OnRequestLobbyList(ResponseLobbyList responseLobbyList);
        void OnJoinLobby(JoinLobby joinLobby);
        void OnLeaveLobby(LeaveLobby leaveLobby);
        void OnPlayerReady(PlayerReady playerReady);
        void OnPlayerChat(PlayerChat playerChat);
    }

    //---------- ILobby Client Listener ----------
    // Listener that handles message from dedi
    //--------------------------------------------
    public interface ILobbyClientListener : INetworkListener
    {    
        void OnUpdateLobbyState(UpdateLobbyState updateLobbyState);
        void OnResponseLobbyList(ResponseLobbyList responseLobbyList);
    }

} // end namespace
