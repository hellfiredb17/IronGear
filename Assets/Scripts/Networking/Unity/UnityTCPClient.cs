using UnityEngine;
using System.Net;

namespace Hawkeye.Unity
{
    public class UnityTCPClient : MonoBehaviour
    {
        //---- Variables
        //--------------
        [Header("IpAddress")]
        public string ipAddress;

        private Client.TCPClient tcpClient;

        //---- Awake
        //----------
        private void Awake()
        {
            tcpClient = new Client.TCPClient();
        }

        //---- Start
        //----------
        private void Start()
        {
            IPAddress iPAddress = IPAddress.Parse(ipAddress);
            tcpClient.Connect(iPAddress, Client.TCPClient.PORT);
        }

        //---- Destroy
        //------------
        private void OnDestroy()
        {
            tcpClient?.Disconnect();
        }

    } // end class
} // end namespace
