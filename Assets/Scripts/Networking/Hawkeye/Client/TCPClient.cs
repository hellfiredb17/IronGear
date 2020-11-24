using UnityEngine;
using Hawkeye;
using Hawkeye.Client;

public class TCPClient : MonoBehaviour
{
    //---- Variables
    //--------------
    ClientGameState gameState;

    //---- Update
    //-----------
    private void Update()
    {
        KeyboardInput();
    }

    private void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (gameState == null)
            {
                gameState = new ClientGameState();
            }
            // TODO - remove game state
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            // try to connect
            gameState.Connect(SharedConsts.LOCAL_IPADDRESS, SharedConsts.PORT);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            // try to connect
            NetObject connection = gameState.FindObject<NetObject>();
            gameState.Send(new CreateLobby("Editor lobby", connection.NetId, 4));
        }
    }
}
