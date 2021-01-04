using System.Collections.Generic;

namespace Hawkeye.Models
{
    /// <summary>
    /// Data model of a lobby, information to be sent over network
    /// </summary>
    [System.Serializable]
    public class LobbyModel
    {
        //---- Variables
        //--------------
        public string LobbyID;
        public string LobbyName;
        public int MaxPlayers;
        public List<LobbyPlayerModel> Players;
        public Queue<LobbyChatModel> Chat;

        //---- Ctor
        //---------
        public LobbyModel(string id, string lobbyName, int maxPlayers)
        {
            LobbyID = id;
            LobbyName = lobbyName;
            MaxPlayers = maxPlayers;
            Players = new List<LobbyPlayerModel>();
            Chat = new Queue<LobbyChatModel>();
        }
    }
}
