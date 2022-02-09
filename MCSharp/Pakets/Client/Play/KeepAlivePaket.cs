using MCSharp.Network;

using System;
using System.Collections.Generic;
using System.Text;

namespace MCSharp.Pakets.Client.Play
{
    public class KeepAlivePaket : IPaket
    {
        public long Payload { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Payload = minecraftStream.ReadInt();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteLong(Payload);
        }
    }
}
