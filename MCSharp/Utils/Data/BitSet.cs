using MCSharp.Network;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MCSharp.Utils.Data
{
	public class BitSet
	{
		private readonly long[] _data;

		/// <summary>
		///		The size of the backing storage
		/// </summary>
		public int Length => _data.Length;

		/// <summary>
		///		The total amount of bits available
		/// </summary>
		public int Count => _data.Length * 64;

		public BitSet(long[] data)
		{
			_data = data;
		}

		static int CountSetBits(long[] input)
		{
			int count = 0;

			for (int i = 0; i < input.Length; i++)
				count += CountSetBits(input[i]);

			return count;
		}

		static int CountSetBits(long n)
		{
			int count = 0;

			while (n > 0)
			{
				n &= (n - 1);
				count++;
			}

			return count;
		}

		public bool IsSet(int bit)
		{
			if ((bit / 64) >= _data.Length) return false;

			// bit >> 6
			return (_data[bit / 64] & (1L << (bit % 64))) != 0;
		}

		public void Set(int bit, bool value)
		{
			throw new NotImplementedException();
		}

		public static BitSet Read(MinecraftStream ms)
		{
			var length = ms.ReadVarInt();
			long[] data = new long[length];

			for (int i = 0; i < data.Length; i++)
			{
				data[i] = ms.ReadLong();
			}

			return new BitSet(data);
		}

		public static void Write(MinecraftStream ms, BitSet bitSet)
        {
			ms.WriteVarInt(bitSet.Count);

			foreach(var l in bitSet._data)
            {
				ms.WriteLong(l);
            }
        }
	}
}
