using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyMenu : MenuBase
{
    //---- Variables
    //--------------
    [Header("Buttons")]
    public Button actionButton;
    public Button sendButton;

    [Header("Input")]
    public TMP_InputField chatInput;

    [Header("Labels")]
    public TextMeshProUGUI title;
    public TextMeshProUGUI playerCount;
    public TextMeshProUGUI players;
    public TextMeshProUGUI chat;
    public Text buttonText;

    private StringBuilder sbChat;
    private StringBuilder sbPlayers;
    private Dictionary<string, bool> playerList;
    private Queue<string> chatQueue;
    
    private int maxCount;
    private bool bHost;
    private bool bReady;

    //---- Interface
    //--------------
    public override void Init()
    {
        sbChat = new StringBuilder();
        sbPlayers = new StringBuilder();
        playerList = new Dictionary<string, bool>();
        chatQueue = new Queue<string>();

        sendButton.onClick.AddListener(OnSendChat);
    }

    public override void Exit()
    {
        base.Exit();
        sbChat.Clear();
        sbPlayers.Clear();
        playerList.Clear();
        chatQueue.Clear();

        players.text = string.Empty;
        chat.text = string.Empty;
    }

    //---- Event Actions
    //------------------
    private void OnStartGame()
    {        
    }

    private void OnToggleReady()
    {
        bReady = !bReady;        
        buttonText.text = bReady ? "UNREADY" : "READY";
    }

    private void OnSendChat()
    {
        string chat = chatInput.text;
        chatInput.text = string.Empty;        
    }

    //---- Setters
    //------------
    public void SetLobby(string lobby)
    {
        title.text = $"[LOBBY]: {lobby}";
    }

    public void SetPlayerCount(int current, int max)
    {
        maxCount = max;
        playerCount.text = $"{current}/{max}";
    }

    public void SetHost(bool value)
    {
        bHost = value;
        if(bHost)
        {            
            actionButton.gameObject.SetActive(false);
        }
        else
        {
            actionButton.gameObject.SetActive(true);
        }
    }

    //---- Chat
    //---------
    public void AddChat(string player, string chat)
    {
        /*string color = localPlayer == player ? "green" : "red";
        string line = $"<color={color}>[{player}]: {chat}</color>";
        chatQueue.Enqueue(line);
        UpdateChat();*/
    }

    private void UpdateChat()
    {
        sbChat.Clear();
        foreach(var chat in chatQueue)
        {
            sbChat.AppendLine(chat);
        }
        chat.text = sbChat.ToString();
    }

    //---- Player
    //-----------
    public void AddPlayer(string player)
    {
        if(playerList.ContainsKey(player))
        {
            return;
        }
        playerList.Add(player, false);
        UpdatePlayerList();
    }

    public void RemovePlayer(string player)
    {
        playerList.Remove(player);
        UpdatePlayerList();
    }

    public void TogglePlayerReady(string player)
    {
        playerList[player] = !playerList[player];
        UpdatePlayerList();
    }

    private void UpdatePlayerList()
    {
        sbPlayers.Clear();
        foreach(var player in playerList)
        {
            string color = player.Value ? "green" : "yellow";
            sbPlayers.AppendLine($"<color={color}>{player.Key}</color>");
        }
        players.text = sbPlayers.ToString();
    }
}
