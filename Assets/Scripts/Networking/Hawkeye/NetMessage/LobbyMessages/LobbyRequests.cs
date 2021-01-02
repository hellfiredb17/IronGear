using Hawkeye.GameStates;
using Hawkeye.Models;

namespace Hawkeye.NetMessages
{
    //---- All Requests (Client to Server) net messages for Lobby ----
    //----------------------------------------------------------------
    public class RequestLobby : RequestMessage
    {
        //---- Public
        //-----------
        public string LobbyName;
        public string ConnectionId;

        //---- Ctor
        //---------
        public RequestLobby(string connectionId, string lobbyName)
        {
            ConnectionId = connectionId;
            LobbyName = lobbyName;
        }

        //---- Interface
        //--------------
        public override void Process(IDediGameState gameState)
        {
            LobbyModel lobby = gameState.GetLobby(LobbyName);
            if(lobby == null)
            {
                // Create new lobby with ID
                lobby = gameState.CreateLobby(LobbyName);
            }

        }
    }
} //  end namespce
