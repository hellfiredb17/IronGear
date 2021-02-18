using System;

namespace Hawkeye.NetMessages
{
    //---------- Enum Message Interfaces ------------
    //-----------------------------------------------
    public enum InterfaceTypes
    {
        None,
        Lobby,
    }

    //---------- NetMessage Utils -------------------
    //-----------------------------------------------
    public static class NetMessageUtils
    {
        public static InterfaceTypes GetInterfaceType(string str)
        {
            if(Enum.TryParse(str, out InterfaceTypes type))
            {
                return type;
            }
            return InterfaceTypes.None;
        }
    }

    //---------- NetMessage Interface / Class -------
    //-----------------------------------------------
    public interface INetMessage
    {
        string InterfaceType { get; }
        string NetMessageType { get; }
    }

    [Serializable]
    public abstract class NetMessage : INetMessage
    {
        //---- NetMessage Interface
        //-------------------------
        public virtual string InterfaceType => throw new NotImplementedException();
        public virtual string NetMessageType => throw new NotImplementedException();
    }

    //---------- Client to Dedi NetMessage ----------
    //-----------------------------------------------
    [Serializable]
    public abstract class ClientToDediMessage : NetMessage
    {
        //---- Variables
        //--------------
        public string ClientId;
    }

    //---------- Dedi to Client NetMessage ----------
    //-----------------------------------------------
    [Serializable]
    public abstract class DediToClientMessage : NetMessage
    {        
    }

} // end namespace
