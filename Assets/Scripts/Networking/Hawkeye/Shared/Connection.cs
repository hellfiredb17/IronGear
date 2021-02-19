using System.Net.Sockets;
using System;
using Hawkeye.NetMessages;

namespace Hawkeye
{
    public class Connection
    {
        //---- Interfaces
        //---------------
        public ILog Log;

        //---- Variables
        //--------------
        public string NetworkId;
        public TcpClient Socket;
        public SharedEnums.ConnectionStatus Status;

        // network packets
        protected NetworkStream stream;
        protected NetworkPacket packet;
        protected byte[] readBuffer;

        //---- Ctor
        //---------
        public Connection()
        {
            Status = SharedEnums.ConnectionStatus.None;
        }

        //---- Read Messages
        //------------------
        protected void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int byteLength = stream.EndRead(result);
                if (byteLength <= 0)
                {
                    Status = SharedEnums.ConnectionStatus.Disconnect;
                    return;
                }

                // create network packet for incoming messages
                if (packet == null)
                {
                    packet = new NetworkPacket();
                }

                byte[] data = new byte[byteLength];
                Array.Copy(readBuffer, data, byteLength);

                // read packet
                packet.AppendBytes(data);
                while (!packet.BytesNullOrEmpty)
                {
                    NetworkPacket.ProcessResult process = packet.Read();
                    if (process == NetworkPacket.ProcessResult.Error)
                    {
                        Status = SharedEnums.ConnectionStatus.Disconnect;
                        return;
                    }

                    if(process == NetworkPacket.ProcessResult.NotDone)
                    {
                        // Packet was broken up, need another packet to finish so wait to read more
                        break;
                    }
                    else if (process == NetworkPacket.ProcessResult.Done)
                    {
                        // Network message done, process                    
                        ProcessNetMessage(packet.Interface, packet.Type, packet.Message);
                        packet.ResetForNewMessage();

                        // will continue to read until all messages are read from bytes.
                    }
                }                

                // start reading again
                stream.BeginRead(readBuffer, 0, SharedConsts.DATABUFFERSIZE, ReceiveCallback, null);
            }
            catch (Exception ex)
            {
                Log?.Error($"Error receiving message: {ex}");
                // disconnect
                Status = SharedEnums.ConnectionStatus.Disconnect;
            }
        }

        protected virtual void ProcessNetMessage(string interfaceType, string messageType, string message)
        {
            throw new NotImplementedException("Processing net messages is not implemented");
        }

        //---- Send
        //---------
        protected void SendNetMessage(NetMessage netMessage)
        {
            if (Status != SharedEnums.ConnectionStatus.Connect)
            {
                Log?.Error($"Connection status: {Status} cannot sent message");
                return;
            }

            NetworkPacket packet = new NetworkPacket();
            if (packet.Write(netMessage))
            {
                stream.BeginWrite(packet.Buffer, 0, packet.Size, SendPacketCallback, null);
            }
        }

        protected void SendPacketCallback(IAsyncResult result)
        {
            stream.EndWrite(result);
        }

        //---- Close / Disconnect
        //-----------------------
        public virtual void Close()
        {
            stream?.Close();
            Socket?.Close();
            Status = SharedEnums.ConnectionStatus.Close;
        }

        public virtual void Disconnect()
        {
            // TODO ?
            Status = SharedEnums.ConnectionStatus.Disconnect;
        }

    } // end class
} // end namespace
