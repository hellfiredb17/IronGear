using System.Collections.Generic;
using Hawkeye.Models;
using Hawkeye.NetMessages;
using UnityEngine;

namespace Hawkeye.GameStates
{
    /// <summary>
    /// Interface for all DediGameStates
    /// This is used for processing NetMessages
    /// </summary>
    public interface IDediGameState
    {
        // Logging
        void Log(string str);
        void Warn(string str);
        void Error(string str);

        // Lobby
        LobbyModel CreateLobby(string id);
        LobbyModel GetLobby(string id);

        // Messages
        /*void Send(string connectionId, ResponseMessage responseMessage);
        void BroadCast(ResponseMessage responseMessage);*/
    }

    /// <summary>
    /// Game state for dedi.
    /// Game state includes a lobby and game logic
    /// </summary>
    public class DediGameState : IDediGameState
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
