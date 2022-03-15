using MCSharp.Network;
using MCSharp.Utils.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCSharp.Pakets.Client.Play
{
    public class ChatMessagePaket : IPaket
    {
        public string JsonData { get; set; }
        public int Position { get; set; }
        public UUID Sender { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            JsonData = minecraftStream.ReadString();
            Position = minecraftStream.ReadByte();
            Sender = minecraftStream.ReadUuid();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteString(JsonData);
            minecraftStream.WriteByte((byte)Position);
            minecraftStream.WriteUuid(Sender);
        }
    }
}
