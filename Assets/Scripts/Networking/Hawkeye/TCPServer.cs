using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Hawkeye.Server
{
    public class TCPServer
    {
        //---- Temp
        //---------
        public static int Port = 4400;
        public static int MaxConnections = 4;

        //---- Variables
        //--------------
        private TcpListener listener;
        private IPAddress ipAddress;
        private List<TcpClient> connections;
        private Thread connectionThread;
        private Thread messageThread;

        //---- Ctor
        //---------
        public TCPServer()
        {
            connections = new List<TcpClient>();
        }

        //---- Open
        //---------
        public void Open(IPAddress iPAddress, int port)
        {
            try
            {
                // open ipaddress and port for connections
                ipAddress = iPAddress;
                listener = new TcpListener(ipAddress, port);
                listener.Start();

                // start connection thread
                connectionThread = new Thread(ConnectionLoop);
                connectionThread.Start();

                // start the message thread
                messageThread = new Thread(MessageLoop);
                messageThread.Start();

                Debug.Log("Server open for connections and messages");
            }
            catch(Exception ex)
            {
                Debug.LogError("Open connection exception\n" + ex.ToString());
            }
        }
       
        /// <summary>
        /// Thread that continues to listen for new connections
        /// </summary>
        private async void ConnectionLoop()
        {
            try
            {
                while(true)
                {
                    // continue to check for connection requests
                    if(listener.Pending() && connections.Count < MaxConnections)
                    {
                        TcpClient client = await listener.AcceptTcpClientAsync();
                        int netId = connections.Count;                        
                        Debug.Log($"Client {netId} connected");

                        // Send welcome message back to client
                        NetworkStream stream = client.GetStream();
                        string message = $"Connected to server as client {netId}";
                        byte[] data = Encoding.Default.GetBytes(message);
                        await stream.WriteAsync(data, 0, data.Length);

                        // Add to connections
                        connections.Add(client);
                    }
                }
            }
            catch (ThreadAbortException abort)
            {
                Debug.LogWarning("Connection thread abort\n" + abort.ToString());
            }
            catch (Exception ex)
            {
                Debug.LogError("Connection thread exception\n" + ex.ToString());
            }
        }

        /// <summary>
        /// Thread that continues to listen for incoming messages from clients
        /// </summary>
        private async void MessageLoop()
        {
            try
            {
                while(true)
                {
                    for(int i = 0; i < connections.Count; i++)
                    {
                        /*NetworkStream networkStream = connections[i].GetStream();
                        if (networkStream.DataAvailable && networkStream.CanRead)
                        {
                            byte[] buffer = new byte[1024];                            
                            await networkStream.ReadAsync(buffer, 0, buffer.Length);
                            string message = Encoding.Default.GetString(buffer);
                            Debug.Log($"Message from client {i}: {message}");
                        }*/
                    }
                }
            }
            catch (ThreadAbortException abort)
            {
                Debug.LogWarning("Message thread abort\n" + abort.ToString());
            }
            catch (Exception ex)
            {
                Debug.LogError("Message thread exception\n" + ex.ToString());
            }
        }

        //---- Close
        //----------

        public void Close()
        {
            // Close the listener
            listener?.Stop();

            // Stop connection thread
            connectionThread?.Abort();
            connectionThread?.Join();

            // Close all connections
            for(int i = 0; i < connections.Count; i++)
            {
                if(connections[i].Connected)
                {
                    connections[i].Close();
                }
            }
            connections.Clear();

            // Stop message thread
            messageThread?.Abort();
            messageThread?.Join();

            Debug.Log("Server closed for connections and messages");
        }

        //---- Broadcast
        //--------------

        public void Broadcast(string message)
        {
            byte[] bytes = Encoding.Default.GetBytes(message);
            InternalBroadcast(bytes);
        }

        private async void InternalBroadcast(byte[] message)
        {
            for(int i = 0; i < connections.Count; i++)
            {
                NetworkStream networkStream = connections[i].GetStream();
                if (networkStream.CanWrite)
                {
                    await networkStream.WriteAsync(message, 0, message.Length);
                }
            }
            Debug.Log($"Boardcast message to clients");
        }

        //---- Send
        //---------

    } // end class
} // end namespace
