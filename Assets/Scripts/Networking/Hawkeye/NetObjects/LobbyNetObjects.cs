using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hawkeye
{
    //---------- Lobby Classes ----------
    //-----------------------------------
    public class LobbyNetObject : NetObject
    {
        public string Name;
        public int MaxPlayers;
        public int HostId;
        public Dictionary<int, LobbyPlayerNetObject> players;
        public List<LobbyChatHistory> chatHistory;

        //---- Ctor
        //---------
        public LobbyNetObject(int id, string name, int host) : base(id)
        {            
            Name = name;
            HostId = host;
        }

        //---- Player
        //-----------
        public void PlayerJoin(LobbyPlayerNetObject player)
        {
            if (players.ContainsKey(player.NetId))
            {
                Debug.LogError($"[Lobby]: Player {player.NetId}:{player.Name} already entered");
                return;
            }
            players.Add(player.NetId, player);
        }

        public void PlayerReady(int id, bool ready)
        {
            LobbyPlayerNetObject player;
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
            LobbyPlayerNetObject player;
            if (!players.TryGetValue(id, out player))
            {
                Debug.LogError($"[Lobby]: Player {id} not found");
                return;
            }
            chatHistory.Add(new LobbyChatHistory(id, player.Name, chat));
        }
    }

    public class LobbyPlayerNetObject : NetObject
    {
        public string Name;
        public bool Ready;

        public LobbyPlayerNetObject(int id, string name, bool ready) : base(id)
        {            
            Name = name;
            Ready = ready;
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
