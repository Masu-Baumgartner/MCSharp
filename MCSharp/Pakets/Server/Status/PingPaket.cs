using MCSharp.Network;

namespace MCSharp.Pakets.Server.Status
{
    public class PingPaket : IPaket
    {
        public long Payload{ get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Payload = minecraftStream.ReadLong();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteLong(Payload);
        }
    }
}
