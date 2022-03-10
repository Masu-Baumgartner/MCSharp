using MCSharp.Network;

using System;
using System.Collections.Generic;
using System.Text;

namespace MCSharp.Pakets.Client.Play
{
    public class UpdateViewPositionPaket : IPaket
    {
        public int ChunkX { get; set; }
        public int ChunkZ { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            ChunkX = minecraftStream.ReadVarInt();
            ChunkZ = minecraftStream.ReadVarInt();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(ChunkX);
            minecraftStream.WriteVarInt(ChunkZ);
        }
    }
}
