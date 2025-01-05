using System.Collections.Concurrent;

namespace GreenOnions.NT.HPictures
{
    public static class LimitCache
    {
        public static ConcurrentDictionary<long, DateTime> GroupCoolDown { get; set; } = new ConcurrentDictionary<long, DateTime>();
        public static ConcurrentDictionary<long, DateTime> WhiteGroupCoolDown { get; set; } = new ConcurrentDictionary<long, DateTime>();
        public static ConcurrentDictionary<long, DateTime> PrivateMessageCoolDown { get; set; } = new ConcurrentDictionary<long, DateTime>();
        public static ConcurrentDictionary<long, int> LimitNumber { get; set; } = new ConcurrentDictionary<long, int>();
    }
}