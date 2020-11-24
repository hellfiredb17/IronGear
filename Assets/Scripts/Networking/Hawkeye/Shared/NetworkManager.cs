using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    //---- Variables
    //--------------
    private static NetworkManager _instance;

    [Header("Server")]
    public TCPServer serverPrefab;

    [Header("Client")]
    public TCPClient clientPrefab;

    private TCPServer server;
    private TCPClient client;

    //---- Awake
    //----------
    private void Awake()
    {
        _instance = this;
    }

    //---- Server
    //-----------    
    public static TCPServer CreateServer()
    {
        if(_instance.server != null)
        {
            Debug.LogError("Server already created, cannot create two");
            return _instance.server;
        }
        _instance.server = Instantiate(_instance.serverPrefab);
        return _instance.server;
    }

    public static TCPServer GetServer()
    {
        return _instance.server;
    }

    public static void RemoveServer()
    {
        if(_instance.server == null)
        {
            return;
        }
        //_instance.server.Close();
        Destroy(_instance.server.gameObject);
    }

    //---- Client
    //-----------
    public static TCPClient CreateClient()
    {
        if (_instance.client != null)
        {
            Debug.LogError("Client already created, cannot create two");
            return _instance.client;
        }
        _instance.client = Instantiate(_instance.clientPrefab);
        return _instance.client;
    }

    public static TCPClient GetClient()
    {
        return _instance.client;
    }

    public static void RemoveClient()
    {
        if (_instance.client == null)
        {
            return;
        }
        /*_instance.client.Disconnect();*/
        Destroy(_instance.client.gameObject);
    }
}
