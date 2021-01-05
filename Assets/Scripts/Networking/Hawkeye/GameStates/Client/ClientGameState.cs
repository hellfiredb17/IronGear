using Hawkeye.Models;
using UnityEngine;

namespace Hawkeye.GameStates
{
    /// <summary>
    /// Client game state    
    /// </summary>    
    public class ClientGameState
    {
        //---- Variables
        //--------------
        private Client client;

        //---- Properties
        //---------------
        public Client Client => client;

        //---- Ctor
        //---------
        public ClientGameState(Client client)
        {
            this.client = client;
        }

        //---- Update
        //-----------
        public void FixedUpdate(float dt)
        {   
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
    }
}
