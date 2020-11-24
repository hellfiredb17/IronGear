using System;
using System.Collections.Generic;

namespace Hawkeye
{
    [System.Serializable]
    public abstract class NetMessage
    {
        //---------- Map of all NetMessages to dictionary ---------
        //---------------------------------------------------------
        public static Dictionary<string, Type> NetMessageTypes = new Dictionary<string, Type>()
        {
            // Connection Messages
            { typeof(ConnectionEstablished).ToString(), typeof(ConnectionEstablished) },

            // Lobby Messages
            { typeof(CreateLobby).ToString(), typeof(CreateLobby) },
            { typeof(GetLobbyList).ToString(), typeof(GetLobbyList) },
            { typeof(ConnectToLobby).ToString(), typeof(ConnectToLobby) },
            { typeof(SendLobbyChat).ToString(), typeof(SendLobbyChat) },
            { typeof(SendLobbyList).ToString(), typeof(SendLobbyList) },
            { typeof(SendLobbyState).ToString(), typeof(SendLobbyState) },
        };

        public abstract void Process(GameState gameState);

    } // end class
} // end namespace
