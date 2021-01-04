using System.Collections.Generic;
using Hawkeye.Models;
using UnityEngine;

namespace Hawkeye.GameStates
{
    /// <summary>
    /// Game state for dedi.
    /// Game state includes a lobby and game logic
    /// </summary>
    public class DediGameState
    {
        //---- Variables
        //--------------
        public readonly string Id;
        public Dictionary<string, LobbyModel> Lobbies;

        //---- Ctor
        //---------
        public DediGameState(string id)
        {
            Id = id;
            Lobbies = new Dictionary<string, LobbyModel>();
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
        public LobbyModel CreateLobby(string loddyId)
        {
            return null;
        }

        public LobbyModel GetLobby(string lobbyId)
        {
            LobbyModel lobby;
            if(!Lobbies.TryGetValue(lobbyId, out lobby))
            {
                return null;
            }
            return lobby;
        }
        #endregion

        /*#region Messages
        public void Send(string connectionId, ResponseMessage message)
        {

        }

        public void BroadCast(ResponseMessage responseMessage)
        {

        }
        #endregion*/
    }
} // end namespace
