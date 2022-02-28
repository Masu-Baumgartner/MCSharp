using MCSharp.Network;
using MCSharp.Utils.Data;

using System;
using System.IO;

namespace MCSharp.Pakets.Client.Play
{
    public class UpdateLightPaket : IPaket
    {
		public int ChunkX, ChunkZ;
		public LightingData Data;

		public void Decode(MinecraftStream stream)
{
			ChunkX = stream.ReadVarInt();
			ChunkZ = stream.ReadVarInt();
			Data = LightingData.FromStream(stream);
        }

        public void Encode(MinecraftStream stream)
        {
			stream.WriteVarInt(ChunkX);
			stream.WriteVarInt(ChunkZ);
			LightingData.ToStream(stream, Data);
        }
    }

	public class LightingData
	{
		public bool TrustEdges;

		public BitSet SkyLightMask;

		public BitSet BlockLightMask;

		public BitSet EmptySkyLightMask;

		public BitSet EmptyBlockLightMask;

		public byte[][] SkyLight;
		public byte[][] BlockLight;

		public LightingData() { }

		public void Decode(MinecraftStream stream)
		{
			TrustEdges = stream.ReadBool();
			SkyLightMask = BitSet.Read(stream);
			BlockLightMask = BitSet.Read(stream);
			EmptySkyLightMask = BitSet.Read(stream);
			EmptyBlockLightMask = BitSet.Read(stream);

			int skyLightArrayCount = stream.ReadVarInt();
			SkyLight = new byte[skyLightArrayCount][];

			for (int idx = 0; idx < SkyLight.Length; idx++)
			{
				int length = stream.ReadVarInt();
				SkyLight[idx] = stream.Read(length);
			}

			int blockLightArrayCount = stream.ReadVarInt();
			BlockLight = new byte[blockLightArrayCount][];

			for (int idx = 0; idx < BlockLight.Length; idx++)
			{
				int length = stream.ReadVarInt();
				BlockLight[idx] = stream.Read(length);
			}
		}

		public void Encode(MinecraftStream stream)
        {
			stream.WriteBool(TrustEdges);
			BitSet.Write(stream, SkyLightMask);
			BitSet.Write(stream, BlockLightMask);
			BitSet.Write(stream, EmptySkyLightMask);
			BitSet.Write(stream, EmptyBlockLightMask);

			stream.WriteVarInt(SkyLight.Length);

			foreach(var i in SkyLight)
            {
				stream.WriteVarInt(i.Length);
				stream.Write(i);
            }

			stream.WriteVarInt(BlockLight.Length);

			foreach(var i in BlockLight)
            {
				stream.WriteVarInt(i.Length);
				stream.Write(i);
            }
        }

		public static LightingData FromStream(MinecraftStream stream)
		{
			var data = new LightingData();
			data.Decode(stream);

			return data;
		}

		public static void ToStream(MinecraftStream stream, LightingData data)
        {
			data.Encode(stream);
        }
	}
}
