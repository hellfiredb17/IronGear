using Hawkeye.Models;

namespace Hawkeye
{
    public class DediLobbyPlayer
    {
        //---- Variables
        //--------------
        public ClientConnection Connection;
        public LobbyPlayerModel Model;

        //---- Ctor
        //---------
        public DediLobbyPlayer(ClientConnection clientConnection, string displayName)
        {
            Connection = clientConnection;
            Model = new LobbyPlayerModel(clientConnection.Id, displayName);
            Model.State = LobbyPlayerModel.Status.Joined;
        }
    }
} // end namespace
