
namespace Hawkeye.Models
{
    /// <summary>
    /// Lobby player data to be sent over network
    /// </summary>
    [System.Serializable]
    public class LobbyPlayerModel
    {
        //---- Variables
        //--------------        
        public string NetId;
        public string DisplayName;        
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
