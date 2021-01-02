using Hawkeye.Models;
using UnityEngine;

namespace Hawkeye.GameStates
{
    /// <summary>
    /// Client game state
    /// Includes a Lobby and Game logic
    /// </summary>    
    public class ClientGameState
    {
        //---- Variables
        //--------------        
        public LobbyModel Lobby;
        private Client.Client client;

        //---- Properties
        //---------------
        public Client.Client Client => client;

        //---- Ctor
        //---------
        public ClientGameState(Client.Client client)
        {
            this.client = client;
        }

        //---- Update
        //-----------
        public void Update(float dt)
        {
            if(client.IncomingMessages.Count > 0)
            {
                foreach(var message in client.IncomingMessages)
                {
                    message.Process(this);
                }
                client.IncomingMessages.Clear();
            }
        }

        //---- Interface
        //--------------
        #region Logging
        public void Log(string str)
        {
            Debug.Log(str);
        }

        public void Warn(string str)
        {
            Debug.LogWarning(str);
        }

        public void Error(string str)
        {
            Debug.LogError(str);
        }
        #endregion

        #region Lobby
        public LobbyModel GetLobby()
        {
            return Lobby;
        }

        public void SetLobby(LobbyModel model)
        {
            Lobby = model;
        }
        #endregion
    }
}
