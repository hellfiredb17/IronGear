using Hawkeye.GameStates;
using Hawkeye.Models;

namespace Hawkeye.NetMessages
{
    //---- All Responses (Server to Client) net messages for Lobby ----
    //-----------------------------------------------------------------
    public class ResponseLobby : ResponseClientMessage
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
        public override void Process(Client client)
        {
            // TODO
            UnityEngine.Debug.Log("Client got message - Response Lobby");
        }
    }
} // end namespace
