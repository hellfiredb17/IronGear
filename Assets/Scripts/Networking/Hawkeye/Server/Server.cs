using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Hawkeye.Server
{
    public class Server
    {
        //---- Delegate
        //-------------
        public delegate int GetNetworkId();

        //---- Variables
        //--------------
        private TcpListener listener;        
        private Dictionary<int, ServerClient> clients;

        //---- Events
        //-----------
        public Action OnConnectionOpen;
        public Action OnConnectionClose;
        public Action<int> OnConnection;
        public Action<int> OnDisconnect;
        public Action<string, string> OnProcessMessage;
        public GetNetworkId GetId;

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
        public void Open(string ipAddress, int port, Action<int> onConnection = null)
        {
            Open(IPAddress.Parse(ipAddress), port, onConnection);
        }

        public void Open(IPAddress ipAddress, int port, Action<int> onConnection = null)
        {            
            listener = new TcpListener(ipAddress, port);
            listener.Start();

            // send event
            OnConnectionOpen?.Invoke();
            OnConnection = onConnection;

            // Open for connections
            listener.BeginAcceptTcpClient(new AsyncCallback(ServerAcceptClient), null);
        }

        private void ServerAcceptClient(IAsyncResult result)
        {
            TcpClient client = listener.EndAcceptTcpClient(result);

            // Re-Open for connections
            listener.BeginAcceptTcpClient(new AsyncCallback(ServerAcceptClient), null);

            //Debug.Log($"[Server]: Incoming connection from {client.Client.RemoteEndPoint}...");
            if(clients.Count == SharedConsts.MAXCONNECTIONS)
            {
                Debug.Log("[Server]: Failed to connect: Server full");
                return;
            }

            // create connection
            int id = GetId == null ? clients.Count : GetId();
            ServerClient tcpClient = new ServerClient(id);
            tcpClient.tcp.Connect(client);
            tcpClient.tcp.OnProcessNetMessage = OnProcessMessage;

            // add to list
            clients.Add(id, tcpClient);

            // send event
            OnConnection?.Invoke(tcpClient.Id);
        }

        //---- Close
        //----------
        public void Close()
        {            
            listener.Stop();
            // send event
            OnConnectionClose?.Invoke();
        }

        //---- Send
        //---------
        public void Send(int id, NetMessage netMessage)
        {
            if (!clients.ContainsKey(id))
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
            foreach (var client in clients)
            {
                client.Value.tcp.Send(netMessage);
            }
        }
    } // end class
} // end namespace
