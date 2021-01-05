using Hawkeye.Models;

namespace Hawkeye.NetMessages
{
	//---------- Collection of all Client Netmessage Responses ---------
	//------------------------------------------------------------------
	public class ResponseCreateClient : ResponseClientMessage
	{
		public string ConnectionId;

		public ResponseCreateClient(string netId)
		{
			ConnectionId = netId;
		}

		public override void Process(Client client)
		{
			client.NetworkId = ConnectionId;
		}
	}	

    #region Lobby Response Messages
    public class ResponseCreateLobby : ResponseClientMessage
	{
		public LobbyModel LobbyModel;

		public ResponseCreateLobby(LobbyModel model)
		{
			LobbyModel = model;
		}

		public override void Process(Client client)
		{
			client.CreateLobby(LobbyModel);
		}
	}

	public class ResponseLobbyList : ResponseClientMessage
    {
		public LobbyInfo[] LobbyInfo;

		public ResponseLobbyList(LobbyInfo[] info)
		{
			LobbyInfo = info;
		}

        public override void Process(Client client)
        {
			client.ResponseLobbyList(LobbyInfo);
        }
    }
    #endregion

} // end namespace
