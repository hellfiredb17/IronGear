using Hawkeye.Models;
using Hawkeye.GameStates;

namespace Hawkeye.NetMessages
{
    //---------- Collection of all Dedi Lobby Netmessage Requests ---------
    //---------------------------------------------------------------------
    public class ResponseUpdateLobby : ResponseLobbyMessage
    {        
        public LobbyModel LobbyState;

        public ResponseUpdateLobby(LobbyModel model)
        {
            LobbyState = model;
        }

        public override void Process(ClientLobbyState lobbyState)
        {
            // update the lobby state
            lobbyState.UpdateState(LobbyState);
        }
    }
} // end namespace