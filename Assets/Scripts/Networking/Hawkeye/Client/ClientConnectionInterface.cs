using Hawkeye.Models;
using Hawkeye.NetMessages;
using System;

namespace Hawkeye
{
    public class ClientConnectionInterface : IClientConnectionListener
    {
        //---- Variables
        //--------------
        private ILog _log;
        private ClientConnection _connection;
        private ConnectionState _state;

        //---- Ctor
        //---------
        public ClientConnectionInterface(ClientConnection connection, ILog log)
        {
            _connection = connection;
            _log = log;

            // TODO - read any client data here
            _state = new ConnectionState();
            _state.ClientId = "DEBUG";
            _state.DisplayName = "DKBUDS";
        }

        //----- Connection Interface
        //--------------------------
        public ConnectionState State => _state;

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
            _state.Status = ConnectionStatus.Online;

            // send client state back to server
            _connection.Send(new ClientInformation(_state));
        }

        public void OnUpdateConnectionState(UpdateConnectionState state)
        {
            throw new NotImplementedException("TODO");
        }

    } // end class
} // end namespace
