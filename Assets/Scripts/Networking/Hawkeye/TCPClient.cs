using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Hawkeye.Client
{
    public class TCPClient
    {
        //---- Temp
        //---------
        public static int PORT = 4400;
        public static int DISCONNECT_TIMEOUT = 500;

        //---- Variables
        //--------------
        private TcpClient client;
        private IPAddress ipAddress;
        private NetworkStream stream;
        private Thread messageThread;

        //---- Ctor
        //---------
        public TCPClient()
        {            
        }

        //---- Connect
        //------------
        public async void Connect(IPAddress ipAddress, int port)
        {
            try
            {
                this.ipAddress = ipAddress;
                client = new TcpClient(AddressFamily.InterNetwork);
                await client.ConnectAsync(ipAddress, port);

                // seems we have a connection - check if we have a welcome message
                stream = client.GetStream();

                // start message thread
                messageThread = new Thread(MessageLoop);
                messageThread.Start();
            }
            catch(Exception ex)
            {
                Debug.LogError(ex.ToString());                
            }
        }

        private async void MessageLoop()
        {
            try
            {
                while (true)
                {   
                    if (stream.DataAvailable && stream.CanRead)
                    {
                        byte[] buffer = new byte[1024];                        
                        await stream.ReadAsync(buffer, 0, buffer.Length);
                        string message = Encoding.Default.GetString(buffer);
                        Debug.Log($"Message from server\n{message}");
                    }
                }
            }
            catch (ThreadAbortException abort)
            {
                Debug.LogWarning("Message thread abort\n" + abort.ToString());
            }
            catch (Exception ex)
            {
                Debug.LogError("Message thread exception\n" + ex.ToString());
            }
        }

        //---- Send
        //---------
        public async void Send(string message)
        {
            if (stream != null && stream.CanWrite)
            {                
                byte[] bytes = Encoding.Default.GetBytes(message);
                await stream.WriteAsync(bytes, 0, bytes.Length);
            }
        }

        //---- Disconnect
        //---------------
        public void Disconnect()
        {
            // TODO - send a disconnect message to server

            // stop message thread
            messageThread?.Abort();
            messageThread?.Join();

            // close the stream and connection
            stream?.Close(DISCONNECT_TIMEOUT);
            client?.Close();
        }

    } // end class   

} // end namespace
