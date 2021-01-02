using UnityEngine;
using System.Net.Sockets;
using System;
using System.Collections.Generic;
using Hawkeye.NetMessages;

namespace Hawkeye.Client
{
    public class Client
    {
        public string NetworkId;
        public TcpClient Socket;
        public Queue<ResponseMessage> IncomingMessages;
        public SharedEnums.ConnectionState ConnectionState;

        private NetworkStream stream;
        private NetworkPacket packet;
        private byte[] readBuffer;        

        //---- Ctor
        //---------
        public Client()
        {
            ConnectionState = SharedEnums.ConnectionState.None;
            IncomingMessages = new Queue<ResponseMessage>();
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

            Debug.Log("[Client]: Connected to server");
            ConnectionState = SharedEnums.ConnectionState.Connect;

            // start reading data
            stream = Socket.GetStream();
            stream.BeginRead(readBuffer, 0, SharedConsts.DATABUFFERSIZE, ReceiveCallback, null);                
        }

        //---- Read Messages
        //------------------
        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int byteLength = stream.EndRead(result);
                if (byteLength <= 0)
                {
                    ConnectionState = SharedEnums.ConnectionState.Disconnect;
                    return;
                }

                // create network packet for incoming messages
                if(packet == null)
                {
                    packet = new NetworkPacket();                    
                }

                byte[] data = new byte[byteLength];                    
                Array.Copy(readBuffer, data, byteLength);

                // read packet
                int read = packet.Read(data);
                if(read == 2)
                {
                    ConnectionState = SharedEnums.ConnectionState.Disconnect;
                    return;
                }
                else if (read == 1)
                {
                    // Network message done, process
                    var netMessage = JsonUtility.FromJson(packet.Message, NetMessage.ResponseMessages[packet.Type]) as ResponseMessage;
                    IncomingMessages.Enqueue(netMessage);
                    packet.ResetForNewMessage();
                }

                // start reading again
                stream.BeginRead(readBuffer, 0, SharedConsts.DATABUFFERSIZE, ReceiveCallback, null);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error receiving TCP data: {ex}");
                // disconnect
                ConnectionState = SharedEnums.ConnectionState.Disconnect;
            }
        }

        //---- Send
        //---------
        public void Send(RequestMessage netMessage)
        {
            if(ConnectionState != SharedEnums.ConnectionState.Connect)
            {
                Debug.LogError("Trying to send message when not connected");
                return;
            }

            NetworkPacket packet = new NetworkPacket();
            if (packet.Write(netMessage))
            {
                stream.BeginWrite(packet.Buffer, 0, packet.Size, SendPacketCallback, null);
            }
        }

        private void SendPacketCallback(IAsyncResult result)
        {
            stream.EndWrite(result);                
        }

        //---- Close / Disconnect
        //-----------------------
        public void Close()
        {
            stream?.Close();
            Socket?.Close();
        }

        public void Disconnect()
        {
            // TODO ?
        }

    } // end client

} // end namespace
