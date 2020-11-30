using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Hawkeye;

public class LobbyHostMenu : MenuBase
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

    private TCPServer server;
    private LobbyNetObject lobby;

    private StringBuilder sbChat;
    private StringBuilder sbPlayers;
    
    private int maxCount;
    private bool bHost;
    private bool bReady;

    //---- Interface
    //--------------
    public override void Init()
    {
        sbChat = new StringBuilder();
        sbPlayers = new StringBuilder();
        sendButton.onClick.AddListener(OnSendChat);
    }

    public override void Enter()
    {
        if(server == null)
        {
            server = TCPServer.Server;
        }

        lobby = server.gameState.FindObject<LobbyNetObject>();
        if(lobby == null)
        {
            Debug.LogError("Unable to find lobby net object");
        }
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

    //---- Update UI
    //--------------
    public void UpdateUI()
    {
        SetLobbyName(lobby.Name);
        SetPlayerCount(lobby.players.Count, lobby.MaxPlayers);
        UpdatePlayerList();
        UpdateChat();
    }
    
    private void UpdatePlayerList()
    {
        sbPlayers.Clear();
        foreach(var player in lobby.players)
        {
            string color = player.Value.Ready ? "green" : "red";
            sbPlayers.AppendLine($"<color={color}>{player.Value.Name}</color>");
        }
        players.text = sbPlayers.ToString();
    }

    private void UpdateChat()
    {
        sbChat.Clear();
        for(int i = 0; i < lobby.chatHistory.Count; i++)
        {
            LobbyChatHistory chat = lobby.chatHistory[i];
            sbChat.AppendLine($"[{chat.PlayerName}]:{chat.Chat}");
        }
        chat.text = sbChat.ToString();
    }

    //---- Setters
    //------------
    public void SetLobbyName(string lobby)
    {
        title.text = $"[LOBBY]: {lobby}";
    }

    public void SetPlayerCount(int current, int max)
    {
        maxCount = max;
        playerCount.text = $"{current}/{max}";
    }
}
