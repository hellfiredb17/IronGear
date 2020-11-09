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
        private int recipient;
        private int dataLength;
        private string message;
        private List<byte> bytes;

        //---- Properties
        //---------------
        public string NetworkMessage => message;
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
            recipient = -1;
            dataLength = 0;
            message = null;
            bytes.Clear();
        }

        //---- Wrtie
        //----------
        public bool Write(int id, string data)
        {
            try
            {
                Reset();
                dataLength = data.Length;

                // -- Order of data -- //
                // Length of message
                // Recipient id
                // Data in json/string format            
                AppendInt(dataLength);
                AppendInt(id);
                AppendString(data);
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
                recipient = ReadInt(sizeof(int), data);
                message = ReadString(sizeof(int) * 2, dataLength, data);                
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
