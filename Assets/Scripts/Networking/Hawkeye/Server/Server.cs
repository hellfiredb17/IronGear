using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Hawkeye.Server
{
    public class Server
    {   
        //---- Variables
        //--------------
        private TcpListener listener;        
        private Dictionary<int, ServerClient> clients;

        //---- Ctor
        //---------
        public Server()
        {
            clients = new Dictionary<int, ServerClient>();
        }

        //---- Open
        //---------
        public void Open(string ipAddress, int port)
        {
            Open(IPAddress.Parse(ipAddress), port);
        }

        public void Open(IPAddress ipAddress, int port)
        {            
            listener = new TcpListener(ipAddress, port);
            listener.Start();

            Debug.Log($"[Server]: Open for connections on {ipAddress.ToString()}");

            // Open for connections
            listener.BeginAcceptTcpClient(new AsyncCallback(ServerAcceptClient), null);
        }

        private void ServerAcceptClient(IAsyncResult result)
        {
            TcpClient client = listener.EndAcceptTcpClient(result);

            // Re-Open for connections
            listener.BeginAcceptTcpClient(new AsyncCallback(ServerAcceptClient), null);

            Debug.Log($"[Server]: Incoming connection from {client.Client.RemoteEndPoint}...");
            if(clients.Count == SharedConsts.MAXCONNECTIONS)
            {
                Debug.Log("[Server]: Failed to connect: Server full");
                return;
            }

            ServerClient tcpClient = new ServerClient(clients.Count);
            tcpClient.tcp.Connect(client);
            clients.Add(clients.Count, tcpClient);
        }
    } // end class
} // end namespace
