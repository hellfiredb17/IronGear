using UnityEngine;
using System.Net;
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

        public void ConnectToServer()
        {
            tcp.Connect();
        }

        public class TCP
        {
            //---- Variables
            //--------------
            public TcpClient socket;
            private NetworkStream stream;
            private byte[] receiveBuffer;

            //---- Connect
            //------------
            public void Connect()
            {
                socket = new TcpClient
                {
                    ReceiveBufferSize = SharedConsts.DATABUFFERSIZE,
                    SendBufferSize = SharedConsts.DATABUFFERSIZE
                };
                receiveBuffer = new byte[SharedConsts.DATABUFFERSIZE];
                socket.BeginConnect(SharedConsts.LOCAL_IPADDRESS, SharedConsts.PORT, ConnectCallback, socket);
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
                Debug.Log("[Client]: Connected to server");
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

                    NetworkPacket packet = new NetworkPacket();
                    if(packet.Read(data))
                    {
                        // TODO - process message
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

        } // end tcp

    } // end client

} // end namespace
