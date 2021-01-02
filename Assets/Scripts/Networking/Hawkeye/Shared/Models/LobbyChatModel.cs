
namespace Hawkeye.Models
{
    public class LobbyChatModel
    {
        //---- Variables
        //--------------
        public string ChatMessage;

        //---- Ctor
        //---------
        public LobbyChatModel(int netId, string chatMessage)
        {
            ChatMessage = chatMessage;
        }
    } // end class
} // end namespace
