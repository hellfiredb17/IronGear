using Hawkeye.GameStates;

namespace Hawkeye.NetMessages
{
    //---------- Collection of all Dedi Netmessage Requests ---------
    //---------------------------------------------------------------
    #region Lobby Request Messages
    public class RequestCreateLobby : RequestDediMessage
	{
		public string ConnectionId;
		public string LobbyName;
		public int MaxPlayers;

		public RequestCreateLobby(string connectionId, string name, int maxPlayers)
		{
			ConnectionId = connectionId;
			LobbyName = name;
			MaxPlayers = maxPlayers;
		}

		public override void Process(Server server)
		{
			// builds lobby on server
			DediLobbyState lobbyState = server.CreateLobby(LobbyName, MaxPlayers, ConnectionId);

			// send lobby state to each connected client (this case just the host)
			lobbyState.SendMessage(new ResponseCreateLobby(lobbyState.Model), null);
		}
	}

	public class RequestLobbyList : RequestDediMessage
	{
		public string ConnectionId;
		
		public RequestLobbyList(string connectionId)
		{
			ConnectionId = connectionId;			
		}

		public override void Process(Server server)
		{
			server.Send(ConnectionId, new ResponseLobbyList(server.GetLobbyInfoList()));
		}
	}
	#endregion

} // end namespace
