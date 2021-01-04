using UnityEngine;
using Hawkeye;
using Hawkeye.GameStates;
using Hawkeye.NetMessages;

public class UnityClient : MonoBehaviour
{
    //---- Variables
    //--------------
    [Header("Connection")]
    public string IpAddress = SharedConsts.LOCAL_IPADDRESS;
    public int Port = SharedConsts.PORT;

    private Client client;
    private ClientGameState gameState;

    //---- Awake
    //----------
    private void Awake()
    {
        client = new Client();
        gameState = new ClientGameState(client);
    }

    //---- Update
    //-----------
    private void Update()
    {
        gameState.Update(Time.deltaTime);
        KeyboardInput();
    }

    private void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            client.Connect(IpAddress, Port);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            client.Send(new RequestCreateLobby(client.NetworkId, "123", 4));
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {            
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {            
        }
    }
}
