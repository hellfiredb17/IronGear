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
        public int ConfirmId;

        //---- Ctor
        //---------
        public CreateLobby(string name, int max, int confirm = -1)
        {
            LobbyName = name;            
            MaxPlayers = max;
            ConfirmId = confirm;
        }

        public override void Process(GameState gameState)
        {
            // Get server game state
            ServerGameState serverState = gameState as ServerGameState;

            // Creates a new lobby adds it to netobjects
            LobbyNetObject lobby = new LobbyNetObject(serverState.GetNetWorkId(), LobbyName, MaxPlayers);
            serverState.Add(lobby);

            serverState.Log($"Lobby {LobbyName} created");

            if(ConfirmId > -1)
            {
                serverState.Send(ConfirmId, new SendLobbyState(
                    new LobbyState(lobby.NetId, lobby.Name, lobby.MaxPlayers, lobby.players.Count)));
            }
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
                lobbyStates.Add(new LobbyState(lobbies[i].NetId, 
                    lobbies[i].Name, 
                    lobbies[i].MaxPlayers, 
                    lobbies[i].players != null ? lobbies[i].players.Count : 0));
            }

            // Send list back to client
            serverState.Send(PlayerId, new SendLobbyList(lobbyStates));
        }
    }

    public class ConnectToLobby : NetMessage
    {
        //---- Variables
        //--------------
        public string PlayerName;
        public int LobbyId;
        public int PlayerId;       
        
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
            // Create lobby player
            LobbyPlayer player = new LobbyPlayer(PlayerId, PlayerName, false);
            // Get lobby then add player
            LobbyNetObject lobby = serverState.GetNetObject<LobbyNetObject>(LobbyId);
            if(lobby == null)
            {
                serverState.Log($"Unable to find Lobby:{LobbyId}");
                return;
            }
            lobby.PlayerJoin(player);

            // Update all connected players to lobby
            List<int> players = lobby.PlayerIds;
            for(int i = 0; i < players.Count; i++)
            {
                serverState.Send(players[i], new SendLobby(lobby));
            }
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
            ClientGameState clientState = gameState as ClientGameState;
            clientState.Log($"Lobby Count: {Lobbies.Count}");
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

            LobbyNetObject lobby = clientState.GetNetObject<LobbyNetObject>(LobbyState.Id);
            if(lobby == null)
            {
                lobby = new LobbyNetObject(LobbyState.Id, LobbyState.Name, LobbyState.MaxPlayers);
                lobby.MaxPlayers = LobbyState.MaxPlayers;
                clientState.Add(lobby);
            }
            clientState.Log($"LobbyState ID:{LobbyState.Id} Name:{LobbyState.Name} Max:{LobbyState.MaxPlayers} Current:{LobbyState.CurrentPlayers}");
        }
    }

    public class SendLobby : NetMessage
    {
        //---- Variables
        //--------------
        public string LobbyName;
        public List<LobbyPlayer> Players;
        public int LobbyId;
        public int MaxPlayers;
        public List<LobbyChatHistory> ChatHistory;

        //---- Ctor
        //---------
        public SendLobby(LobbyNetObject lobby)
        {
            LobbyId = lobby.NetId;
            LobbyName = lobby.Name;
            Players = lobby.Players;
            MaxPlayers = lobby.MaxPlayers;
            ChatHistory = lobby.chatHistory;
        }

        public override void Process(GameState gameState)
        {
            ClientGameState clientState = gameState as ClientGameState;
            clientState.Log($"Lobby ID:{LobbyId} Name:{LobbyName} Current Players:{(Players != null ? Players.Count : 0)}/{MaxPlayers}");
        }
    }

}// end namespace
