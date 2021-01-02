using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using Hawkeye.NetMessages;

namespace Hawkeye.Server
{
    public class Server
    {
        //---- Variables
        //--------------
        private TcpListener listener;        
        private Dictionary<string, ClientConnection> connections;
        private HashSet<string> usedNetworkIds;

        //---- Ctor
        //---------
        public Server()
        {
            connections = new Dictionary<string, ClientConnection>();
            usedNetworkIds = new HashSet<string>();
        }

        //---- Open
        //---------
        public void Open(string ipAddress, int port, Action<int> onConnection = null)
        {
            Open(IPAddress.Parse(ipAddress), port, onConnection);
        }

        public void Open(IPAddress ipAddress, int port, Action<int> onConnection = null)
        {            
            listener = new TcpListener(ipAddress, port);
            listener.Start();

            // Open for connections
            Debug.Log($"[Server]:Open for connections on: {ipAddress}");
            listener.BeginAcceptTcpClient(new AsyncCallback(ServerAcceptClient), null);
        }

        private void ServerAcceptClient(IAsyncResult result)
        {
            TcpClient client = listener.EndAcceptTcpClient(result);

            // Re-Open for connections
            listener.BeginAcceptTcpClient(new AsyncCallback(ServerAcceptClient), null);
            
            // Connections maxed out
            if(connections.Count == SharedConsts.MAXCONNECTIONS)
            {
                Debug.Log("[Server]: Failed to connect: Server full");
                return;
            }

            // create connection
            ClientConnection connection = new ClientConnection(GetNetworkId(), client);
            connections.Add(connection.Id, connection);

            // start reading incoming messages from connection
            connection.BeginReadMessages();

            // send back connection accept message
            connection.Send(new ResponseConnection(connection.Id));
        }

        private string GetNetworkId()
        {
            string id;
            do
            {
                id = Guid.NewGuid().ToString();
            }
            while (usedNetworkIds.Contains(id));
            usedNetworkIds.Add(id);
            return id;
        }

        //---- Close
        //----------
        public void Close()
        {            
            listener.Stop();
            // TODO
        }

        //---- Update
        //-----------
        public void FixedUpdate(float dt)
        {

        }

        //---- Send
        //---------
        public void Send(string id, ResponseMessage netMessage)
        {
            ClientConnection connection;
            if(!connections.TryGetValue(id, out connection))
            {
                Debug.LogError($"Unable to find connection:{id}");
                return;
            }
            connection.Send(netMessage);
        }

        //---- Broadcast
        //--------------
        public void Broadcast(ResponseMessage netMessage)
        {
            foreach (var connection in connections)
            {
                connection.Value.Send(netMessage);
            }
        }
    } // end class
} // end namespace
