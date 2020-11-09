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

        //---- Properties
        //---------------
        public bool Connected => clients?.Count > 0;

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

            // create connection
            ServerClient tcpClient = new ServerClient(clients.Count);
            tcpClient.tcp.Connect(client);
            // add to list
            clients.Add(clients.Count, tcpClient);

            // send client configure information
            Send(tcpClient.Id, new ClientConfigure(tcpClient.Id));
        }

        //---- Send
        //---------
        public void Send(int id, NetMessage netMessage)
        {
            // Parse message into json
            string json = JsonUtility.ToJson(netMessage);
            Send(id, json);
        }

        private void Send(int id, string netMessage)
        {
            if(!clients.ContainsKey(id))
            {
                Debug.LogError($"[Server]: Does not contain key:{id} for client");
                return;
            }
            clients[id].tcp.Send(netMessage);
        }

        //---- Broadcast
        //--------------
        public void Broadcast(NetMessage netMessage)
        {
            // Parse message into json
            string json = JsonUtility.ToJson(netMessage);
            Broadcast(json);
        }

        private void Broadcast(string netMessage)
        {
            foreach(var client in clients)
            {
                client.Value.tcp.Send(netMessage);
            }
        }
    } // end class
} // end namespace
