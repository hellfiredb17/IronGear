using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using Hawkeye.NetMessages;
using Hawkeye.GameStates;
using Hawkeye.Models;

namespace Hawkeye
{
    public class Server
    {
        //---- Variables
        //--------------
        private TcpListener listener;        
        private Dictionary<string, ClientConnection> connections;
        private HashSet<string> usedNetworkIds;

        private Dictionary<string, DediLobbyState> lobbies;

        //---- Ctor
        //---------
        public Server()
        {
            connections = new Dictionary<string, ClientConnection>();
            usedNetworkIds = new HashSet<string>();
            lobbies = new Dictionary<string, DediLobbyState>();
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
            connection.OnProcessNetMessage = ProcessNetMessage;
            connections.Add(connection.Id, connection);

            // start reading incoming messages from connection
            connection.BeginReadMessages();

            // send back connection accept message
            connection.Send(new ResponseCreateClient(connection.Id));
        }

        //---- Processing Of NetMessages
        //------------------------------
        private void ProcessNetMessage(string message, string type)
        {
            // enum for request type, cast to correct message type for processing
            NetMessage.NetMessageType t = NetMessage.GetMessageType(type);
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
            }
        }

        //---- Connection
        //---------------
        public ClientConnection GetConnection(string id)
        {
            ClientConnection connection;
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

        //---- Lobby
        //----------
        #region Lobby Functions
        public DediLobbyState CreateLobby(string displayName, int maxPlayers, string hostId)
        {
            ClientConnection host = GetConnection(hostId);
            if(host == null)
            {
                return null;
            }

            DediLobbyState lobbyState = new DediLobbyState(GetNetworkId(), displayName, maxPlayers);
            lobbyState.AddConnection(host, displayName);

            // add lobby to server
            lobbies.Add(lobbyState.Model.LobbyId, lobbyState);

            return lobbyState;
        }

        public DediLobbyState GetLobby(string lobbyId)
        {
            DediLobbyState lobbyState;
            if(!lobbies.TryGetValue(lobbyId, out lobbyState))
            {
                Debug.LogError($"Unable to get lobby:{lobbyId}");
            }
            return lobbyState;
        }

        public LobbyInfo[] GetLobbyInfoList()
        {
            List<LobbyInfo> lobbyInfo = new List<LobbyInfo>();
            foreach(var lobby in lobbies)
            {
                DediLobbyState dediLobbyState = lobby.Value;
                LobbyInfo info = new LobbyInfo(dediLobbyState.Model.LobbyId, dediLobbyState.Model.LobbyName, 
                    dediLobbyState.Players.Count, dediLobbyState.Model.MaxPlayers);
                lobbyInfo.Add(info);
            }
            return lobbyInfo.ToArray();
        }
        #endregion

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
            foreach(var lobby in lobbies)
            {
                lobby.Value.FixedUpdate(dt);
            }
        }

        //---- Send
        //---------
        public void Send(string id, NetMessage netMessage)
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
        public void Broadcast(NetMessage netMessage)
        {
            foreach (var connection in connections)
            {
                connection.Value.Send(netMessage);
            }
        }
    } // end class
} // end namespace
