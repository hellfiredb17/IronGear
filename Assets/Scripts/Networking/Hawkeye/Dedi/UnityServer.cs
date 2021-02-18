using UnityEngine;
using Hawkeye;

public class UnityServer : MonoBehaviour
{
    //---- Variables
    //--------------
    private Server server;
    private UnityLogger logger;

    //---- Awake / Start
    //------------------
    private void Awake()
    {
        logger = new UnityLogger();
        server = new Server(logger);
    }

    private void Start()
    {
        server.Open(SharedConsts.LOCAL_IPADDRESS, SharedConsts.PORT);
    }

    //---- Update
    //-----------
    private void FixedUpdate()
    {
        server?.FixedUpdate(Time.fixedDeltaTime);
    }

    private void Update()
    {
        //KeyboardInput();
    }

    private void KeyboardInput()
    {   
    }
}
