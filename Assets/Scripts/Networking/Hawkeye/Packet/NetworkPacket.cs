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
        //---- Variables
        //--------------        
        private string messageType;
        private string message;
        private int typeLength;
        private int msgLength;
        private List<byte> bytes;

        //---- Properties
        //---------------
        public byte[] Buffer => bytes.ToArray();
        public string Type => messageType;
        public string Message => message;
        public int Size => bytes.Count;

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
            typeLength = 0;
            msgLength = 0;
            messageType = null;
            message = null;
            bytes.Clear();
        }

        /// <summary>
        /// Does not clear the bytes array incase another message data is in buffer
        /// </summary>
        public void ResetForNewMessage()
        {
            typeLength = 0;
            msgLength = 0;
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
                
                messageType = netMessage.GetType().ToString();
                string json = JsonUtility.ToJson(netMessage);

                // -- Order of data -- //
                // Length of type
                // String of type
                // Length of msg
                // Strting of msg                
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
        /// <summary>
        /// Read bytes, returns 0 if not done (new more data), 1 if message is done, 2 for an error
        /// </summary>        
        public int Read(byte[] data)
        {   
            try
            {
                // add bytes to byte buffer for processing
                bytes.AddRange(data);

                // get type length
                if(typeLength == 0)
                {
                    if (bytes.Count > sizeof(int))
                    {
                        typeLength = ReadInt(0, bytes.GetRange(0, sizeof(int)).ToArray());
                        //Debug.Log($"Message Type Length: {typeLength}");
                        bytes.RemoveRange(0, sizeof(int));
                    }
                    else
                    {
                        return 0;
                    }
                }

                // get the message type
                if(string.IsNullOrEmpty(messageType))
                {
                    if(bytes.Count >= typeLength)
                    {
                        messageType = ReadString(0, typeLength, bytes.ToArray());
                        //Debug.Log($"Message Type: {messageType}");
                        bytes.RemoveRange(0, typeLength);
                    }
                    else
                    {
                        return 0;
                    }
                }

                // get message length
                if(msgLength == 0)
                {
                    if (bytes.Count > sizeof(int))
                    {
                        msgLength = ReadInt(0, bytes.ToArray());
                        //Debug.Log($"Message Length: {msgLength}");
                        bytes.RemoveRange(0, sizeof(int));
                    }
                    else
                    {
                        return 0;
                    }
                }

                // get message
                if(string.IsNullOrEmpty(message))
                {
                    if(bytes.Count >= msgLength)
                    {
                        message = ReadString(0, msgLength, bytes.ToArray());
                        //Debug.Log($"Message: {message}");
                        bytes.RemoveRange(0, msgLength);
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }

                Debug.LogError("Reached code that should never get to, check that we are reseting after complete message");
                return 1;
            }
            catch(Exception ex)
            {
                Debug.LogError($"Network packet read error\n{ex}");
                return 2;
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
