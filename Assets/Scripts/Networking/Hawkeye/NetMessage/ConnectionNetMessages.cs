using UnityEngine;

namespace Hawkeye
{
    //-------------------------------------- Connection NetMessages Sent to Server ---------------------------------------    
    //
    //--------------------------------------------------------------------------------------------------------------------


    //-------------------------------------- Connection NetMessages Sent to Client ---------------------------------------    
    //
    //--------------------------------------------------------------------------------------------------------------------
    public class ConnectionEstablished : NetMessage
    {
        //---- Variables
        //--------------
        public int NetId;

        //---- Ctor
        //---------
        public ConnectionEstablished(int id)
        {
            NetId = id;
        }

        public override void Process(GameState gameState)
        {
            // Create net object with id, and add to game state
            ClientGameState clientState = gameState as ClientGameState;
            clientState.Log($"Connection Established NetID:{NetId}");

            NetObject netObject = new NetObject(NetId);
            clientState.Add(netObject);
        }
    }

} // end namespace
