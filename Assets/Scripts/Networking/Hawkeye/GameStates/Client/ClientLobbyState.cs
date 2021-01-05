using System.Collections.Generic;
using Hawkeye.Models;
using Hawkeye.NetMessages;

namespace Hawkeye.GameStates
{
    public class ClientLobbyState
    {
        //---- Variables
        //--------------
        public Queue<ResponseLobbyMessage> IncomingMessages;
        public LobbyModel Model;
        // TODO - add view

        private Client client;
        private Queue<NetMessage> OutgoingMessages;
        private LobbyPlayerModel localPlayer;

        //---- Ctor
        //---------
        public ClientLobbyState(Client client, LobbyModel model)
        {
            this.client = client;
            Model = model;
            IncomingMessages = new Queue<ResponseLobbyMessage>();
            OutgoingMessages = new Queue<NetMessage>();

            // get the local player
            for(int i = 0; i < model.Players.Count; i++)
            {
                if(model.Players[i].NetId == client.NetworkId)
                {
                    localPlayer = model.Players[i];
                    break;
                }
            }
        }

        //---- Update
        //-----------
        public void FixedUpdate(float dt)
        {
            // Process incoming messages
            if(IncomingMessages.Count > 0)
            {
                while(IncomingMessages.Count > 0)
                {
                    IncomingMessages.Dequeue().Process(this);                    
                }
            }

            // Send any outgoing messages
            if(OutgoingMessages.Count > 0)
            {
                while(OutgoingMessages.Count > 0)
                {
                    client.Send(OutgoingMessages.Dequeue());
                }
            }
        }

        //---- Update State
        //-----------------
        public void UpdateState(LobbyModel updateModel)
        {
            // compare the current state and updated state and see what has changed            
            if(Model.LobbyName != updateModel.LobbyName)
            {
                UnityEngine.Debug.Log("Lobby name change");
                Model.LobbyName = updateModel.LobbyName;
            }

            if(Model.Players.Count != updateModel.Players.Count)
            {
                UnityEngine.Debug.Log("Lobby player count change");
                // TODO - figure out what has changed
            }

            /*if( (Model.Chat != null && updateModel.Chat != null) ||
                Model.Chat.Count != updateModel.Chat.Count)
            {
                UnityEngine.Debug.Log("Lobby chat count change");
            }*/
        }

        //---- User Actions
        //-----------------
        public void ToggleReady()
        {
            client.Send(new RequestLobbyToggleReady(Model.LobbyId, client.NetworkId));
        }

        public void SendChat(string chat)
        {
            client.Send(new RequestLobbyChat(Model.LobbyId, localPlayer.DisplayName, chat));
        }

    } // end class
} // end namespace
