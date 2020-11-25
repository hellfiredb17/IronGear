using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Hawkeye
{
    /// <summary>
    /// Base data to be sent over network
    /// Writes data into byte array that is sent over network connection
    /// </summary>
    public class NetworkPacket
    {
        //---- Variables
        //--------------        
        private int dataLength;
        private string message;
        private string messageType;
        private List<byte> bytes;

        //---- Properties
        //---------------
        public string NetworkMessage => message;
        public string MessageType => messageType;

        public Byte[] Bytes => bytes.ToArray();
        public int Size => bytes.Count;

        //---- Ctor
        //---------
        public NetworkPacket()
        {
            bytes = new List<byte>();
        }

        //---- Reset
        //----------
        public void Reset()
        {            
            dataLength = 0;
            message = null;
            bytes.Clear();
        }

        //---- Wrtie
        //----------
        public bool Write(NetMessage netMessage)
        {
            try
            {
                Reset();
                
                messageType = netMessage.GetType().ToString();
                string json = JsonUtility.ToJson(netMessage);

                dataLength = messageType.Length + json.Length;

                // -- Order of data -- //
                // Length of message
                // Length of type
                // String of type
                // Data in json/string format            
                AppendInt(dataLength);
                AppendInt(messageType.Length);                
                AppendString(messageType);
                AppendString(json);
                return true;
            }
            catch(Exception ex)
            {
                Debug.LogError($"Network packet wrtie error\n{ex}");
                return false;
            }
        }

        private void AppendInt(int data)
        {
            byte[] databytes = BitConverter.GetBytes(data);
            bytes.AddRange(databytes);
        }

        private void AppendString(string data)
        {
            byte[] dataBytes = Encoding.Default.GetBytes(data);
            bytes.AddRange(dataBytes);
        }        

        //---- Read
        //---------
        public bool Read(byte[] data)
        {   
            try
            {
                dataLength = ReadInt(0, data);
                int messageTypeSize = ReadInt(sizeof(int), data);
                messageType = ReadString(sizeof(int) * 2, messageTypeSize, data);
                message = ReadString(sizeof(int) * 2 + messageTypeSize, dataLength - messageTypeSize, data);
                return true;
            }
            catch(Exception ex)
            {
                Debug.LogError($"Network packet read error\n{ex}");
                return false;
            }
        }

        private int ReadInt(int start, byte[] data)
        {   
            return BitConverter.ToInt32(data, start);
        }

        private string ReadString(int start, int size, byte[] data)
        {            
            return Encoding.Default.GetString(data, start, size);
        }
    } // end class
} // end namespace
