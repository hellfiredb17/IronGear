using System;

namespace Hawkeye.Models
{
    public enum ConnectionStatus
    {
        Offline,
        Online
    }

    [Serializable]
    public class ConnectionState
    {
        //---- Variables
        //--------------
        public string NetworkToken; // given from server

        public string ClientId;
        public string DisplayName;
        public string ErrorMessage;
        public ConnectionStatus Status;
        // TODO - any other client information to be passed to server here

    } // end classes
} // end namespace
