using Hawkeye.NetMessages;
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
	
	public class ResponseCreateLobby : ResponseClientMessage
	{
		public LobbyModel LobbyModel;

		public ResponseCreateLobby(LobbyModel model)
		{
			LobbyModel = model;
		}

		public override void Process(Client client)
		{
			UnityEngine.Debug.Log($"Got create lobby message Name:{LobbyModel.LobbyName}");
		}
	}

} // end namespace
