using GreenOnions.NT.OpenAiClient.Models;
using Lagrange.Core.Message;

namespace GreenOnions.NT.OpenAiClient
{
    internal class ChatingUser(MessageChain chain, DateTime timeOut, ChatConfig config)
    {
        public MessageChain Chain { get; set; } = chain;
        public DateTime TimeOut { get; set; } = timeOut;
        public ChatConfig Config { get; set; } = config;
        public List<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
    }
}
