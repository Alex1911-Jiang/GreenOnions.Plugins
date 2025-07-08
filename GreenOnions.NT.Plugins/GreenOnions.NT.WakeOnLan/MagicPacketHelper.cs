using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace GreenOnions.NT.WakeOnLan
{
    public enum WakeStatus
    {
        InvalidInput,
        AlreadyAwake,
        WakeSuccessful,
        WakeTimeout,
        Error
    }

    public static class MagicPacketHelper
    {
        private const int DelayBeforePingMs = 40_000;
        private const int PingIntervalMs    = 1_000;
        private const int PingTimeoutMs     = 1_000;
        private const int PostWakeAttempts  = 40;

        public static async Task<WakeStatus> SendMagicPacket(string? mac, string? ip)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(mac) || string.IsNullOrWhiteSpace(ip))
                    return WakeStatus.InvalidInput;

                if (await PingHost(ip))
                    return WakeStatus.AlreadyAwake;

                var macBytes = mac.Split([':', '-', ' '], StringSplitOptions.RemoveEmptyEntries)
                                  .Select(b => Convert.ToByte(b, 16))
                                  .ToArray();
                if (macBytes.Length != 6) return WakeStatus.InvalidInput;

                var packet = new byte[102];
                for (int i = 0; i < 6; i++) packet[i] = 0xFF;
                for (int i = 1; i <= 16; i++)
                    Buffer.BlockCopy(macBytes, 0, packet, i * 6, 6);

                using (var client = new UdpClient())
                {
                    client.EnableBroadcast = true;
                    await client.SendAsync(packet, packet.Length, new IPEndPoint(IPAddress.Broadcast, 9));
                }

                await Task.Delay(DelayBeforePingMs);

                for (int i = 0; i < PostWakeAttempts; i++)
                {
                    if (await PingHost(ip)) return WakeStatus.WakeSuccessful;
                    await Task.Delay(PingIntervalMs);
                }

                return WakeStatus.WakeTimeout;
            }
            catch
            {
                return WakeStatus.Error;
            }
        }

        private static async Task<bool> PingHost(string ip)
        {
            using var ping = new Ping();
            try
            {
                var reply = await ping.SendPingAsync(ip, PingTimeoutMs);
                return reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}
