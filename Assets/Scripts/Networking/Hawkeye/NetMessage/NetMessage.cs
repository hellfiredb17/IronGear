using System;
using System.Collections.Generic;
using Hawkeye.GameStates;

namespace Hawkeye.NetMessages
{
    [System.Serializable]
    public abstract class NetMessage
    {
        /// <summary>
        /// Map of all Request Messages to type
        /// </summary>
        public static Dictionary<string, Type> RequestMessages = new Dictionary<string, Type>()
        {
            //---- Lobby Messages ----
            //------------------------            
            { typeof(RequestLobby).ToString(), typeof(RequestLobby) },            
        };

        /// <summary>
        /// Map of all Response Messages to type
        /// </summary>
        public static Dictionary<string, Type> ResponseMessages = new Dictionary<string, Type>()
        {
            //---- Connection Messages ----
            //-----------------------------
            { typeof(ResponseConnection).ToString(), typeof(ResponseConnection) },
            { typeof(ResponseConnectionState).ToString(), typeof(ResponseConnectionState) },

            //---- Lobby Messages ----
            //------------------------                        
            { typeof(ResponseLobby).ToString(), typeof(ResponseLobby) },
        };

    } // end class

    /// <summary>
    /// Request Message - Client to Server
    /// </summary>
    public abstract class RequestMessage : NetMessage
    {
        public abstract void Process(IDediGameState gameState);
    }

    /// <summary>
    /// Response Message - Server to Client
    /// </summary>
    public abstract class ResponseMessage : NetMessage
    {
        public abstract void Process(ClientGameState gameState);
    }

} // end namespace
