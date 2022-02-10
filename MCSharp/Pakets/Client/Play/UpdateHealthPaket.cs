using MCSharp.Network;

namespace MCSharp.Pakets.Client.Play
{
    public class UpdateHealthPaket : IPaket
    {
        public float Health { get; set; }
        public int Food { get; set; }
        public float Saturation { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Health = minecraftStream.ReadFloat();
            Food = minecraftStream.ReadVarInt();
            Saturation = minecraftStream.ReadFloat();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteFloat(Health);
            minecraftStream.WriteVarInt(Food);
            minecraftStream.WriteFloat(Saturation);
        }
    }
}
