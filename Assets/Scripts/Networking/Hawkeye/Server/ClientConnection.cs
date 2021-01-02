using System.Collections.Generic;
using System;
using System.Net.Sockets;
using UnityEngine;
using Hawkeye.NetMessages;

namespace Hawkeye.Server
{
    /// <summary>
    /// Class for a connection to a client
    /// Used on the server
    /// </summary>
    public class ClientConnection
    {
        //---- Variables
        //--------------
        public string Id;
        public TcpClient Socket;
        public Queue<RequestMessage> IncomingMessages;
        public SharedEnums.ConnectionState ConnectionState;

        private NetworkStream stream;
        private NetworkPacket packet;
        private byte[] readBuffer;

        //---- Ctor
        //---------
        public ClientConnection(string id, TcpClient socket)
        {
            Id = id;
            Socket = socket;
            IncomingMessages = new Queue<RequestMessage>();
            ConnectionState = SharedEnums.ConnectionState.Connect;
        }

        //---- Close
        //----------
        public void CloseConnection()
        {
            if(Socket != null)
            {
                Socket.Close();
            }
            if(stream != null)
            {
                stream.Close();
            }
            ConnectionState = SharedEnums.ConnectionState.Close;
        }

        //---- Reconnect
        //--------------
        public void Reconnect()
        {
            // TODO - try to reconntect to socket
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

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int byteLength = stream.EndRead(result);
                if (byteLength <= 0)
                {
                    // Disconnected
                    ConnectionState = SharedEnums.ConnectionState.Disconnect;
                    return;
                }

                // create network packet 
                if(packet == null)
                {
                    packet = new NetworkPacket();
                }

                // get the data
                byte[] data = new byte[byteLength];
                Array.Copy(readBuffer, data, byteLength);

                int read = packet.Read(data);
                if(read == 2)
                {
                    ConnectionState = SharedEnums.ConnectionState.Disconnect;
                    return;
                }
                else if (read == 1)
                {
                    // Net Message done with send, process
                    var netMessage = JsonUtility.FromJson(packet.Message, NetMessage.RequestMessages[packet.Type]) as RequestMessage;
                    IncomingMessages.Enqueue(netMessage);
                    packet.ResetForNewMessage();
                }

                // start reading again
                stream.BeginRead(readBuffer, 0, SharedConsts.DATABUFFERSIZE, ReceiveCallback, null);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error receiving TCP data: {ex}");
                // Disconnected
                ConnectionState = SharedEnums.ConnectionState.Disconnect;
            }
        }

        //---- Send Messages
        //------------------
        public void Send(ResponseMessage message)
        {
            NetworkPacket packet = new NetworkPacket();
            if(packet.Write(message))
            {
                stream.BeginWrite(packet.Buffer, 0, packet.Size, SendPacketCallback, null);
            }
        }

        private void SendPacketCallback(IAsyncResult result)
        {
            stream.EndWrite(result);
        }

    } // end class
} // end namespace
