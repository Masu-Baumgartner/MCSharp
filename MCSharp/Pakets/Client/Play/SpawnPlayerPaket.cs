using MCSharp.Network;
using MCSharp.Utils.Data;

namespace MCSharp.Pakets.Client.Play
{
    public class SpawnPlayerPaket : IPaket
    {
        public int EntityId { get; set; }
        public UUID UUID { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public int Yaw { get; set; }
        public int Pitch { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            UUID = minecraftStream.ReadUuid();
            X = minecraftStream.ReadDouble();
            Y = minecraftStream.ReadDouble();
            Z = minecraftStream.ReadDouble();
            Yaw = minecraftStream.ReadByte();
            Pitch = minecraftStream.ReadByte();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(EntityId);
            minecraftStream.WriteUuid(UUID);
            minecraftStream.WriteDouble(X);
            minecraftStream.WriteDouble(Y);
            minecraftStream.WriteDouble(Z);
            minecraftStream.WriteByte((byte)Yaw);
            minecraftStream.WriteByte((byte)Pitch);
        }
    }
}
