using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Hawkeye;

public class LobbyHostMenu : LobbyMenu
{
    //---- Variables
    //--------------
    private TCPServer server;

    //---- Menu Interface
    //-------------------
    public override void Init()
    {
        base.Init();
        actionButton.onClick.AddListener(OnStartPress);
    }

    //---- Lobby Interface
    //--------------------
    protected override void GetlobbyNetObject()
    {
        // Get server object
        if (server == null)
        {
            server = TCPServer.Server;
        }

        // Get lobby net object which contains all the details
        lobby = server.gameState.FindObject<LobbyNetObject>();
        if (lobby == null)
        {
            server.gameState.Log("Unable to find lobby net object");
        }
    }

    //---- Start Game Action
    //----------------------
    private void OnStartPress()
    {
        // TODO
    }
}
