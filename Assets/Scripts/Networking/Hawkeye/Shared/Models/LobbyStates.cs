using System;
using System.Collections.Generic;

namespace Hawkeye.Models
{
    //---------- Lobby State ---------
    //--------------------------------
    [Serializable]
    public class LobbyState
    {
        //---- Variables
        //--------------
        public string Id;
        public string DisplayName;
        public int MaxPlayers;
        public List<LobbyPlayerState> Players;
        public Queue<LobbyChatState> Chat;

        //---- Ctor
        //---------
        public LobbyState(string id, string lobbyName, int maxPlayers)
        {
            Id = id;
            DisplayName = lobbyName;
            MaxPlayers = maxPlayers;
            Players = new List<LobbyPlayerState>();
            Chat = new Queue<LobbyChatState>();
        }
    }

    //---------- Lobby Player State ---------
    //---------------------------------------
    [Serializable]
    public class LobbyPlayerState
    {
        //---- Variables
        //--------------
        public string Id;
        public string DisplayName;
        public bool Ready;

        //---- Ctor
        //---------
        public LobbyPlayerState(string id, string name)
        {
            Id = id;
            DisplayName = name;
        }
    }

    //---------- Lobby Chat State ---------
    //-------------------------------------
    [Serializable]
    public class LobbyChatState
    {
        //---- Variables
        //--------------
        public Queue<ChatState> Chats;

        //---- Ctor
        //---------
        public LobbyChatState()
        {
            Chats = new Queue<ChatState>();
        }
    }

    [Serializable]
    public class ChatState
    {
        //---- Variables
        //--------------
        public string DisplayPlayer;
        public string Chat;
        public DateTime DateTime;

        //---- Ctor
        //---------
        public ChatState(string player, string chat)
        {
            DisplayPlayer = player;
            Chat = chat;
            this.DateTime = DateTime.Now;
        }
    }

    //---------- Lobby List State ---------
    //-------------------------------------
    [System.Serializable]
    public class LobbyListState
    {
        //---- Variables
        //--------------
        public List<LobbyBasicState> LobbyList;

        //---- Ctor
        //---------
        public LobbyListState()
        {
            LobbyList = new List<LobbyBasicState>();
        }
    }

    [Serializable]
    public class LobbyBasicState
    {
        //---- Variables
        //--------------
        public string Id;
        public string DisplayName;
        public int PlayerCount;
        public int MaxPlayers;

        //---- Ctor
        //---------
        public LobbyBasicState(string id, string name, int count, int max)
        {
            Id = id;
            DisplayName = name;
            PlayerCount = count;
            MaxPlayers = max;
        }

        public LobbyBasicState(LobbyState lobbyState)
        {
            Id = lobbyState.Id;
            DisplayName = lobbyState.DisplayName;
            PlayerCount = lobbyState.Players.Count;
            MaxPlayers = lobbyState.MaxPlayers;
        }
    }
}
