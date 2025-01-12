using System.Collections.Concurrent;

namespace GreenOnions.NT.HPictures
{
    public static class LimitCache
    {
        public static ConcurrentDictionary<uint, DateTime> GroupCoolDown { get; set; } = new ConcurrentDictionary<uint, DateTime>();
        public static ConcurrentDictionary<uint, DateTime> WhiteGroupCoolDown { get; set; } = new ConcurrentDictionary<uint, DateTime>();
        public static ConcurrentDictionary<uint, DateTime> PrivateMessageCoolDown { get; set; } = new ConcurrentDictionary<uint, DateTime>();
        public static ConcurrentDictionary<uint, int> LimitNumber { get; set; } = new ConcurrentDictionary<uint, int>();
    }
}