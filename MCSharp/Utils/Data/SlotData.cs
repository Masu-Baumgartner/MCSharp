using fNbt;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCSharp.Utils.Data
{
	public class SlotData
	{
		public int ItemID = -1;
		public short ItemDamage = 0;
		public byte Count = 0;

		public NbtCompound Nbt = null;

		public SlotData()
		{

		}
	}
}
