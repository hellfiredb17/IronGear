
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
        public string NetId;
        public string DisplayName;
        public Status State;        
        public bool Ready;

        //---- Ctor
        //---------
        public LobbyPlayerModel(string id, string displayName)
        {
            NetId = id;
            DisplayName = displayName;
        }
    } // end class
}// end namespace
