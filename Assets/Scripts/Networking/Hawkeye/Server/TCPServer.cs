using UnityEngine;
using Hawkeye.Server;
using Hawkeye;

public class TCPServer : MonoBehaviour
{
    //---- Variables
    //--------------
    private Server server;
    private int gameTime;
    private float fTime;

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

    //---- Update
    //-----------
    private void Update()
    {
        if(!server.Connected)
        {
            return;
        }

        fTime += Time.deltaTime;

        // For testing lets send a message every second to connected clients
        int t = Mathf.FloorToInt(fTime);
        if(gameTime != t)
        {
            gameTime = t;
            server.Broadcast(new GameTime(gameTime));
        }
    }
}
