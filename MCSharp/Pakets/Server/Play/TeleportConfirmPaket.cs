using MCSharp.Network;

using System;
using System.Collections.Generic;
using System.Text;

namespace MCSharp.Pakets.Server.Play
{
    public class TeleportConfirmPaket : IPaket
    {
        public int TeleportID { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            TeleportID = minecraftStream.ReadVarInt();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(TeleportID);
        }
    }
}
