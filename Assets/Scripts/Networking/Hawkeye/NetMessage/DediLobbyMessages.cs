using Hawkeye.GameStates;
using Hawkeye.Models;

namespace Hawkeye.NetMessages
{
    //---------- Collection of all Dedi Lobby Netmessage Requests ---------
    //---------------------------------------------------------------------
    public class RequestJoinLobby : RequestLobbyMessage
    {
        public string ConnectionId;        
        public string DisplayName;

        public RequestJoinLobby(string connectionId, string lobbyId, string displayName)
        {
            ConnectionId = connectionId;
            LobbyId = lobbyId;
            DisplayName = displayName;
        }

        public override void Process(DediLobbyState lobbyState)
        {            
            // TODO
        }
    }

    public class RequestLeaveLobby : RequestLobbyMessage
    {
        public string ConnectionId;        

        public RequestLeaveLobby(string connectionId, string lobbyId)
        {
            ConnectionId = connectionId;
            LobbyId = lobbyId;
        }
        public override void Process(DediLobbyState lobbyState)
        {
            // TODO
        }
    }

    public class RequestLobbyChat : RequestLobbyMessage
    {
        public string PlayerName;
        public string ChatMessage;

        public RequestLobbyChat(string lobbyId, string name, string chat)
        {
            LobbyId = lobbyId;
            PlayerName = name;
            ChatMessage = chat;
        }

        public override void Process(DediLobbyState lobbyState)
        {
            // Update chat log
            lobbyState.AddChat(PlayerName, ChatMessage);

            // Send lobby state to all connected clients
            lobbyState.SendMessage(new ResponseUpdateLobby(lobbyState.Model), null);
        }
    }

    public class RequestLobbyToggleReady : RequestLobbyMessage
    {
        public string ConnectionId;        

        public RequestLobbyToggleReady(string lobbyId, string connectionId)
        {
            LobbyId = lobbyId;
            ConnectionId = connectionId;            
        }

        public override void Process(DediLobbyState lobbyState)
        {
            // Update player
            lobbyState.ToggleReady(ConnectionId);

            // Send lobby state to all connected clients
            lobbyState.SendMessage(new ResponseUpdateLobby(lobbyState.Model), null);
        }
    }

} // end namespace
