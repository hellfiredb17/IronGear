using System;
using System.Net.Sockets;
using UnityEngine;

namespace Hawkeye.Server
{
    public class Client
    {
        public int Id;
        public TCP tcp;

        public Client(int clientId)
        {
            Id = clientId;
            tcp = new TCP(Id);
        }

        public class TCP
        {
            public TcpClient socket;
            private NetworkStream stream;
            private byte[] receiveBuffer;
            private readonly int id;

            public TCP(int id)
            {
                this.id = id;
            }

            public void Connect(TcpClient socket)
            {
                this.socket = socket;
                this.socket.ReceiveBufferSize = Shared.DATABUFFERSIZE;
                this.socket.SendBufferSize = Shared.DATABUFFERSIZE;
                stream = this.socket.GetStream();
                receiveBuffer = new byte[Shared.DATABUFFERSIZE];

                // Start reading buffer
                stream.BeginRead(receiveBuffer, 0, Shared.DATABUFFERSIZE, ReceiveCallback, null);

                // TODO: send weclome packet
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

                    // TODO: handle data

                    // start reading again
                    stream.BeginRead(receiveBuffer, 0, Shared.DATABUFFERSIZE, ReceiveCallback, null);
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
