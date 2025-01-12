using Lagrange.Core.Message;

namespace GreenOnions.NT.PictureSearcher
{
    internal class SearchingUser(MessageChain chain, DateTime timeOut)
    {
        public MessageChain Chain { get; set; } = chain;
        public DateTime TimeOut { get; set; } = timeOut;
    }
}
