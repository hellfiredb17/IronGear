using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Hawkeye;

public class LobbyClientMenu : LobbyMenu
{
    //---- Variables
    //--------------
    [Header("Chat Input")]
    public Button sendButton;
    public TMP_InputField chatInput;
    public Text buttonText;

    private TCPClient client;

    //---- Menu Interface
    //-------------------
    public override void Init()
    {
        base.Init();
        sendButton.onClick.AddListener(OnSendChatPress);
    }

    //---- Lobby Interface
    //--------------------
    protected override void GetlobbyNetObject()
    {
        // Get client
        if(client == null)
        {
            client = TCPClient.Client;
        }

        // Get lobby net object which contains all the details
        lobby = client.gameState.FindObject<LobbyNetObject>();
        if (lobby == null)
        {
            client.gameState.LogError("Unable to find lobby net object");
        }
    }

    //---- Input Actions
    //------------------
    private void OnSendChatPress()
    {
        string text = chatInput.text;
        if(string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
        {
            client.gameState.Log("Text is null, doing nothing");
            return;
        }

        // Send chat to server        
        client.gameState.Send(new SendChat(client.gameState.NetId, text));
        // Clear text
        chatInput.text = string.Empty;
    }
}
