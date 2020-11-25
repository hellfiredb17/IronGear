using UnityEngine;
using Hawkeye;

public class TCPServer : MonoBehaviour
{
    //---- Static System
    //------------------
    public static TCPServer Server;

    //---- Variables
    //--------------
    public ServerGameState gameState;

    //---- Awake
    //----------
    private void Awake()
    {
        Server = this;
    }

    //---- Update
    //-----------
    private void Update()
    {
        //KeyboardInput();
    }

    private void KeyboardInput()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(gameState == null)
            {
                gameState = new ServerGameState();                
            }
            // TODO - remove game state
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            // open for connections
            gameState.Open(SharedConsts.LOCAL_IPADDRESS, SharedConsts.PORT);            
        }
    }
}
