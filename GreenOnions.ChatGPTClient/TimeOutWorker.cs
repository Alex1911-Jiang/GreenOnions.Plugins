using GreenOnions.Interface;

namespace GreenOnions.ChatGPTClient
{
    internal class TimeOutWorker
    {
        internal DateTime TimeOutAt { get; set; }
        internal Action<GreenOnionsMessages>? TimeOutDo { get; set; }
    }
}
