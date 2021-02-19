using System;
using System.Collections.Generic;
using Hawkeye.Models;

namespace Hawkeye.NetMessages
{
    //---------- Connection NetMessages Utils ------------
    //----------------------------------------------------
    public static class ConnectionMessageUtils
    {
        private static Dictionary<string, Type> _connectionMessageTypes = new Dictionary<string, Type>
        {
            // -- dedi to client
            {typeof(NetworkToken).ToString(),  typeof(NetworkToken)},

            // -- client to dedi
            {typeof(ClientConnection).ToString(),  typeof(ClientConnection)},            
        };

        public static bool GetType(string str, out Type type)
        {
            return _connectionMessageTypes.TryGetValue(str, out type);
        }
    }

    //---------- Base Connection NetMessages -------------
    //----------------------------------------------------
    #region Base Connection Messages
    [Serializable]
    public abstract class ClientToDediConnectionMessage : ClientToDediMessage
    {
        //---- Variables
        //--------------
        public string Token;

        //---- NetMessage Interface
        //-------------------------
        public override string InterfaceType => InterfaceTypes.Lobby.ToString();
    }

    [Serializable]
    public abstract class DediToClientConnectionMessage : DediToClientMessage
    {
        //---- NetMessage Interface
        //-------------------------
        public override string InterfaceType => InterfaceTypes.Connection.ToString();
    }
    #endregion

    //---------- Client to Dedi Messages -------------
    //------------------------------------------------
    #region Client to Dedi Messages
    [Serializable]
    public class ClientConnection : ClientToDediConnectionMessage
    {
        //---- Variables
        //--------------        
        public string DisplayName;

        //---- Ctor
        //---------
        public ClientConnection(string token, string id, string name)
        {
            Token = token;
            ClientId = id;
            DisplayName = name;
        }
    }
    #endregion

    //---------- Dedi to Client Messages -------------
    //------------------------------------------------
    #region Dedi to Client
    public class NetworkToken : DediToClientConnectionMessage
    {
        //----- Variables
        //---------------
        public string Token;

        //---- Ctor
        //---------
        public NetworkToken(string token)
        {
            Token = token;
        }
    }
    #endregion

} // end namespace

