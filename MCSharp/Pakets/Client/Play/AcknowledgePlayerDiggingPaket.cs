using MCSharp.Network;
using MCSharp.Utils.Math;

using System;
using System.Collections.Generic;
using System.Text;

namespace MCSharp.Pakets.Client.Play
{
    public class AcknowledgePlayerDiggingPaket : IPaket
    {
        public BlockCoordinates Location { get; set; }
        public int Block { get; set; }
        public int Status { get; set; } // Same as Player Digging. Only Started digging (0), Cancelled digging (1), and Finished digging (2) are used.
        public bool Successful { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            ulong ll = minecraftStream.ReadULong();
            var x = ll >> 38;
            var y = ll & 0xFFF;
            var z = (ll >> 12) & 0x3FFFFFF;
            Location = new BlockCoordinates((int)x, (int)y, (int)z);
            Block = minecraftStream.ReadVarInt();
            Status = minecraftStream.ReadVarInt();
            Successful = minecraftStream.ReadBool();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            ulong ll = (ulong)(((Location.X & 0x3FFFFFF) << 38) | ((Location.Y & 0x3FFFFFF) << 12) | (Location.Z & 0xFFF));

            minecraftStream.WriteULong(ll);
            minecraftStream.WriteVarInt(Block);
            minecraftStream.WriteVarInt(Status);
            minecraftStream.WriteBool(Successful);
        }
    }
}
