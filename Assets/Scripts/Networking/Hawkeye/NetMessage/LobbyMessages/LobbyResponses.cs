using Hawkeye.GameStates;
using Hawkeye.Models;

namespace Hawkeye.NetMessages
{
    //---- All Responses (Server to Client) net messages for Lobby ----
    //-----------------------------------------------------------------
    public class ResponseLobby : ResponseMessage
    {
        //---- Public
        //-----------
        public LobbyModel Lobby;

        //---- Ctor
        //---------
        public ResponseLobby(LobbyModel model)
        {
            Lobby = model;
        }

        //---- Interface
        //--------------
        public override void Process(ClientGameState gameState)
        {
            // Sets the client's lobby
            gameState.SetLobby(Lobby);
        }
    }
} // end namespace
