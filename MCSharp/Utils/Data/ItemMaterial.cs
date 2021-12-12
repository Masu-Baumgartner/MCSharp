using System;
using System.Collections.Generic;
using System.Text;

namespace MCSharp.Utils.Data
{
	[Flags]
	public enum ItemMaterial : int
	{
		Any = None | AnyMaterial,
		AnyMaterial = Wood | Stone | Gold | Iron | Diamond,
		//Leather = -2, // 0xFFFFFFFE
		//Chain = -1, // 0xFFFFFFFF

		None = 1 << 0,
		Wood = 1 << 1,
		Stone = 1 << 2,
		Gold = 1 << 3,
		Iron = 1 << 4,
		Diamond = 1 << 5,
	}
}
