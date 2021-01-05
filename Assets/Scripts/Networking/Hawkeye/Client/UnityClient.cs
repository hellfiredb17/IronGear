using UnityEngine;
using Hawkeye;
using Hawkeye.NetMessages;

public class UnityClient : MonoBehaviour
{
    //---- Variables
    //--------------
    [Header("Connection")]
    public string IpAddress = SharedConsts.LOCAL_IPADDRESS;
    public int Port = SharedConsts.PORT;

    [Header("Views")]
    public GameObject LobbyView;
    public GameObject GameView;

    private Client client;    

    //---- Awake
    //----------
    private void Awake()
    {
        client = new Client();        
    }

    //---- Updates
    //------------
    private void FixedUpdate()
    {
        client?.FixedUpdate(Time.fixedDeltaTime);        
    }

    private void Update()
    {        
        KeyboardInput();
    }

    private void KeyboardInput()
    {
        // TESTING SERVER WITHOUT UI

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
            client.Send(new RequestLobbyList(client.NetworkId));
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            client.lobbyState.ToggleReady();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            client.lobbyState.SendChat("Test chat function");
        }
    }
}
