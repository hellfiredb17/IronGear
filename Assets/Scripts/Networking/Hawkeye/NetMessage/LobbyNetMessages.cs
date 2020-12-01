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

            // Send lobby information to player that joined
            serverState.Send(PlayerId, new SendLobby(lobby));

            // Update all connected players that new player joined
            List<int> players = lobby.PlayerIds;
            for(int i = 0; i < players.Count; i++)
            {
                if(players[i] == PlayerId)
                {
                    continue;
                }
                serverState.Send(players[i], new UpdateLobby());
            }
        }
    }

    public class SendChat : NetMessage
    {
        //---- Variables
        //--------------        
        public int PlayerID;
        public string Text;

        //---- Ctor
        //---------
        public SendChat(int id, string text)
        {
            PlayerID = id;
            Text = text;
        }

        public override void Process(GameState gameState)
        {
            // Get server game state
            ServerGameState serverState = gameState as ServerGameState;

            if(serverState.CurrentState != GameState.State.Lobby)
            {
                serverState.Log("Getting lobby messages when not in lobby state");
                return;
            }

            // Get lobby net object
            LobbyNetObject lobbyNetObject = serverState.FindObject<LobbyNetObject>();
            lobbyNetObject.AddChat(PlayerID, Text);

            // Update the UI to show new message
            LobbyHostMenu lobby = serverState.Menus.GetMenu<LobbyHostMenu>(MenuManager.Menu.LobbyHost);
            lobby.UpdateUI();

            // TODO - send all players connected to update UI
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
            if(clientState.Menus.Current == MenuManager.Menu.ClientSetup)
            {
                ClientMenu menu = clientState.Menus.GetMenu<ClientMenu>(MenuManager.Menu.ClientSetup);
                menu.LobbyLists(Lobbies);
            }
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

            // Add lobby to net objects
            LobbyNetObject lobby = new LobbyNetObject(LobbyId, LobbyName, MaxPlayers, Players, ChatHistory);
            clientState.Add(lobby);

            // Goto lobby menu
            clientState.Menus.Show(MenuManager.Menu.LobbyClient);
        }
    }

    public class UpdateLobby : NetMessage
    {
        public override void Process(GameState gameState)
        {
            // TODO
        }
    }

}// end namespace
