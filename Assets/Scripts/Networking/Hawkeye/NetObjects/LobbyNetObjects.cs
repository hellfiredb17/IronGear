using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hawkeye
{
    //---------- Lobby Classes ----------
    //-----------------------------------
    [System.Serializable]
    public class LobbyNetObject : NetObject
    {
        public string Name;
        public int MaxPlayers;        
        public Dictionary<int, LobbyPlayer> players;
        public List<LobbyChatHistory> chatHistory;

        //---- Ctor
        //---------
        public LobbyNetObject(int id, string name, int maxPlayers) : base(id)
        {            
            Name = name;
            MaxPlayers = maxPlayers;

            players = new Dictionary<int, LobbyPlayer>();
            chatHistory = new List<LobbyChatHistory>();
        }

        public LobbyNetObject(int id, string name, int maxPlayers, List<LobbyPlayer> players, List<LobbyChatHistory> chat) : base(id)
        {
            Name = name;
            MaxPlayers = maxPlayers;
            this.players = new Dictionary<int, LobbyPlayer>(players.Count);
            for(int i = 0; i < players.Count; i++)
            {
                this.players.Add(players[i].Id, players[i]);
            }            
            chatHistory = chat;
        }

        //---- Player
        //-----------
        public List<LobbyPlayer> Players => new List<LobbyPlayer>(players.Values);
        public List<int> PlayerIds => new List<int>(players.Keys);

        public void PlayerJoin(LobbyPlayer player)
        {
            if (players.ContainsKey(player.Id))
            {
                Debug.LogError($"[Lobby]: Player {player.Id}:{player.Name} already entered");
                return;
            }
            players.Add(player.Id, player);
        }

        public void PlayerReady(int id, bool ready)
        {
            LobbyPlayer player;
            if (!players.TryGetValue(id, out player))
            {
                Debug.LogError($"[Lobby]: Player {id} not found");
                return;
            }
            player.Ready = ready;
        }

        public void PlayerExit(int id)
        {
            players.Remove(id);
        }

        //---- Chat
        //---------
        public void AddChat(int id, string chat)
        {
            LobbyPlayer player;
            if (!players.TryGetValue(id, out player))
            {
                Debug.LogError($"[Lobby]: Player {id} not found");
                return;
            }
            chatHistory.Add(new LobbyChatHistory(id, player.Name, chat));
        }
    }
    
    //------------------- Non-NetMessage Lobby Classes --------------------
    //---------------------------------------------------------------------
    [System.Serializable]
    public class LobbyState
    {        
        public string Name;
        public int Id;
        public int MaxPlayers;
        public int CurrentPlayers;

        public LobbyState(int id, string name, int max, int current)
        {
            Name = name;
            Id = id;
            MaxPlayers = max;
            CurrentPlayers = current;
        }
    }

    [System.Serializable]
    public class LobbyPlayer
    {
        public int Id;
        public string Name;
        public bool Ready;
        public LobbyPlayer(int id, string name, bool ready)
        {
            Id = id;
            Name = name;
            Ready = ready;
        }
    }

    [System.Serializable]
    public class LobbyChatHistory
    {
        public string PlayerName;
        public int PlayerId;
        public string Chat;

        public LobbyChatHistory(int id, string name, string chat)
        {
            PlayerName = name;
            PlayerId = id;
            Chat = chat;
        }
    }
} // end namespace
