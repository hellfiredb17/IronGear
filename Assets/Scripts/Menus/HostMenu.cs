using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Net;
using Hawkeye;

public class HostMenu : MenuBase
{
    [Flags]
    private enum Error
    {
        None = 0,
        IPA = 1 << 0,
        Port = 1 << 1,
        Name = 1 << 2,
        Players = 1 << 3,
    }

    //---- Variables
    //--------------
    [Header("Inputs")]
    public TMP_InputField IpAddress;
    public TMP_InputField Port;
    public TMP_InputField LobbyName;    
    public TMP_InputField MaxPlayers;

    [Header("Button")]
    public Button CreateButton;

    TCPServer server;
    private Error errorState;    

    //---- Interface
    //--------------
    public override void Init()
    {
        IpAddress.onValueChanged.AddListener(ValidateIpAddress);
        Port.onValueChanged.AddListener(ValidatePort);
        LobbyName.onValueChanged.AddListener(ValidateName);
        MaxPlayers.onValueChanged.AddListener(ValidateMaxPlayers);

        CreateButton.onClick.AddListener(OnCreatePress);
    }

    public override void Enter()
    {
        if (server == null)
        {
            server = TCPServer.Server;
            if (server.gameState == null)
            {
                server.gameState = new ServerGameState();
            }
        }        

        IpAddress.text = SharedConsts.LOCAL_IPADDRESS;
        Port.text = SharedConsts.PORT.ToString();

        LobbyName.text = string.Empty;
        MaxPlayers.text = string.Empty;

        errorState = (Error.Name | Error.Players);        
        UpdateButtonState();
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    //---- Action Callbacks
    //---------------------
    private void OnOpenConnection()
    {
        CreateLobby();
    }

    private void CreateLobby()
    {
        // create lobby
        server.gameState.DirectMessage(new CreateLobby(LobbyName.text, int.Parse(MaxPlayers.text)));

        /*
        // setup lobby before entering lobby
        LobbyMenu lobby = menuManager.GetMenu<LobbyMenu>(MenuManager.State.Lobby);
        lobby.SetLobby(LobbyName.text);
        lobby.SetPlayerCount(0, int.Parse(MaxPlayers.text));
        lobby.SetHost(true);
        */

        // goto lobby
        menuManager.Show(MenuManager.State.Lobby);
    }

    //---- Actions
    //------------
    private void OnCreatePress()
    {
        CreateButton.gameObject.SetActive(false);
        
        // open connection
        server.gameState.Open(IpAddress.text, int.Parse(Port.text), OnOpenConnection);
    }

    private void ValidateName(string text)
    {
        if(string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
        {
            errorState |= Error.Name;
        }
        else
        {
            errorState &= ~Error.Name;
        }
        UpdateButtonState();
    }

    private void ValidateIpAddress(string text)
    {
        bool nonError = IPAddress.TryParse(text, out IPAddress address);
        if(nonError)
        {
            IpAddress.textComponent.color = Color.black;
            errorState &= ~Error.IPA;
        }
        else
        {
            IpAddress.textComponent.color = Color.red;
            errorState |= Error.IPA;
        }
        UpdateButtonState();
    }

    private void ValidatePort(string text)
    {
        if (!ValidateInt(text))
        {
            Port.textComponent.color = Color.red;
            errorState |= Error.Port;
        }
        else
        {
            Port.textComponent.color = Color.black;
            errorState &= ~Error.Port;
        }
        UpdateButtonState();
    }

    private void ValidateMaxPlayers(string text)
    {
        if(!ValidateInt(text))
        {
            MaxPlayers.textComponent.color = Color.red;
            errorState |= Error.Players;
        }
        else
        {
            MaxPlayers.textComponent.color = Color.black;
            errorState &= ~Error.Players;
        }
        UpdateButtonState();
    }

    private bool ValidateInt(string text)
    {
        return int.TryParse(text, out int test);
    }

    private void UpdateButtonState()
    {
        if(errorState == 0)
        {
            CreateButton.gameObject.SetActive(true);
        }
        else
        {
            CreateButton.gameObject.SetActive(false);
        }
    }
}
