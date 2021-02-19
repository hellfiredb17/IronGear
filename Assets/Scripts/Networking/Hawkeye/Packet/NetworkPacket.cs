using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Hawkeye.NetMessages;

namespace Hawkeye
{
    /// <summary>
    /// Base data to be sent over network
    /// Writes data into byte array that is sent over network connection
    /// </summary>
    public class NetworkPacket
    {
        //---- Enum
        //---------
        public enum ProcessResult
        {
            NotDone = 0,
            Done,
            Error
        }

        //---- Variables
        //--------------        
        private string interfaceType;
        private string messageType;
        private string message;
        private int interfaceLength;
        private int typeLength;
        private int msgLength;
        private List<byte> bytes;

        //---- Properties
        //---------------
        public byte[] Buffer => bytes.ToArray();
        public string Interface => interfaceType;
        public string Type => messageType;
        public string Message => message;
        public int Size => bytes.Count;
        public bool BytesNullOrEmpty => bytes == null || bytes.Count == 0;

        //---- Ctor
        //---------
        public NetworkPacket()
        {
            bytes = new List<byte>();
        }

        //---- Resets
        //-----------
        public void Reset()
        {
            interfaceLength = 0;
            typeLength = 0;
            msgLength = 0;
            interfaceType = null;
            messageType = null;
            message = null;
            bytes.Clear();
        }

        /// <summary>
        /// Does not clear the bytes array incase another message data is in buffer
        /// </summary>
        public void ResetForNewMessage()
        {
            interfaceLength = 0;
            typeLength = 0;
            msgLength = 0;
            interfaceType = null;
            messageType = null;
            message = null;
        }

        //---- Wrtie
        //----------
        public bool Write(NetMessage netMessage)
        {
            try
            {
                Reset();

                interfaceType = netMessage.InterfaceType;
                messageType = netMessage.NetMessageType;
                string json = JsonUtility.ToJson(netMessage);

                // -- Order of data -- //
                // Length of interface
                // String of interface
                // Length of type
                // String of type
                // Length of msg
                // Strting of msg                
                AppendInt(interfaceType.Length);
                AppendString(interfaceType);
                AppendInt(messageType.Length);                
                AppendString(messageType);
                AppendInt(json.Length);
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
        public void AppendBytes(byte[] data)
        {
            // add bytes to byte buffer for processing
            bytes.AddRange(data);
        }

        /// <summary>
        /// Read bytes, returns not done (new more data), done, or an error
        /// </summary>
        public ProcessResult Read()
        {   
            try
            {
                // parse interface, type, and message
                if(!GetInterface() || !GetMessageType())
                {
                    return ProcessResult.NotDone;
                }

                // parse the message
                return GetMessage();
            }
            catch(Exception ex)
            {
                Debug.LogError($"Network packet read error\n{ex}");
                return ProcessResult.Error;
            }
        }

        private bool GetInterface()
        {
            // get interface length
            if (interfaceLength == 0)
            {
                if (bytes.Count > sizeof(int))
                {
                    interfaceLength = ReadInt(0, bytes.GetRange(0, sizeof(int)).ToArray());
                    bytes.RemoveRange(0, sizeof(int));
                }
                else
                {
                    return false;
                }
            }

            // get the interface type
            if (string.IsNullOrEmpty(interfaceType))
            {
                if (bytes.Count >= interfaceLength)
                {
                    interfaceType = ReadString(0, interfaceLength, bytes.ToArray());
                    bytes.RemoveRange(0, interfaceLength);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private bool GetMessageType()
        {
            // get type length
            if (typeLength == 0)
            {
                if (bytes.Count > sizeof(int))
                {
                    typeLength = ReadInt(0, bytes.GetRange(0, sizeof(int)).ToArray());                    
                    bytes.RemoveRange(0, sizeof(int));
                }
                else
                {
                    return false;
                }
            }

            // get the message type
            if (string.IsNullOrEmpty(messageType))
            {
                if (bytes.Count >= typeLength)
                {
                    messageType = ReadString(0, typeLength, bytes.ToArray());                    
                    bytes.RemoveRange(0, typeLength);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private ProcessResult GetMessage()
        {
            // get message length
            if (msgLength == 0)
            {
                if (bytes.Count > sizeof(int))
                {
                    msgLength = ReadInt(0, bytes.ToArray());                    
                    bytes.RemoveRange(0, sizeof(int));
                }
                else
                {
                    return ProcessResult.NotDone;
                }
            }

            // get message            
            if (bytes.Count >= msgLength)
            {
                message = ReadString(0, msgLength, bytes.ToArray());                    
                bytes.RemoveRange(0, msgLength);
                return ProcessResult.Done;
            }
            else
            {
                return ProcessResult.NotDone;
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
