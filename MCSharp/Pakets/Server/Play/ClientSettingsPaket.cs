using MCSharp.Network;

using System;
using System.Collections.Generic;
using System.Text;

namespace MCSharp.Pakets.Server.Play
{
    public class ClientSettingsPaket : IPaket
    {
        public string Locale { get; set; }
        public int ViewDistance { get; set; }
        public int ChatMode { get; set; }
        public bool ChatColors { get; set; }
        public uint DisplayedSkinParts { get; set; }
        public int MainHand { get; set; }
        public bool EnableTextFiltering { get; set; }
        public bool AllowServerListings { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Locale = minecraftStream.ReadString();
            ViewDistance = minecraftStream.ReadByte();
            ChatMode = minecraftStream.ReadVarInt();
            ChatColors = minecraftStream.ReadBool();
            DisplayedSkinParts = minecraftStream.ReadUnsignedByte();
            MainHand = minecraftStream.ReadVarInt();
            EnableTextFiltering = minecraftStream.ReadBool();
            AllowServerListings = minecraftStream.ReadBool();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteString(Locale);
            minecraftStream.WriteByte((byte)ViewDistance);
            minecraftStream.WriteVarInt(ChatMode);
            minecraftStream.WriteBool(ChatColors);
            minecraftStream.WriteByte((byte)DisplayedSkinParts);
            minecraftStream.WriteVarInt(MainHand);
            minecraftStream.WriteBool(EnableTextFiltering);
            minecraftStream.WriteBool(AllowServerListings);
        }
    }
}
