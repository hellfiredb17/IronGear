using UnityEngine;
using Hawkeye;
using Hawkeye.Server;

public class UnityServer : MonoBehaviour
{
    //---- Variables
    //--------------
    private Server server;

    //---- Awake / Start
    //------------------
    private void Awake()
    {
        server = new Server();
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
        if(Input.GetKeyDown(KeyCode.Q))
        {            
        }
    }
}
