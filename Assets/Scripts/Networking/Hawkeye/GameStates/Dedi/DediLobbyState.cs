using System.Collections.Generic;
using Hawkeye.NetMessages;
using Hawkeye.Models;

namespace Hawkeye.GameStates
{
    public class DediLobbyState
    {
        //---- Outgoing Message
        //---------------------
        #region Outgoing Message
        private class OutgoingMessage
        {
            public string[] ToSend;
            public NetMessage NetMessage;
        }
        #endregion

        //---- Variables
        //--------------        
        public LobbyModel Model;
        public Dictionary<string, DediLobbyPlayer> Players;
        public Queue<RequestLobbyMessage> IncomingMessages;

        private Queue<OutgoingMessage> OutgoingMessages;        

        //---- Ctor
        //---------
        public DediLobbyState(string id, string displayName, int maxPlayers)
        {
            Model = new LobbyModel(id, displayName, maxPlayers);

            Players = new Dictionary<string, DediLobbyPlayer>();
            IncomingMessages = new Queue<RequestLobbyMessage>();
            OutgoingMessages = new Queue<OutgoingMessage>();
        }

        //---- Update
        //-----------
        public void FixedUpdate(float dt)
        {
            // Process all incoming messages
            if(IncomingMessages.Count > 0)
            {
                while(IncomingMessages.Count > 0)
                {
                    RequestLobbyMessage lobbyMessage = IncomingMessages.Dequeue();
                    lobbyMessage.Process(this);
                }
            }

            // Send any outgoing messages
            if(OutgoingMessages.Count > 0)
            {
                while(OutgoingMessages.Count > 0)
                {
                    OutgoingMessage message = OutgoingMessages.Dequeue();
                    if(message.ToSend == null || message.ToSend.Length == 0)
                    {
                        BroadCast(message.NetMessage);
                    }
                    else
                    {
                        for(int i = 0; i < message.ToSend.Length; i++)
                        {
                            Send(message.ToSend[i], message.NetMessage);
                        }
                    }
                }
            }
        }

        //---- Add / Remove Players
        //-------------------------
        public void AddConnection(ClientConnection connection, string displayName)
        {
            DediLobbyPlayer lobbyPlayer = new DediLobbyPlayer(connection, displayName);
            Model.Players.Add(lobbyPlayer.Model); // update model data
            Players.Add(lobbyPlayer.Connection.Id, lobbyPlayer);
        }

        public void RemoveConnection(string connectionId)
        {
            DediLobbyPlayer player = GetPlayer(connectionId);
            if(player == null)
            {
                return;
            }

            // Update model data
            for(int i = 0; i < Model.Players.Count; i++)
            {
                if(Model.Players[i].NetId == connectionId)
                {
                    Model.Players.RemoveAt(i);
                    break;
                }
            }

            Players.Remove(connectionId);
        }

        //---- Toggle Ready
        //-----------------
        public void ToggleReady(string connectionId)
        {
            DediLobbyPlayer player = GetPlayer(connectionId);
            if(player == null)
            {
                return;
            }
            player.Model.Ready = !player.Model.Ready;
            UnityEngine.Debug.Log($"{player.Model.DisplayName}:" + (player.Model.Ready ? "Ready" : "Not Ready"));
        }

        //---- Add Chat
        //-------------
        public void AddChat(string playerName, string chatMessage)
        {
            LobbyChatModel chatModel = new LobbyChatModel(playerName, chatMessage);
            Model.Chat.Enqueue(chatModel); // update model data
            UnityEngine.Debug.Log($"[{chatModel.DateTime.ToShortTimeString()}]{chatModel.DisplayName}:{chatModel.Chat}");            
        }

        //---- Send Messages
        //------------------
        #region Send NetMessages
        public void SendMessage(NetMessage netMessage, params string[] ids)
        {
            OutgoingMessage message = new OutgoingMessage()
            {
                ToSend = ids,
                NetMessage = netMessage
            };
            OutgoingMessages.Enqueue(message);
        }

        private void Send(string clientId, NetMessage netMessage)
        {
            DediLobbyPlayer player = GetPlayer(clientId);
            if(player == null)
            {
                return;
            }
            player.Connection.Send(netMessage);
        }

        private void BroadCast(NetMessage netMessage)
        {
            foreach(var player in Players)
            {
                player.Value.Connection.Send(netMessage);
            }
        }
        #endregion

        //---- Util
        //---------
        public DediLobbyPlayer GetPlayer(string id)
        {
            DediLobbyPlayer client;
            if(!Players.TryGetValue(id, out client))
            {
                UnityEngine.Debug.LogError($"Unable to find client:{id}");
            }
            return client;
        }
    }
} // end namespace
