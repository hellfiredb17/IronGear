using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using Hawkeye.NetMessages;

namespace Hawkeye
{
    public class Server
    {
        //---- Variables
        //--------------
        private TcpListener listener;        
        private Dictionary<string, DediConnection> connections;
        private HashSet<string> usedNetworkIds;
        private ILog log;

        //private Dictionary<string, DediLobbyState> lobbies;

        //---- Ctor
        //---------
        public Server(ILog logger)
        {
            log = logger;
            connections = new Dictionary<string, DediConnection>();
            usedNetworkIds = new HashSet<string>();
            //lobbies = new Dictionary<string, DediLobbyState>();
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

            // create connection
            DediConnection connection = new DediConnection(GetNetworkId(), client, log);
            //connection.OnProcessNetMessage = ProcessNetMessage;
            connections.Add(connection.NetworkId, connection);

            // start reading incoming messages from connection
            connection.BeginReadMessages();

            // send back connection accept message
            //connection.Send(new ResponseCreateClient(connection.Id));
        }

        //---- Processing Of NetMessages
        //------------------------------
        private void ProcessNetMessage(string message, string type)
        {
            // enum for request type, cast to correct message type for processing
            /*NetMessage.NetMessageType t = NetMessage.GetMessageType(type);
            switch(t)
            {
                case NetMessage.NetMessageType.RequestDedi:
                    var dediMessage = JsonUtility.FromJson(message, NetMessage.RequestDediMessages[type]) as RequestDediMessage;
                    dediMessage.Process(this);
                    break;
                case NetMessage.NetMessageType.RequestLobby:
                    var lobbyMessage = JsonUtility.FromJson(message, NetMessage.RequestLobbyMessages[type]) as RequestLobbyMessage;
                    DediLobbyState lobby = GetLobby(lobbyMessage.LobbyId);
                    if(lobby == null)
                    {
                        Debug.LogError($"Server cannot process lobbyMessage:{type}\nMessage:{message}");
                        return;
                    }
                    lobby.IncomingMessages.Enqueue(lobbyMessage);
                    break;
                case NetMessage.NetMessageType.RequestGame:
                    var gameMessage = JsonUtility.FromJson(message, NetMessage.RequestGameMessages[type]) as RequestGameMessage;
                    // TODO -
                    break;
                default:
                    Debug.LogError($"Server cannot process netmessage:{type}\nMessage:{message}");
                    break;
            }*/
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
        public string GetNetworkId()
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
            /*foreach(var lobby in lobbies)
            {
                lobby.Value.FixedUpdate(dt);
            }*/
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
