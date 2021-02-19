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
            {typeof(UpdateConnectionState).ToString(),  typeof(UpdateConnectionState)},

            // -- client to dedi
            {typeof(ClientInformation).ToString(),  typeof(ClientInformation)},            
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
        public override string InterfaceType => InterfaceTypes.Connection.ToString();
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
    public class ClientInformation : ClientToDediConnectionMessage
    {
        //---- Variables
        //--------------
        public ConnectionState State;

        //---- Ctor
        //---------
        public ClientInformation(ConnectionState state)
        {
            Token = state.NetworkToken;
            ClientId = state.ClientId;
            State = state;
        }
    }
    #endregion

    //---------- Dedi to Client Messages -------------
    //------------------------------------------------
    #region Dedi to Client
    [Serializable]
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

    [Serializable]
    public class UpdateConnectionState : DediToClientConnectionMessage
    {
        //---- Variables
        //--------------
        public ConnectionState State;

        //---- Ctor
        //---------
        public UpdateConnectionState(ConnectionState state)
        {
            State = state;
        }
    }
    #endregion

} // end namespace

