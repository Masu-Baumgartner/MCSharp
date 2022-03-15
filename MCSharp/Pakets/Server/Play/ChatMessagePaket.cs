using MCSharp.Network;

using System;
using System.Collections.Generic;
using System.Text;

namespace MCSharp.Pakets.Server.Play
{
    public class ChatMessagePaket : IPaket
    {
        public string Message { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Message = minecraftStream.ReadString();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteString(Message);
        }
    }
}
