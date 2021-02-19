using System;
using Hawkeye;
using Hawkeye.NetMessages;
using Hawkeye.Models;
using UnityEngine;

public class NetworkBridge : MonoBehaviour
{
    //---- Variables
    //--------------
    [HideInInspector] public ClientConnection Connection;
    [HideInInspector] public ClientState State;
    public ClientLobby Lobby;

    private UnityLogger _log;        
    private ClientConnectionListener _connectionListener;

    //---- Ctor
    //---------
    public void Init()
    {
        _log = new UnityLogger();
        LoadClientState();
        Connection = new ClientConnection(State, _log);
        _connectionListener = new ClientConnectionListener(State, Connection, _log);
        Lobby.Init(_log);

        // Hook up listeners
        Connection.ConnectionListener = _connectionListener;
        Connection.LobbyListener = Lobby;
    }

    private void LoadClientState()
    {
        State = new ClientState()
        {
            Id = "DEBUG",
            DisplayName = "DK"
        };
    }
}

