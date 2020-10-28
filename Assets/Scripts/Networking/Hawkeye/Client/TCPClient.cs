using UnityEngine;
using Hawkeye.Client;

public class TCPClient : MonoBehaviour
{
    //---- Variables
    //--------------
    private Client client;

    //---- Awake
    //----------
    private void Awake()
    {
        client = new Client();
    }

    //---- Public
    //-----------
    public void Connect()
    {
        client.ConnectToServer();
    }
}
