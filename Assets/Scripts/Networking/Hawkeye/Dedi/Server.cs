using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using Hawkeye.NetMessages;
using Hawkeye.Models;

namespace Hawkeye
{
    public class Server
    {
        //---- Variables
        //--------------
        private TcpListener listener;        
        private Dictionary<string, DediConnection> connections;
        private DediLobbies lobbies;
        private HashSet<string> usedNetworkTokens;
        private ILog log;

        //---- Ctor
        //---------
        public Server(ILog logger)
        {
            log = logger;
            connections = new Dictionary<string, DediConnection>();
            usedNetworkTokens = new HashSet<string>();
            lobbies = new DediLobbies(this, log);            
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

            // Open for connections
            log.Output($"Open for connections on: {ipAddress}");
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

            // create token Id
            string token = GetNetworkToken();            
            DediConnection connection = new DediConnection(token, client, log);
            connection.LobbyListener = lobbies;            
            connections.Add(connection.NetworkId, connection);

            // start reading incoming messages from connection
            connection.BeginReadMessages();

            // send back connection accept message
            connection.Send(new NetworkToken(token));
        }

        //---- Connection
        //---------------
        public DediConnection GetConnection(string id)
        {
            DediConnection connection;
            if(!connections.TryGetValue(id, out connection))
            {
                Debug.LogError($"Unable to get connection:{id}");
            }
            return connection;
        }

        //---- Network Ids
        //----------------
        public string GetNetworkToken()
        {
            string id;
            do
            {
                id = Guid.NewGuid().ToString();
            }
            while (usedNetworkTokens.Contains(id));
            usedNetworkTokens.Add(id);
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
            // Lobbies updates
            lobbies.FixedUpdate(dt);
        }

        //---- Send
        //---------
        public void Send(string id, DediToClientMessage netMessage)
        {
            DediConnection connection;
            if(!connections.TryGetValue(id, out connection))
            {
                Debug.LogError($"Unable to find connection:{id}");
                return;
            }
            connection.Send(netMessage);
        }

        //---- Broadcast
        //--------------
        public void Broadcast(DediToClientMessage netMessage)
        {
            foreach (var connection in connections)
            {
                connection.Value.Send(netMessage);
            }
        }
    } // end class
} // end namespace
