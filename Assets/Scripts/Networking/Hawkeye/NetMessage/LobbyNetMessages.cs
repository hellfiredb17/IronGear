using System.Collections.Generic;

namespace Hawkeye
{
    //-------------------------------------- Lobby NetMessages Sent to Server ---------------------------------------    
    //
    //---------------------------------------------------------------------------------------------------------------
    public class CreateLobby : NetMessage
    {
        //---- Variables
        //--------------
        public string LobbyName;
        public int MaxPlayers;
        public int HostId;

        //---- Ctor
        //---------
        public CreateLobby(string name, int host, int max)
        {
            LobbyName = name;
            HostId = host;
            MaxPlayers = max;
        }

        public override void Process(GameState gameState)
        {
            // Get server game state
            ServerGameState serverState = gameState as ServerGameState;

            // Creates a new lobby adds it to netobjects
            LobbyNetObject lobby = new LobbyNetObject(serverState.GetNetWorkId(), LobbyName, HostId);
            serverState.Add(lobby);

            serverState.Log($"Lobby {LobbyName} created");

            // send lobby state down to client
            serverState.Send(HostId, new SendLobbyState(new LobbyState(lobby.NetId, LobbyName, MaxPlayers, 0)));
        }
    }

    public class GetLobbyList : NetMessage
    {
        //---- Variables
        //--------------        
        public int PlayerId;

        //---- Ctor
        //---------
        public GetLobbyList(int playerId)
        {
            PlayerId = playerId;
        }

        public override void Process(GameState gameState)
        {            
            // Get server game state
            ServerGameState serverState = gameState as ServerGameState;
            // Get lobby list
            List<LobbyNetObject> lobbies = serverState.FindAllObjects<LobbyNetObject>();
            if(lobbies.Count == 0)
            {
                // Send there is no lobbies open
                serverState.Send(PlayerId, new SendLobbyList(null));
                return;
            }

            // Convert to lobby state
            List<LobbyState> lobbyStates = new List<LobbyState>(lobbies.Count);
            for(int i = 0; i < lobbies.Count; i++)
            {
                lobbyStates.Add(new LobbyState(lobbies[i].NetId, lobbies[i].Name, lobbies[i].MaxPlayers, lobbies[i].players.Count));
            }

            // Send list back to client
            serverState.Send(PlayerId, new SendLobbyList(lobbyStates));
        }
    }

    public class ConnectToLobby : NetMessage
    {
        //---- Variables
        //--------------
        string PlayerName;
        int LobbyId;
        int PlayerId;       
        
        //---- Ctor
        //---------
        public ConnectToLobby(int lobbyId, int playerId, string playerName)
        {
            PlayerName = playerName;
            LobbyId = lobbyId;
            PlayerId = playerId;
        }

        public override void Process(GameState gameState)
        {
            // Get server game state
            ServerGameState serverState = gameState as ServerGameState;
        }
    }

    public class SendLobbyChat : NetMessage
    {
        //---- Variables
        //--------------        
        public int LobbyId;
        public int PlayerId;
        public string ChatMessage;

        //---- Ctor
        //---------
        public SendLobbyChat(int lobby, int player, string chat)
        {
            LobbyId = lobby;
            PlayerId = player;
            ChatMessage = chat;
        }

        public override void Process(GameState gameState)
        {
            // Get server game state
            ServerGameState serverState = gameState as ServerGameState;
        }
    }

    //-------------------------------------- Lobby NetMessages Sent to Client ---------------------------------------    
    //
    //---------------------------------------------------------------------------------------------------------------
    public class SendLobbyList : NetMessage
    {
        //---- Variables
        //--------------        
        public List<LobbyState> Lobbies;

        //---- Ctor
        //---------
        public SendLobbyList(List<LobbyState> lobbies)
        {
            Lobbies = lobbies;
        }

        public override void Process(GameState gameState)
        {
            // TODO
        }
    }

    public class SendLobbyState : NetMessage
    {
        //---- Variables
        //--------------
        public LobbyState LobbyState;

        //---- Ctor
        //---------
        public SendLobbyState(LobbyState state)
        {
            LobbyState = state;
        }

        public override void Process(GameState gameState)
        {
            ClientGameState clientState = gameState as ClientGameState;
            clientState.Log($"TODO = LobbyState ID:{LobbyState.Id} Name:{LobbyState.Name} Max:{LobbyState.MaxPlayers} Current:{LobbyState.CurrentPlayers}");
        }
    }

}// end namespace
