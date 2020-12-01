using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Hawkeye;

public abstract class LobbyMenu : MenuBase
{
    //---- Variables
    //--------------
    [Header("Buttons")]
    public Button actionButton;    

    [Header("Title")]
    public TextMeshProUGUI title;

    [Header("Players")]
    public TextMeshProUGUI playerCount;
    public TextMeshProUGUI players;

    [Header("Chat")]
    public TextMeshProUGUI chat;    
    
    protected LobbyNetObject lobby;
    
    protected StringBuilder sbChat;
    protected StringBuilder sbPlayers;

    //---- Interface
    //--------------
    public override void Init()
    {
        sbChat = new StringBuilder();
        sbPlayers = new StringBuilder();
    }

    public override void Enter()
    {
        GetlobbyNetObject();
        UpdateLobbyName();
        UpdateUI();
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        lobby = null;
        sbChat.Clear();
        sbPlayers.Clear();       

        players.text = string.Empty;
        chat.text = string.Empty;
    }

    //---- Public
    //-----------
    public void AddChat(LobbyChatHistory chat)
    {

    }

    //---- Lobby Interface
    //--------------------
    protected abstract void GetlobbyNetObject();

    //---- UI Functions
    //-----------------
    public void UpdateUI()
    {
        UpdatePlayerList();
        UpdatePlayerCount();
        UpdateChat();
    }

    protected void UpdateLobbyName()
    {
        title.text = $"[LOBBY]: {lobby.Name}";
    }

    protected void UpdatePlayerCount()
    {
        playerCount.text = $"{lobby.players.Count}/{lobby.MaxPlayers}";
    }

    protected void UpdatePlayerList()
    {
        sbPlayers.Clear();
        foreach(var player in lobby.players)
        {
            string color = player.Value.Ready ? "green" : "red";
            sbPlayers.AppendLine($"<color={color}>{player.Value.Name}</color>");
        }
        players.text = sbPlayers.ToString();
    }

    protected void UpdateChat()
    {
        sbChat.Clear();
        for(int i = 0; i < lobby.chatHistory.Count; i++)
        {
            LobbyChatHistory chat = lobby.chatHistory[i];
            sbChat.AppendLine($"[{chat.PlayerName}]:{chat.Chat}");
        }
        chat.text = sbChat.ToString();
    }
}
