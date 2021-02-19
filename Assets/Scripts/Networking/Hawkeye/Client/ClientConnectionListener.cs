using Hawkeye.Models;
using Hawkeye.NetMessages;
using System;

namespace Hawkeye
{
    public class ClientConnectionListener : IClientConnectionListener
    {
        //---- Variables
        //--------------
        private ILog _log;
        private ClientConnection _connection;
        private ClientState _state;

        //---- Ctor
        //---------
        public ClientConnectionListener(ClientState state, ClientConnection connection, ILog log)
        {
            _state = state;
            _connection = connection;
            _log = log;
        }

        //----- Connection Interface
        //--------------------------
        public void OnProcess(object netMessage, Type type)
        {
            if(type == typeof(NetworkToken))
            {
                OnNetworkToken(netMessage as NetworkToken);
            }
            else if ( type == typeof(UpdateConnectionState))
            {
                OnUpdateConnectionState(netMessage as UpdateConnectionState);
            }
        }

        public void OnNetworkToken(NetworkToken token)
        {
            _log.Output($"Recieved network token: {token.Token}");

            _state.NetworkToken = token.Token;            

            // send client state back to server
            _connection.NetworkId = token.Token;
            _connection.Send(new ClientInformation(_state));
        }

        public void OnUpdateConnectionState(UpdateConnectionState state)
        {
            throw new NotImplementedException("TODO");
        }

    } // end class
} // end namespace
