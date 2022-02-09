using MCSharp.Enums;
using MCSharp.Pakets;

namespace MCSharp.Network
{
    public class PacketQueueItem
    {
        public IPaket Paket { get; set; }
        public MinecraftState State { get; set; }
    }
}
