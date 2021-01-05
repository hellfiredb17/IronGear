using System;
using System.Collections.Generic;
using Hawkeye.GameStates;

namespace Hawkeye.NetMessages
{
    [System.Serializable]
    public abstract class NetMessage
    {
        public enum NetMessageType
        {
            NetMessage,

            RequestDedi,
            RequestLobby,
            RequestGame,

            ResponseClient,
            ResponseLobby,
            ResponseGame
        }

        //---------- Map of all Request Messages ----------
        //-------------------------------------------------
        #region Request Message Maps
        /// <summary>
        /// Map of all Request Dedi Messages to type
        /// </summary>
        public static Dictionary<string, Type> RequestDediMessages = new Dictionary<string, Type>()
        {
               { typeof(RequestCreateLobby).ToString(), typeof(RequestCreateLobby) },
               { typeof(RequestLobbyList).ToString(), typeof(RequestLobbyList) },
        };

        /// <summary>
        /// Map of all Request Lobby Messages to type
        /// </summary>
        public static Dictionary<string, Type> RequestLobbyMessages = new Dictionary<string, Type>()
        {
            { typeof(RequestJoinLobby).ToString(), typeof(RequestJoinLobby) },
            { typeof(RequestLeaveLobby).ToString(), typeof(RequestLeaveLobby) },
            { typeof(RequestLobbyChat).ToString(), typeof(RequestLobbyChat) },
            { typeof(RequestLobbyToggleReady).ToString(), typeof(RequestLobbyToggleReady) },
        };

        /// <summary>
        /// Map of all Request Game Messages to type
        /// </summary>
        public static Dictionary<string, Type> RequestGameMessages = new Dictionary<string, Type>()
        {

        };
        #endregion

        //---------- Map of all Response Messages ----------
        //--------------------------------------------------
        #region Response Message Maps
        /// <summary>
        /// Map of all Response Client Messages to type
        /// </summary>
        public static Dictionary<string, Type> ResponseClientMessages = new Dictionary<string, Type>()
        {
            { typeof(ResponseCreateClient).ToString(), typeof(ResponseCreateClient) },
            { typeof(ResponseCreateLobby).ToString(), typeof(ResponseCreateLobby) },
            { typeof(ResponseLobbyList).ToString(), typeof(ResponseLobbyList) },
        };

        /// <summary>
        /// Map of all Response Lobby Messages to type
        /// </summary>
        public static Dictionary<string, Type> ResponseLobbyMessages = new Dictionary<string, Type>()
        {
            { typeof(ResponseUpdateLobby).ToString(), typeof(ResponseUpdateLobby) },
        };

        /// <summary>
        /// Map of all Response Game Messages to type
        /// </summary>
        public static Dictionary<string, Type> ResponseGameMessages = new Dictionary<string, Type>()
        {            
        };
        #endregion

        //---------- Util ----------
        //--------------------------
        public static NetMessageType GetMessageType(string strType)
        {
            // Requests            
            if(RequestDediMessages.ContainsKey(strType))
            {
                return NetMessageType.RequestDedi;
            }
            if (RequestLobbyMessages.ContainsKey(strType))
            {
                return NetMessageType.RequestLobby;
            }
            if (RequestGameMessages.ContainsKey(strType))
            {
                return NetMessageType.RequestGame;
            }

            // Responses
            if (ResponseClientMessages.ContainsKey(strType))
            {
                return NetMessageType.ResponseClient;
            }
            if (ResponseLobbyMessages.ContainsKey(strType))
            {
                return NetMessageType.ResponseLobby;
            }
            if (ResponseGameMessages.ContainsKey(strType))
            {
                return NetMessageType.ResponseGame;
            }

            return NetMessageType.NetMessage;
        }

    } // end class

    //---------- Different Request Messages -----------
    // Request messages are from client to server
    //-------------------------------------------------
    #region Requests
    /// <summary>
    /// Request for the server - to create lobbies/games
    /// </summary>
    public abstract class RequestDediMessage : NetMessage
    {
        public abstract void Process(Server server);
    }

    /// <summary>
    /// Request for lobby
    /// </summary>
    public abstract class RequestLobbyMessage : NetMessage
    {
        public string LobbyId;
        public abstract void Process(DediLobbyState lobbyState);
    }

    /// <summary>
    /// Request for game
    /// </summary>
    public abstract class RequestGameMessage : NetMessage
    {
        public string GameId;
        public abstract void Process(DediGameState gameState);
    }
    #endregion

    //---------- Different Response Message ----------
    // Response messages are from server to client
    //------------------------------------------------
    #region Responses
    /// <summary>
    /// Request for client - to create lobby / game
    /// </summary>
    public abstract class ResponseClientMessage : NetMessage
    {
        public abstract void Process(Client client);
    }

    /// <summary>
    /// Response for lobby
    /// </summary>
    public abstract class ResponseLobbyMessage : NetMessage
    {
        public abstract void Process(ClientLobbyState lobbyState);
    }

    /// <summary>
    /// Response for game
    /// </summary>
    public abstract class ResponseGameMessage : NetMessage
    {
        public abstract void Process(ClientGameState gameState);
    }
    #endregion

} // end namespace
