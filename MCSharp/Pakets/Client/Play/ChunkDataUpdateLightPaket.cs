using MCSharp.Network;

using fNbt;
using System.Collections.Generic;
using System;
using Logging.Net;
using Org.BouncyCastle.Bcpg;
using System.Threading.Tasks;
using System.IO;

namespace MCSharp.Pakets.Client.Play
{
    public class ChunkDataUpdateLightPaket : IPaket
    {
		public int ChunkX;
		public int ChunkZ;

		public Memory<byte> Buffer;
		public List<BlockEntityData> TileEntities;
		public NbtCompound HeightMaps;

		public LightingData LightingData;

        public void Decode(MinecraftStream stream)
		{
			ChunkX = stream.ReadInt();
			ChunkZ = stream.ReadInt();

			HeightMaps = stream.ReadNbtCompound();

			int i = stream.ReadVarInt();
			Buffer = new Memory<byte>(new byte[i]);
			stream.ReadMemory(Buffer, i);

			int tileEntities = stream.ReadVarInt();

			for (int k = 0; k < tileEntities; k++)
			{
				BlockEntityData blockEntity = new BlockEntityData();
				blockEntity.Read(stream);
				TileEntities.Add(blockEntity);
			}

			LightingData = LightingData.FromStream(stream);
		}

		public void Encode(MinecraftStream stream)
		{
			stream.WriteInt(ChunkX);
			stream.WriteInt(ChunkZ);

			stream.WriteNbtCompound(HeightMaps);

			stream.WriteVarInt(Buffer.Length);
			stream.Write(Buffer.ToArray());
			
			stream.WriteVarInt((int)TileEntities.Count);

			foreach(BlockEntityData blockEntity in TileEntities)
            {
				blockEntity.Write(stream);
            }

			LightingData.ToStream(stream, LightingData);
		}

		public class BlockEntityData
		{
			public byte X { get; set; }
			public byte Z { get; set; }
			public short Y { get; set; }
			public int Type { get; set; }
			public NbtCompound Data { get; set; }

			public BlockEntityData() { }

			public void Read(MinecraftStream stream)
			{
				var packedXZ = stream.ReadUnsignedByte();

				X = (byte)(packedXZ >> 4);
				Z = (byte)((packedXZ) & 15);

				Y = stream.ReadShort();
				Type = stream.ReadVarInt();
				Data = stream.ReadNbtCompound();
			}

			public void Write(MinecraftStream stream) 
			{
				byte b = (byte)(X << 4 | Z & 15);
				stream.Write(new byte[] {b});

				stream.WriteShort(Y);
				stream.WriteVarInt(Type);
				stream.WriteNbtCompound(Data);
			}
		}
	}
}
