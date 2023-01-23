using GreenOnions.Interface;

namespace GreenOnions.GPT3Client
{
    internal class TimeOutWorker
    {
        internal DateTime TimeOutAt { get; set; }
        internal Action<GreenOnionsMessages>? TimeOutDo { get; set; }
    }
}
