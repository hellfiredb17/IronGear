using System;
using Hawkeye;

namespace Hawkeye.Server
{
    //---- Server Net Messages ----
    //-----------------------------
    [Serializable]
    public class ClientConfigure : NetMessage
    {
        //---- Variables
        //--------------
        public int ClientId;

        //---- Ctor
        //---------
        public ClientConfigure(int id)
        {
            ClientId = id;
        }

        //---- Type
        //---------
        public override string Type()
        {
            return typeof(ClientConfigure).ToString();
        }
    }

    [Serializable]
    public class Message : NetMessage
    {
        //---- Variables
        //--------------
        public string Msg;

        //---- Ctor
        //---------
        public Message(string message)
        {
            Msg = message;
        }

        //---- Type
        //---------
        public override string Type()
        {
            return typeof(Message).ToString();
        }
    }

    [Serializable]
    public class GameTime : NetMessage
    {
        //---- Variables
        //--------------
        public int Time;

        //---- Ctor
        //---------
        public GameTime(int time)
        {
            Time = time;
        }

        //---- Type
        //---------
        public override string Type()
        {
            return typeof(GameTime).ToString();
        }
    }
} // end namespace
