using System;
using System.Net.Sockets;
using Hawkeye.NetMessages;

namespace Hawkeye
{
    /// <summary>
    /// Class for a connection to a client
    /// Used on the server
    /// </summary>
    public class DediConnection : Connection
    {
        //---- NetMessage Listeners
        //-------------------------
        public ILobbyDediListener LobbyListener;

        //---- Ctor
        //---------
        public DediConnection(string id, TcpClient socket, ILog log) : base()
        {
            NetworkId = id;
            Socket = socket;            
            Status = SharedEnums.ConnectionStatus.Connect;
            Log = log;
        }

        //---- Reconnect
        //--------------
        public void Reconnect()
        {
            // TODO - try to reconntect to socket
        }

        //---- Send
        //---------
        public void Send(DediToClientMessage netMessage)
        {
            SendNetMessage(netMessage);
        }

        //---- Read Messages
        //------------------
        public void BeginReadMessages()
        {
            Socket.ReceiveBufferSize = SharedConsts.DATABUFFERSIZE;
            Socket.SendBufferSize = SharedConsts.DATABUFFERSIZE;
            stream = Socket.GetStream();
            readBuffer = new byte[SharedConsts.DATABUFFERSIZE];

            // start reading buffer
            stream.BeginRead(readBuffer, 0, SharedConsts.DATABUFFERSIZE, ReceiveCallback, null);
        }

        protected override void ProcessNetMessage(string interfaceType, string messageType, string message)
        {
            // TODO
            base.ProcessNetMessage(interfaceType, messageType, message);
        }

    } // end class
} // end namespace
