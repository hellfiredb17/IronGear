using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hawkeye
{
    public class ServerGameState : GameState
    {
        //---- Variables
        //--------------
        private Server.Server server;
        private int networkId;

        //---- Ctor
        //---------
        public ServerGameState() : base()
        {
            server = new Server.Server();
            server.GetId = GetNetWorkId;
            server.OnConnection = OnConnection;
            server.OnProcessMessage = ProcessNetMessage;            
            Log("Game state created");
        }

        //---- Connection
        //---------------
        public void Open(string ipaddress, int port)
        {
            server.Open(ipaddress, port);
            Log($"Open connections on {ipaddress}:{port}");
        }

        public void Close()
        {
            server.Close();
        }

        private void OnConnection(int id)
        {
            Log($"Connection Established NetId:{id}");
            NetObject connection = new NetObject(id);
            Add(connection);
            server.Send(id, new ConnectionEstablished(id));            
        }

        //---- State Change
        //-----------------
        public override void StateChange(State state)
        {
            switch(state)
            {
                case State.None:
                    break;
                case State.Connecting:
                    break;
                case State.Disconnecting:
                    break;
                case State.Lobby:
                    break;
                case State.Game:
                    break;
            }
            this.state = state;
        }

        //---- Utils
        //----------
        public int GetNetWorkId()
        {
            ++networkId;
            return networkId;
        }

        public void Log(string log)
        {
            Debug.Log($"<color=yellow>[Server]: {log}</color>");
        }

        //---- Send Messages
        //------------------
        public void Send(int id, NetMessage netMessage)
        {
            server.Send(id, netMessage);
        }

        public void BoardCast(NetMessage netMessage)
        {
            server.Broadcast(netMessage);
        }
    }
} // end namespace
