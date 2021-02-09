using System;
using Hawkeye.NetMessages;
using Hawkeye.Models;

public class ClientNetworkBridge : ILobbyClientListener
{
    //---- Events
    //-----------
    public Action<LobbyListState> OnLobbyListUpdate;
    public Action<LobbyState> OnLobbyStateUpdate;

    //---- Lobby Interface
    //--------------------
    #region Lobby Interface
    public void OnProcess(object netMessage, Type type)
    {
        if(type == typeof(UpdateLobbyState))
        {
            OnUpdateLobbyState(netMessage as UpdateLobbyState);
        }
        else if (type == typeof(ResponseLobbyList))
        {
            OnResponseLobbyList(netMessage as ResponseLobbyList);
        }
    }

    public void OnUpdateLobbyState(UpdateLobbyState updateLobbyState)
    {
        OnLobbyStateUpdate?.Invoke(updateLobbyState.State);
    }

    public void OnResponseLobbyList(ResponseLobbyList responseLobbyList)
    {
        OnLobbyListUpdate?.Invoke(responseLobbyList.LobbyList);
    }
    #endregion

    // TODO - add other netmessage interfaces here
}

