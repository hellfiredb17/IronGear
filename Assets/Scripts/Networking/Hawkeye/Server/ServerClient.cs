using System;
using System.Net.Sockets;
using UnityEngine;
using Hawkeye;

namespace Hawkeye.Server
{
    public class ServerClient
    {
        public int Id;
        public TCP tcp;

        public ServerClient(int clientId)
        {
            Id = clientId;
            tcp = new TCP(Id);
        }

        public class TCP
        {
            //---- Variables
            //--------------
            public TcpClient socket;
            private NetworkStream stream;
            private byte[] receiveBuffer;
            private readonly int id;

            //---- Events
            //-----------
            public Action<string, string> OnProcessNetMessage;

            //---- Ctor
            //---------
            public TCP(int id)
            {
                this.id = id;
            }

            //---- Connect
            //------------
            public void Connect(TcpClient socket)
            {
                this.socket = socket;
                this.socket.ReceiveBufferSize = SharedConsts.DATABUFFERSIZE;
                this.socket.SendBufferSize = SharedConsts.DATABUFFERSIZE;
                stream = this.socket.GetStream();
                receiveBuffer = new byte[SharedConsts.DATABUFFERSIZE];

                // Start reading buffer
                stream.BeginRead(receiveBuffer, 0, SharedConsts.DATABUFFERSIZE, ReceiveCallback, null);
            }

            private void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    int byteLength = stream.EndRead(result);
                    if (byteLength <= 0)
                    {
                        // TODO: disconnect
                        return;
                    }

                    byte[] data = new byte[byteLength];
                    Array.Copy(receiveBuffer, data, byteLength);
                    
                    // read packet
                    NetworkPacket packet = new NetworkPacket();
                    if (packet.Read(data))
                    {
                        OnProcessNetMessage?.Invoke(packet.MessageType, packet.NetworkMessage);
                    }

                    // start reading again
                    stream.BeginRead(receiveBuffer, 0, SharedConsts.DATABUFFERSIZE, ReceiveCallback, null);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error receiving TCP data: {ex}");
                    // TODO: disconnect
                }
            }

            //---- Send
            //---------
            public void Send(NetMessage netMessage)
            {
                NetworkPacket packet = new NetworkPacket();
                if (packet.Write(netMessage))
                {
                    stream.BeginWrite(packet.Bytes, 0, packet.Size, SendPacketCallback, null);
                }
            }

            private void SendPacketCallback(IAsyncResult result)
            {
                stream.EndWrite(result);                
            }
        } // end tcp
    } // end client    
} // end namespace
