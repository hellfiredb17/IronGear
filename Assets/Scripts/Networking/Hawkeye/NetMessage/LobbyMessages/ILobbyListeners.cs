using System;
using Hawkeye.Models;

namespace Hawkeye.NetMessages
{
    //---------- ILobby Listener -----------------
    // Base listener class dedi/client share - if any
    //--------------------------------------------
    public interface ILobbyListener
    {
        void OnProcess(object netMessage, Type type);
    }

    //---------- ILobby Dedi Listener ------------
    // Listener that handles messages from clients
    //--------------------------------------------
    public interface ILobbyDediListener : ILobbyListener
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
    public interface ILobbyClientListener : ILobbyListener
    {    
        void OnUpdateLobbyState(UpdateLobbyState updateLobbyState);
        void OnResponseLobbyList(ResponseLobbyList responseLobbyList);
    }

} // end namespace
