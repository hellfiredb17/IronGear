using System;

namespace Hawkeye.Models
{
    public class LobbyChatModel
    {
        //---- Variables
        //--------------
        public DateTime DateTime;
        public string DisplayName;
        public string Chat;

        //---- Ctor
        //---------
        public LobbyChatModel(string name, string chat)
        {
            DateTime = DateTime.Now;
            DisplayName = name;
            Chat = chat;
        }
    } // end class
} // end namespace
