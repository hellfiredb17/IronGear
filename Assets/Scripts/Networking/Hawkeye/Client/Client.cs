using UnityEngine;
using System.Net.Sockets;
using System;

namespace Hawkeye.Client
{
    public class Client
    {
        public int Id;
        public TCP tcp;

        public Client()
        {
            tcp = new TCP();
        }

        public void ConnectToServer(string ipaddress, int port)
        {
            tcp.Connect(ipaddress, port);
        }

        public void SetId(int id)
        {
            this.Id = id;
            tcp.SetId(id);
        }

        public void Send(NetMessage netMessage)
        {
            tcp.Send(netMessage);
        }

        public void Disconnect()
        {
            tcp.Disconnect();
        }

        //---- TCP Class ----
        //-------------------
        public class TCP
        {
            //---- Variables
            //--------------
            public TcpClient socket;
            private NetworkStream stream;
            private byte[] receiveBuffer;
            private int id;

            //---- Event
            //----------
            public Action<string, string> OnProcessNetMessage;

            //---- Ctor
            //---------
            public TCP()
            {                
            }

            //---- Setters
            //------------
            public void SetId(int id)
            {
                this.id = id;
            }

            //---- Connect
            //------------
            public void Connect(string ipaddress, int port)
            {
                socket = new TcpClient
                {
                    ReceiveBufferSize = SharedConsts.DATABUFFERSIZE,
                    SendBufferSize = SharedConsts.DATABUFFERSIZE
                };
                receiveBuffer = new byte[SharedConsts.DATABUFFERSIZE];
                socket.BeginConnect(ipaddress, port, ConnectCallback, socket);
            }

            private void ConnectCallback(IAsyncResult result)
            {
                socket.EndConnect(result);
                if(!socket.Connected)
                {
                    return;
                }
                stream = socket.GetStream();
                stream.BeginRead(receiveBuffer, 0, SharedConsts.DATABUFFERSIZE, ReceiveCallback, null);
                //Debug.Log("[Client]: Connected to server");
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
                    if(packet.Read(data))
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

            //---- Disconnect
            //---------------
            public void Disconnect()
            {
                stream.Close();
                socket.Close();
            }

        } // end tcp

    } // end client

} // end namespace
