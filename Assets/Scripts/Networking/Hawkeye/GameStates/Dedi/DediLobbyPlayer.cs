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
        }
    }
} // end namespace
