using System;
using System.Net.Sockets;
using Hawkeye.NetMessages;
using UnityEngine; // for json - might need to think about this if not in this project

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
            InterfaceTypes type = NetMessageUtils.GetInterfaceType(interfaceType);
            switch(type)
            {
                case InterfaceTypes.Lobby:
                    ProcessLobbyMessages(messageType, message);
                    break;
            }
        }

        private void ProcessLobbyMessages(string messageType, string message)
        {
            if(LobbyMessageUtils.GetType(messageType, out Type type))
            {
                LobbyListener?.OnProcess(JsonUtility.FromJson(message, type), type);
            }
            else
            {
                Log.Error($"Unable to parse type: {messageType}");
            }            
        }

    } // end class
} // end namespace
