using UnityEngine;
using System.Net.Sockets;
using System;
using Hawkeye.NetMessages;

namespace Hawkeye
{
    public class ClientConnection : Connection
    {
        //---- NetMessage Listeners
        //-------------------------
        public ILobbyClientListener LobbyListener;

        //---- Ctor
        //---------
        public ClientConnection(ILog logger) : base()
        {
            Log = logger;
        }

        //---- Connect
        //------------
        public void Connect(string ipaddress, int port)
        {
            Socket = new TcpClient
            {
                ReceiveBufferSize = SharedConsts.DATABUFFERSIZE,
                SendBufferSize = SharedConsts.DATABUFFERSIZE
            };
            readBuffer = new byte[SharedConsts.DATABUFFERSIZE];
            Socket.BeginConnect(ipaddress, port, ConnectCallback, null);
        }

        private void ConnectCallback(IAsyncResult result)
        {
            Socket.EndConnect(result);
            if(!Socket.Connected)
            {
                return;
            }

            Log?.Output("[Client]: Connected to server");
            Status = SharedEnums.ConnectionStatus.Connect;

            // start reading data
            stream = Socket.GetStream();
            stream.BeginRead(readBuffer, 0, SharedConsts.DATABUFFERSIZE, ReceiveCallback, null);                
        }

        //---- Read Messages
        //------------------
        protected override void ProcessNetMessage(string interfaceType, string messageType, string message)
        {
            switch(NetMessageUtils.GetInterfaceType(interfaceType))
            {
                case InterfaceTypes.None:
                    break;
                case InterfaceTypes.Lobby:
                    if(LobbyListener == null)
                    {
                        Debug.LogError("Lobby listener is null, did you forget to set it?");
                        return;
                    }

                    Type type;
                    if(!LobbyMessageUtils.GetType(messageType, out type))
                    {
                        Debug.LogError($"Unable to get message type: {messageType} is it added to dictionary?");
                        return;
                    }
                    object netMessage = JsonUtility.FromJson(message, type);
                    LobbyListener.OnProcess(netMessage, type);
                    break;
                // TODO any other interfaces here
            }
        }

        //---- Send
        //---------
        public void Send(ClientToDediMessage netMessage)
        {
            netMessage.ClientId = NetworkId;
            SendNetMessage(netMessage);
        }

        //---- Close / Disconnect
        //-----------------------
        public override void Close()
        {
            base.Close();
            // TODO
        }

        public override void Disconnect()
        {
            base.Close();
            // TODO
        }

    } // end client connection
} // end namespace
