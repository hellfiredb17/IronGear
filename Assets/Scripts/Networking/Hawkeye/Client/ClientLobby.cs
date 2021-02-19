using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hawkeye;
using Hawkeye.NetMessages;
using Hawkeye.Models;
using System;

public class ClientLobby : MonoBehaviour, ILobbyClientListener
{
    //---- Enum
    //---------
    private enum State
    {
        None,
        LobbyList,
        Lobby
    }

    //---- Variables
    //--------------
    [Header("Views")]
    public GameObject LobbyView;
    public GameObject LobbyListView;

    private ILog _log;
    private LobbyState _lobbyState;
    private LobbyListState _listLobbyState;
    private State _state;

    //---- Initialize
    //---------------
    public void Init(ILog log)
    {
        _log = log;
        _state = State.None;
        LobbyView.SetActive(false);
        LobbyListView.SetActive(false);
    }

    //---- State
    //----------
    private void SetState(State state)
    {
        // exit
        switch (_state)
        {            
            case State.LobbyList:
                _log.Output("Exit Lobby list");
                //LobbyListView.SetActive(false);
                break;
            case State.Lobby:
                _log.Output("Exit Lobby");
                //LobbyView.SetActive(false);
                break;
        }

        // enter
        switch (state)
        {
            case State.None:
                LobbyListView.SetActive(false);
                LobbyView.SetActive(false);                
                break;
            case State.LobbyList:
                _log.Output("Entering into Lobby list");
                //LobbyListView.SetActive(false);                
                break;
            case State.Lobby:
                _log.Output("Entering into Lobby");
                //LobbyView.SetActive(false);
                break;
        }
        _state = state;
    }

    //---- Interface
    //--------------
    #region Interface
    public void OnProcess(object netMessage, Type type)
    {
        if(type == typeof(ResponseLobbyList))
        {
            OnResponseLobbyList(netMessage as ResponseLobbyList);
        }
        else if (type == typeof(UpdateLobbyState))
        {
            OnUpdateLobbyState(netMessage as UpdateLobbyState);
        }
    }

    public void OnResponseLobbyList(ResponseLobbyList responseLobbyList)
    {
        throw new NotImplementedException();
    }

    public void OnUpdateLobbyState(UpdateLobbyState updateLobbyState)
    {
        if(_lobbyState == null)
        {
            _lobbyState = updateLobbyState.State;
            SetState(State.Lobby);
            return;
        }

        // TODO - compare and update view
        _lobbyState = updateLobbyState.State;
    }
    #endregion

}
