using UnityEngine;
using System.Net;

namespace Hawkeye.Unity
{
    public class UnityTCPServer : MonoBehaviour
    {
        //---- Variables
        //--------------
        [Header("IpAddress")]
        public string ipAddress;

        private Server.TCPServer tcpServer;

        //---- Awake
        //----------
        private void Awake()
        {
            tcpServer = new Server.TCPServer();
        }

        //---- Start
        //----------
        private void Start()
        {
            IPAddress iPAddress = IPAddress.Parse(ipAddress);
            tcpServer.Open(iPAddress, Server.TCPServer.Port);
        }

        //---- Destroy
        //------------
        private void OnDestroy()
        {
            tcpServer?.Close();
        }

    } // end class
} // end namespace
