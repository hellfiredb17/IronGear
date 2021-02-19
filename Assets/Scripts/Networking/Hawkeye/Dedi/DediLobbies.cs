using System.Collections.Generic;
using Hawkeye.NetMessages;
using Hawkeye.Models;
using System;

namespace Hawkeye
{
    /// <summary>
    /// Class that handles all lobbies for each connection
    /// Uses the ILobby interface
    /// </summary>
    public class DediLobbies : ILobbyDediListener
    {
        //---- Variables
        //--------------
        private Server _server;
        private ILog _log;
        private Dictionary<string, LobbyState> _lobbies;

        //---- Ctor
        //---------
        public DediLobbies(Server server, ILog log)
        {
            _server = server;
            _log = log;
            _lobbies = new Dictionary<string, LobbyState>();
        }

        //---- Public
        //-----------
        public void FixedUpdate(float dt)
        {
            // TODO
        }

        //---- Lobby Interface
        //--------------------
        #region Interface
        public void OnProcess(object netMessage, Type type)
        {
            // Parse the message into a lobby message
            if(type == typeof(CreateLobby))
            {
                OnCreateLobby(netMessage as CreateLobby);
            }
            else if (type == typeof(JoinLobby))
            {
                OnJoinLobby(netMessage as JoinLobby);
            }         
            
            // TODO - other client to dedi messages
        }

        public void OnRequestLobbyList(ResponseLobbyList responseLobbyList)
        {
            throw new NotImplementedException();
        }

        public void OnCreateLobby(CreateLobby createLobby)
        {
            // create new lobby and add to list
            string id = Guid.NewGuid().ToString();
            LobbyState lobbyState = new LobbyState(id, createLobby.DisplayName, createLobby.MaxPlayers);
            _lobbies.Add(id, lobbyState);
            _log.Output($"Lobby Created Id:[{id}] Name:[{createLobby.DisplayName}]");
        }

        public void OnJoinLobby(JoinLobby joinLobby)
        {
            _log.Output($"Player [{joinLobby.ClientId}] joining lobby [{joinLobby.LobbyId}]");
            LobbyState state = GetLobbyById(joinLobby.LobbyId);
            if(state == null)
            {
                // try geting lobby by name, will return the first match
                state = GetLobbyByName(joinLobby.LobbyId);
                if (state == null)
                {
                    // TODO - need to send feed back to player that cannot connect
                    return;
                }
            }

            // Add player if not already added
            if(!state.ContainsPlayerId(joinLobby.ClientId))
            {
                state.AddPlayer(joinLobby.ClientId, joinLobby.PlayerDisplayName);
            }

            // send all players connected to this lobby a lobby update
            Broadcast(state, new UpdateLobbyState(state));
        }

        public void OnLeaveLobby(LeaveLobby leaveLobby)
        {
            throw new NotImplementedException();
        }

        public void OnPlayerChat(PlayerChat playerChat)
        {
            throw new NotImplementedException();
        }

        public void OnPlayerReady(PlayerReady playerReady)
        {
            throw new NotImplementedException();
        }
        #endregion

        //---- Private
        //------------
        private LobbyState GetLobbyById(string lobbyId)
        {
            if(_lobbies.TryGetValue(lobbyId, out LobbyState state))
            {
                return state;
            }
            _log.Warn($"Unable to find lobby by id [{lobbyId}]");
            return null;
        }

        private LobbyState GetLobbyByName(string lobbyName)
        {
            foreach(var lobby in _lobbies.Values)
            {
                if(lobby.DisplayName == lobbyName)
                {
                    return lobby;
                }
            }
            _log.Warn($"Unable to find lobby by name [{lobbyName}]");
            return null;
        }

        private void Broadcast(LobbyState state, DediToClientMessage netMessage)
        {
            _log.Output($"Lobby:[{state.Id}] [{state.DisplayName}]\nPlayers: {state.Players.Count}/{state.MaxPlayers}");
            for (int i = 0; i < state.Players.Count; i++)
            {
                _server.Send(state.Players[i].Id, netMessage);
            }            
        }

    } // end class
} // end namespace
