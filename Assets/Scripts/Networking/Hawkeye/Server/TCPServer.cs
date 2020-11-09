using UnityEngine;
using Hawkeye.Server;
using Hawkeye;

public class TCPServer : MonoBehaviour
{
    //---- Variables
    //--------------
    private Server server;

    //---- Awake
    //----------
    private void Awake()
    {
        server = new Server();
    }

    //---- Start
    //----------
    private void Start()
    {
        server.Open(SharedConsts.LOCAL_IPADDRESS, SharedConsts.PORT);
    }
}
