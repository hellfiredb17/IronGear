using UnityEngine;
using Hawkeye;
using Hawkeye.Client;

public class TCPClient : MonoBehaviour
{
    //---- Static System
    //------------------
    public static TCPClient Client;

    //---- Variables
    //--------------
    public ClientGameState gameState;

    //---- Awake
    //----------
    private void Awake()
    {
        Client = this;
    }

    //---- Update
    //-----------
    private void Update()
    {
        KeyboardInput();
    }

    private void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (gameState == null)
            {
                gameState = new ClientGameState();
            }
            // TODO - remove game state
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // try to connect
            gameState.Connect(SharedConsts.LOCAL_IPADDRESS, SharedConsts.PORT);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // try to connect
            NetObject connection = gameState.FindObject<NetObject>();
            gameState.Send(new GetLobbyList(connection.NetId));
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // try to connect
            NetObject connection = gameState.FindObject<NetObject>();
            gameState.Send(new ConnectToLobby(1, connection.NetId, "Client"));
        }
    }
}
