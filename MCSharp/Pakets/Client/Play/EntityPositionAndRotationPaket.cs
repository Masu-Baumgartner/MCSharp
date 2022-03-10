using MCSharp.Network;
using MCSharp.Utils.Data;

namespace MCSharp.Pakets.Client.Play
{
    public class EntityPositionAndRotationPaket : IPaket
    {
        public int EntityId { get; set; }
        public short DeltaX { get; set; }
        public short DeltaY { get; set; }
        public short DeltaZ { get; set; }
        public byte[] Yaw { get; set; }
        public byte[] Pitch { get; set; }
        public bool OnGround { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            DeltaX = minecraftStream.ReadShort();
            DeltaY = minecraftStream.ReadShort();
            DeltaZ = minecraftStream.ReadShort();
            Yaw = minecraftStream.Read(1);
            Pitch = minecraftStream.Read(1);
            OnGround = minecraftStream.ReadBool();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(EntityId);
            minecraftStream.WriteDouble(DeltaX);
            minecraftStream.WriteDouble(DeltaY);
            minecraftStream.WriteDouble(DeltaZ);
            minecraftStream.Write(Yaw, 0, 1);
            minecraftStream.Write(Pitch, 0, 1);
            minecraftStream.WriteBool(OnGround);
        }
    }
}
