using System;

namespace Hawkeye.Models
{    
    /// <summary>
    /// Class to hold information about a client + connection
    /// </summary>
    [Serializable]    
    public class ClientState
    {
        //---- Variables
        //--------------
        public string NetworkToken; // given from server
        
        // client information
        public string Id;
        public string DisplayName;       

    } // end classes
} // end namespace
