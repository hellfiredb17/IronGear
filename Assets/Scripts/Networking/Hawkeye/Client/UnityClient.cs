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

    [Header("NetworkBridge")]
    public NetworkBridge NetworkBridge;

    //---- Awake
    //----------
    private void Awake()
    {
        NetworkBridge.Init();
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
            NetworkBridge.Connection.Connect(IpAddress, Port);
        }

        // LOBBY FUNCTIONS
        if (Input.GetKeyDown(KeyCode.Alpha2)) // create + join
        {
            NetworkBridge.Connection.Send(new CreateLobby("123", 4));
            NetworkBridge.Connection.Send(new JoinLobby("123", NetworkBridge.State.DisplayName));
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) // leave
        {
            NetworkBridge.Connection.Send(new LeaveLobby("123"));
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) // ready
        {
            NetworkBridge.Connection.Send(new PlayerReady("123", true));
        }

        if (Input.GetKeyDown(KeyCode.Alpha5)) // chat
        {
            NetworkBridge.Connection.Send(new PlayerChat("123", "This is a test chat - Hello"));
        }

        // LOBBY LISTING
        if (Input.GetKeyDown(KeyCode.Alpha5)) // get lobby list
        {
            NetworkBridge.Connection.Send(new RequestLobbyList());
        }
    }
}
