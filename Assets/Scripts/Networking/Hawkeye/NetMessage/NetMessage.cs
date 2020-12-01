﻿using System;
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
            { typeof(SendChat).ToString(), typeof(SendChat) },
            { typeof(SendLobbyList).ToString(), typeof(SendLobbyList) },
            { typeof(SendLobbyState).ToString(), typeof(SendLobbyState) },
            { typeof(SendLobby).ToString(), typeof(SendLobby) },
        };

        public abstract void Process(GameState gameState);

    } // end class
} // end namespace
