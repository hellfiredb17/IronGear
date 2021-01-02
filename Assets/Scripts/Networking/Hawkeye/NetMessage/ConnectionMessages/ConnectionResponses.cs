using Hawkeye.GameStates;

namespace Hawkeye.NetMessages
{
    //---- All Responses (Server to Client) net messages for connection ----
    //----------------------------------------------------------------------
    public class ResponseConnection : ResponseMessage
    {
        //---- Public
        //-----------
        public string NetworkId;

        //---- Ctor
        //---------
        public ResponseConnection(string netId)
        {
            NetworkId = netId;
        }

        //---- Interface
        //--------------
        public override void Process(ClientGameState gameState)
        {
            // TODO
            UnityEngine.Debug.Log("Response back from connection!");

            gameState.Client.NetworkId = NetworkId;
        }
    }

    public class ResponseConnectionState : ResponseMessage
    {
        //---- Public
        //-----------
        public SharedEnums.ConnectionState Connection;

        //---- Ctor
        //---------
        public ResponseConnectionState(SharedEnums.ConnectionState state)
        {
            Connection = state;
        }

        //---- Interface
        //--------------
        public override void Process(ClientGameState gameState)
        {
            // TODO
        }
    }

} // end namespace
