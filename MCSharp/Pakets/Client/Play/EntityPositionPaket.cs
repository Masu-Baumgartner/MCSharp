using MCSharp.Network;
using MCSharp.Utils.Data;

namespace MCSharp.Pakets.Client.Play
{
    public class EntityPositionPaket : IPaket
    {
        public int EntityId { get; set; }
        public short DeltaX { get; set; }
        public short DeltaY { get; set; }
        public short DeltaZ { get; set; }
        public bool OnGround { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            DeltaX = minecraftStream.ReadShort();
            DeltaY = minecraftStream.ReadShort();
            DeltaZ = minecraftStream.ReadShort();
            OnGround = minecraftStream.ReadBool();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(EntityId);
            minecraftStream.WriteDouble(DeltaX);
            minecraftStream.WriteDouble(DeltaY);
            minecraftStream.WriteDouble(DeltaZ);
            minecraftStream.WriteBool(OnGround);
        }
    }
}
