using System;
using System.Collections.Generic;
using Hawkeye.Models;

namespace Hawkeye.NetMessages
{
    //---------- Lobby NetMessages Utils ------------
    //-----------------------------------------------
    public static class LobbyMessageUtils
    {
        private static Dictionary<string, Type> _lobbyMessageTypes = new Dictionary<string, Type>
        {
            // -- dedi to client
            {typeof(UpdateLobbyState).ToString(),  typeof(UpdateLobbyState)},
            {typeof(ResponseLobbyList).ToString(),  typeof(ResponseLobbyList)},

            // -- client to dedi
            {typeof(CreateLobby).ToString(),  typeof(CreateLobby)},
            {typeof(JoinLobby).ToString(),  typeof(JoinLobby)},
        };

        public static bool GetType(string str, out Type type)
        {
            return _lobbyMessageTypes.TryGetValue(str, out type);
        }
    }

    //---------- Base Lobby NetMessages -------------
    //-----------------------------------------------
    #region Base Lobby Messages
    [Serializable]
    public abstract class ClientToDediLobbyMessage : ClientToDediMessage
    {
        //---- Variables
        //--------------
        public string LobbyId;

        //---- NetMessage Interface
        //-------------------------
        public override string InterfaceType => InterfaceTypes.Lobby.ToString();
        //public override string NetMessageType => GetType().ToString();
    }

    [Serializable]
    public abstract class DediToClientLobbyMessage : DediToClientMessage
    {
        //---- NetMessage Interface
        //-------------------------
        public override string InterfaceType => InterfaceTypes.Lobby.ToString();
        //public override string NetMessageType => GetType().ToString();
    }
    #endregion

    //---------- Client to Dedi Messages -------------
    //------------------------------------------------
    #region Client to Dedi Messages
    [Serializable]
    public class CreateLobby : ClientToDediLobbyMessage
    {
        //---- Variables
        //--------------
        public string DisplayName;
        public int MaxPlayers;

        //---- Ctor
        //---------
        public CreateLobby(string name, int maxPlayers)
        {   
            DisplayName = name;
            MaxPlayers = maxPlayers;
        }
    }

    [Serializable]
    public class RequestLobbyList : ClientToDediLobbyMessage
    {        
        //---- Ctor
        //---------
        public RequestLobbyList()
        {            
        }
    }

    [Serializable]
    public class JoinLobby : ClientToDediLobbyMessage
    {
        //---- Variables
        //--------------        
        public string PlayerDisplayName;

        //---- Ctor
        //---------
        public JoinLobby(string lobbyId, string playerName)
        {
            LobbyId = lobbyId;            
            PlayerDisplayName = playerName;
        }
    }

    [Serializable]
    public class LeaveLobby : ClientToDediLobbyMessage
    {
        //---- Ctor
        //---------
        public LeaveLobby(string lobbyId)
        {
            LobbyId = lobbyId;            
        }
    }

    [Serializable]
    public class PlayerReady : ClientToDediLobbyMessage
    {
        //---- Variables
        //--------------
        public bool IsReady;

        //---- Ctor
        //---------
        public PlayerReady(string lobbyId, bool ready)
        {
            LobbyId = lobbyId;            
            IsReady = ready;
        }
    }

    [Serializable]
    public class PlayerChat : ClientToDediLobbyMessage
    {
        //---- Variables
        //--------------
        public string Chat;

        //---- Ctor
        //---------
        public PlayerChat(string lobbyId, string chat)
        {
            LobbyId = lobbyId;            
            Chat = chat;
        }
    }
    #endregion

    //---------- Dedi to Client Messages -------------
    //------------------------------------------------
    #region Dedi to Client
    [Serializable]
    public class UpdateLobbyState : DediToClientLobbyMessage
    {
        //---- Variables
        //--------------
        public LobbyState State;

        //---- Ctor
        //---------
        public UpdateLobbyState(LobbyState lobbyState)
        {
            State = lobbyState;
        }
    }

    [Serializable]
    public class ResponseLobbyList : DediToClientLobbyMessage
    {
        //---- Variables
        //--------------
        public LobbyListState LobbyList;

        //---- Ctor
        //---------
        public ResponseLobbyList(LobbyListState lobbyList)
        {
            LobbyList = lobbyList;
        }
    }
    #endregion

} // end namespace
