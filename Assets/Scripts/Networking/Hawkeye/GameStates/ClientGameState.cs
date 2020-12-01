using UnityEngine;
using System;

namespace Hawkeye
{
    public class ClientGameState : GameState
    {
        //---- Variables
        //--------------
        private Client.Client client;

        //---- Ctor
        //---------
        public ClientGameState()
        {
            client = new Client.Client();
            client.tcp.OnProcessNetMessage = ProcessNetMessage;            
            Log("Game state created");
        }

        //---- Properties
        //---------------
        public int NetId => client.Id;
        
        //---- Connections
        //----------------
        public void Connect(string ipaddress, int port, Action onConnect = null)
        {
            Log($"Try connection to {ipaddress}:{port}");
            client.ConnectToServer(ipaddress, port, onConnect);
        }

        public void Disconnect()
        {
            client.Disconnect();
        }

        //---- Send Messages
        //------------------
        public void Send(NetMessage netMessage)
        {
            client.Send(netMessage);
        }

        //---- Utils
        //----------
        public void Log(string log)
        {
            Debug.Log($"<color=green>[Client]: {log}</color>");
        }

        public void LogError(string log)
        {
            Debug.LogError($"[Client]: {log}");
        }
    }
}
