using System.Collections.Generic;

namespace Hawkeye.Models
{
    /// <summary>
    /// Lobby player data to be sent over network
    /// </summary>
    public class LobbyPlayerModel
    {
        //---- Status
        //-----------
        public enum Status
        {
            Disconnected,
            Joined,
            Left
        }

        //---- Variables
        //--------------
        public string Id;
        public string Name;
        public Status State;
        public Queue<LobbyChatModel> Chats;
        public bool Ready;

        //---- Ctor
        //---------
        public LobbyPlayerModel(string netId, string name)
        {
            Id = netId;
            Name = name;
            State = Status.Disconnected;
            Chats = new Queue<LobbyChatModel>();
        }

        //---- Chat
        //---------
        public void AddChat(LobbyChatModel chatModel)
        {
            Chats.Enqueue(chatModel);
        }
    } // end class
}// end namespace
