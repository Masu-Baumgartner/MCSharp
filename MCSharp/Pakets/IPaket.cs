using MCSharp.Network;

namespace MCSharp.Pakets
{
    public interface IPaket
    {
        public void Encode(MinecraftStream minecraftStream);
        public void Decode(MinecraftStream minecraftStream);
    }
}
