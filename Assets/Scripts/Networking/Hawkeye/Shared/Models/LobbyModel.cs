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
        public string LobbyId;
        public string LobbyName;
        public int MaxPlayers;
        public List<LobbyPlayerModel> Players;
        public Queue<LobbyChatModel> Chat;

        //---- Ctor
        //---------
        public LobbyModel(string id, string lobbyName, int maxPlayers)
        {
            LobbyId = id;
            LobbyName = lobbyName;
            MaxPlayers = maxPlayers;
            Players = new List<LobbyPlayerModel>();
            Chat = new Queue<LobbyChatModel>();
        }
    }

    [System.Serializable]
    public class LobbyInfo
    {
        public string LobbyId;
        public string LobbyName;
        public int PlayerCount;
        public int MaxCount;

        //---- Ctor
        //---------
        public LobbyInfo(string id, string name, int count, int maxCount)
        {
            LobbyId = id;
            LobbyName = name;
            PlayerCount = count;
            MaxCount = maxCount;
        }
    }
}
