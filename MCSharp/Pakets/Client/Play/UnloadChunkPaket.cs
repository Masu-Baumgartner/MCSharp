using MCSharp.Network;
using MCSharp.Utils.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCSharp.Pakets.Client.Play
{
    public class UnloadChunkPaket : IPaket
    {
        public int ChunkX { get; set; }
        public int ChunkZ { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            ChunkX = minecraftStream.ReadInt();
            ChunkZ = minecraftStream.ReadInt();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteInt(ChunkX);
            minecraftStream.WriteInt(ChunkZ);
        }
    }
}
