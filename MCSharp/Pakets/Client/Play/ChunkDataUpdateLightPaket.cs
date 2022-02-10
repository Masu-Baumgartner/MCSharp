using MCSharp.Network;

using fNbt;
using System.Collections.Generic;

namespace MCSharp.Pakets.Client.Play
{
    public class ChunkDataUpdateLightPaket : IPaket
    {
        public int ChunkX { get; set; }
        public int ChunkZ { get; set; }
        public NbtCompound Heightmaps { get; set; }
        public int Size { get; set; }
        public byte[] ChunkData { get; set; }
        public int EntitiesCount { get; set; }
        public List<BlockEntity> Entities { get; set; }
        public bool TrustEdges { get; set; }
        public BitSet SkyLightMask { get; set; }
        public BitSet BlockLightMask { get; set; }
        public BitSet EmptySkyLightMask { get; set; }
        public BitSet EmptyBlockLightMask { get; set; }
        public int SkyLightLenght { get; set; }
        public List<LightArrayThing> SkyLight { get; set; }
        public int BlockLightLenght { get; set; }
        public List<LightArrayThing> BlockLight { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            ChunkX = minecraftStream.ReadInt();
            ChunkZ = minecraftStream.ReadInt();

            /*
            int t = minecraftStream.ReadVarInt();
            for (int i = 0; i < t; i++)
                minecraftStream.ReadLong();*/

            Heightmaps = minecraftStream.ReadNbtCompound();
            Size = minecraftStream.ReadVarInt();
            ChunkData = minecraftStream.Read(Size);
            EntitiesCount = minecraftStream.ReadVarInt();
            Entities = new List<BlockEntity>();

            for(int i = 0; i < EntitiesCount; i++)
            {
                var be = new BlockEntity();

                be.PackedXZ = minecraftStream.ReadByte();
                be.Y = minecraftStream.ReadShort();
                be.Type = minecraftStream.ReadVarInt();
                be.Data = minecraftStream.ReadNbtCompound();

                Entities.Add(be);
            }

            TrustEdges = minecraftStream.ReadBool();

            var bs = new BitSet();
            bs.Lenght = minecraftStream.ReadVarInt();
            bs.Bits = new List<long>();

            for(int i = 0; i < bs.Lenght; i++)
            {
                bs.Bits.Add(minecraftStream.ReadLong());
            }

            SkyLightMask = bs;

            bs = new BitSet();
            bs.Lenght = minecraftStream.ReadVarInt();
            bs.Bits = new List<long>();

            for (int i = 0; i < bs.Lenght; i++)
            {
                bs.Bits.Add(minecraftStream.ReadLong());
            }

            BlockLightMask = bs;

            bs = new BitSet();
            bs.Lenght = minecraftStream.ReadVarInt();
            bs.Bits = new List<long>();

            for (int i = 0; i < bs.Lenght; i++)
            {
                bs.Bits.Add(minecraftStream.ReadLong());
            }

            EmptySkyLightMask = bs;

            bs = new BitSet();
            bs.Lenght = minecraftStream.ReadVarInt();
            bs.Bits = new List<long>();

            for (int i = 0; i < bs.Lenght; i++)
            {
                bs.Bits.Add(minecraftStream.ReadLong());
            }

            EmptyBlockLightMask = bs;

            SkyLightLenght = minecraftStream.ReadVarInt();
            SkyLight = new List<LightArrayThing>();

            for(int i = 0; i < SkyLightLenght; i++)
            {
                var l = new LightArrayThing();
                l.Byte = new List<byte>();

                l.Lenght = minecraftStream.ReadVarInt();

                for(int ii = 0; ii < l.Lenght; ii++)
                {
                    l.Byte.Add((byte)minecraftStream.ReadByte());
                }

                SkyLight.Add(l);
            }

            BlockLightLenght = minecraftStream.ReadVarInt();
            BlockLight = new List<LightArrayThing>();

            for (int i = 0; i < BlockLightLenght; i++)
            {
                var l = new LightArrayThing();
                l.Byte = new List<byte>();

                l.Lenght = minecraftStream.ReadVarInt();

                for (int ii = 0; ii < l.Lenght; ii++)
                {
                    l.Byte.Add((byte)minecraftStream.ReadByte());
                }

                BlockLight.Add(l);
            }

        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteInt(ChunkX);
            minecraftStream.WriteInt(ChunkZ);
            minecraftStream.WriteNbtCompound(Heightmaps);
            minecraftStream.WriteVarInt(Size);
            minecraftStream.Write(ChunkData);
            minecraftStream.WriteVarInt(EntitiesCount);
            
            foreach(var e in Entities)
            {
                minecraftStream.WriteByte((byte)e.PackedXZ);
                minecraftStream.WriteShort(e.Y);
                minecraftStream.WriteVarInt(e.Type);
                minecraftStream.WriteNbtCompound(e.Data);
            }

            minecraftStream.WriteBool(TrustEdges);

            minecraftStream.WriteVarInt(SkyLightMask.Lenght);
            foreach (var l in SkyLightMask.Bits)
                minecraftStream.WriteLong(l);

            minecraftStream.WriteVarInt(BlockLightMask.Lenght);
            foreach (var l in BlockLightMask.Bits)
                minecraftStream.WriteLong(l);

            minecraftStream.WriteVarInt(EmptySkyLightMask.Lenght);
            foreach (var l in EmptySkyLightMask.Bits)
                minecraftStream.WriteLong(l);

            minecraftStream.WriteVarInt(EmptyBlockLightMask.Lenght);
            foreach (var l in EmptyBlockLightMask.Bits)
                minecraftStream.WriteLong(l);

            minecraftStream.WriteVarInt(SkyLightLenght);
            foreach(var lt in SkyLight)
            {
                minecraftStream.WriteVarInt(lt.Lenght);

                foreach(var b in lt.Byte)
                {
                    minecraftStream.WriteByte(b);
                }
            }

            minecraftStream.WriteVarInt(BlockLightLenght);
            foreach (var lt in BlockLight)
            {
                minecraftStream.WriteVarInt(lt.Lenght);

                foreach (var b in lt.Byte)
                {
                    minecraftStream.WriteByte(b);
                }
            }
        }

        public struct BlockEntity
        {
            public int PackedXZ { get; set; }
            public short Y { get; set; }
            public int Type { get; set; }
            public NbtCompound Data { get; set; }
        }

        public struct BitSet
        {
            public int Lenght { get; set; }
            public List<long> Bits { get; set; }
        }

        public struct LightArrayThing
        {
            public int Lenght { get; set; }
            public List<byte> Byte { get; set; }
        }
    }
}
