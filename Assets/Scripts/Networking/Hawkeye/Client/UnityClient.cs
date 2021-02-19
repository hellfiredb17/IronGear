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

    private UnityLogger log;
    private ClientConnection connection;
    private ClientConnectionInterface connectionInterface;

    //---- Awake
    //----------
    private void Awake()
    {        
        log = new UnityLogger();        
        connection = new ClientConnection(log);
        connectionInterface = new ClientConnectionInterface(connection, log);
        connection.ConnectionListener = connectionInterface;
    }

    //---- Updates
    //------------
    private void FixedUpdate()
    {        
    }

    private void Update()
    {        
        KeyboardInput();
    }

    private void KeyboardInput()
    {
        // KEYBOARD TESTING CONNECTION
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            connection.Connect(IpAddress, Port);
        }

        // LOBBY FUNCTIONS
        if (Input.GetKeyDown(KeyCode.Alpha2)) // create + join
        {
            connection.Send(new CreateLobby("123", 4));
            connection.Send(new JoinLobby("123", "All The Tea"));
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) // leave
        {
            connection.Send(new LeaveLobby("123"));
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) // ready
        {
            connection.Send(new PlayerReady("123", true));
        }

        if (Input.GetKeyDown(KeyCode.Alpha5)) // chat
        {
            connection.Send(new PlayerChat("123", "This is a test chat - Hello"));
        }

        // LOBBY LISTING
        if (Input.GetKeyDown(KeyCode.Alpha5)) // get lobby list
        {
            connection.Send(new RequestLobbyList());
        }
    }
}
