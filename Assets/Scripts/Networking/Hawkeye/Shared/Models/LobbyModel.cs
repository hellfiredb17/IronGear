using System.Collections.Generic;

namespace Hawkeye.Models
{
    /// <summary>
    /// Data model of a lobby, information to be sent over network
    /// </summary>
    public class LobbyModel
    {
        //---- Variables
        //--------------
        public string LobbyID;
        public string LobbyName;
        public int MaxPlayers;
        public List<LobbyPlayerModel> Players;

        //---- Ctor
        //---------
        public LobbyModel(string id, string lobbyName, int maxPlayers)
        {
            LobbyID = id;
            LobbyName = lobbyName;
            MaxPlayers = maxPlayers;
            Players = new List<LobbyPlayerModel>();
        }

        //---- Getters
        //------------
        public LobbyPlayerModel GetLobbyPlayer(int id)
        {
            for(int i = 0; i < Players.Count; i++)
            {
                /*if(Players[i].NetId == id)
                {
                    return Players[i];
                }*/
            }
            return null;
        }

        //---- Add / Remove / Disconnect
        //------------------------------
        public bool PlayerJoin(LobbyPlayerModel player)
        {
            if(Players.Count == MaxPlayers)
            {
                return false;
            }
            player.State = LobbyPlayerModel.Status.Joined;
            Players.Add(player);
            return true;
        }

        public void PlayerLeave(LobbyPlayerModel player)
        {
            player.State = LobbyPlayerModel.Status.Left;
            Players.Remove(player);
        }

        public void PlayerLeave(int netId)
        {
            LobbyPlayerModel player = GetLobbyPlayer(netId);
            player.State = LobbyPlayerModel.Status.Left;
            Players.Remove(player);
        }

        public LobbyPlayerModel PlayerDisconnect(int netId)
        {
            LobbyPlayerModel player = GetLobbyPlayer(netId);
            player.State = LobbyPlayerModel.Status.Disconnected;
            Players.Remove(player);
            return player;
        }

        public void SetPlayerReady(int netid, bool isReady)
        {
            LobbyPlayerModel player = GetLobbyPlayer(netid);
            player.Ready = isReady;
        }
    }
}
