using MCSharp.Network;

namespace MCSharp.Pakets.Client.Login
{
    public class SetCompressionPaket : IPaket
    {
        public int Threshold { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Threshold = minecraftStream.ReadVarInt();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(Threshold);
        }
    }
}
