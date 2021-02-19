using Hawkeye.NetMessages;

namespace Hawkeye
{
    //---------- Dedi Listener ----------
    // Dedi side interface
    //-----------------------------------
    public interface IDediConnectionListener : INetworkListener
    {        
        void OnClientConnection(ClientConnection connection);
    }

    //---------- Client Listener ----------
    // Client side interface
    //-------------------------------------
    public interface IClientConnectionListener : INetworkListener
    {        
        void OnNetworkToken(NetworkToken token);
        void OnUpdateConnectionState(UpdateConnectionState state);
    }

} // end namespace
