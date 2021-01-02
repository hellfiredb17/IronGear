using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Hawkeye;
using Hawkeye.Models;

public class ClientMenu : MenuBase
{
    //---- Variables
    //--------------
    [Header("Title")]
    public TextMeshProUGUI titleLabel;

    [Header("Network")]
    public TMP_InputField ipaddressInput;
    public TMP_InputField portInput;

    [Header("Player")]
    public TMP_InputField playerInput;

    [Header("Button")]
    public Button connectButton;
    public Text buttonText;

    private UnityClient client;
    private int playerId;
    private List<LobbyModel> lobbies;
    private bool bConnected;

    //---- Interface
    //--------------
    public override void Enter()
    {
        if(client == null)
        {
            //client = UnityClient.Client;
            /*if(client.gameState == null)
            {
                //client.gameState = new ClientGameState();
            }*/
        }

        playerId = -1;
        lobbies = null;
        bConnected = false;

        titleLabel.text = "Client";
        buttonText.text = "Connect";

        ipaddressInput.text = SharedConsts.LOCAL_IPADDRESS;
        portInput.text = SharedConsts.PORT.ToString();

        connectButton.onClick.AddListener(OnConnectPress);

        ipaddressInput.interactable = true;
        portInput.interactable = true;
        playerInput.interactable = false;
        connectButton.interactable = true;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        lobbies = null;

        ipaddressInput.text = string.Empty;
        portInput.text = string.Empty;
        playerInput.text = string.Empty;
        connectButton.onClick.RemoveAllListeners();
    }

    //---- Update
    //-----------
    private void Update()
    {
        if(!bConnected)
        {
            return;
        }

        if(playerId == -1)
        {
            if(GetPlayerID())
            {
                titleLabel.text = $"Client[{playerId}]";
                GetLobbyList();
            }            
        }
    }

    //---- Button Input
    //-----------------
    private void OnConnectPress()
    {
        if(ValidateIpaddress() && ValidatePort())
        {
            //client.gameState.Connect(ipaddressInput.text, int.Parse(portInput.text), OnConnectionEstablished);

            // turn off all ui
            ipaddressInput.interactable = false;
            portInput.interactable = false;
            playerInput.interactable = false;
            connectButton.interactable = false;

            return;
        }
        //client.gameState.Log("One or more validations failed");
    }

    private bool ValidateIpaddress()
    {
        return System.Net.IPAddress.TryParse(ipaddressInput.text, out System.Net.IPAddress ip);
    }

    private bool ValidatePort()
    {
        return int.TryParse(portInput.text, out int v);
    }

    private bool ValidateName()
    {
        return !string.IsNullOrEmpty(playerInput.text) && !string.IsNullOrWhiteSpace(playerInput.text);
    }

    //---- Connection Established
    //---------------------------
    private void OnConnectionEstablished()
    {
        titleLabel.text = "Client - Connected";
        buttonText.text = "Join";

        // set up click action
        connectButton.onClick.RemoveAllListeners();

        // turn on/off ui        
        playerInput.interactable = true;
        bConnected = true;
    }

    //---- Get Player ID
    //------------------
    private bool GetPlayerID()
    {
        /*NetObject netObject = client.gameState.FindObject<NetObject>();
        if(netObject == null)
        {
            return false;
        }
        playerId = netObject.NetId;*/
        return true;
    }

    //---- Get Lobby Count
    //--------------------
    public void LobbyLists(List<LobbyModel> lobbies)
    {
        this.lobbies = lobbies;

        connectButton.onClick.RemoveAllListeners();
        if (lobbies.Count == 0)
        {
            //client.gameState.Log("No open lobbies found");
            buttonText.text = "Refresh";
            connectButton.onClick.AddListener(GetLobbyList);                        
        }
        else
        {
            buttonText.text = "Join";
            connectButton.onClick.AddListener(JoinLobby);
        }
        connectButton.interactable = true;
    }

    private void GetLobbyList()
    {
        //client.gameState.Send(new GetLobbyList(playerId));
    }

    //---- Join Lobby
    //---------------
    private void JoinLobby()
    {
        if(!ValidateName())
        {
            return;
        }

        connectButton.onClick.RemoveAllListeners();
        connectButton.interactable = false;
        buttonText.text = "Joining...";

        //client.gameState.Send(new ConnectToLobby(lobbies[0].Id, playerId, playerInput.text));
    }
}
