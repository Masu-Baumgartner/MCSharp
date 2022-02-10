using MCSharp.Network;
using MCSharp.Utils.Data;

using System;
using System.Collections.Generic;
using System.Text;

namespace MCSharp.Pakets.Client.Play
{
    public class PlayerInfoPaket : IPaket
    {
        public int Action { get; set; }
        public int NumberOfPlayers { get; set; }
        public List<PlayerInfo> Players { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Action = minecraftStream.ReadVarInt();
            NumberOfPlayers = minecraftStream.ReadVarInt();
            Players = new List<PlayerInfo>();
            for(int i = 0; i < NumberOfPlayers; i++)
            {
                var pi = new PlayerInfo();

                pi.UUID = minecraftStream.ReadUuid();

                switch(Action)
                {
                    case 0:
                        pi.Name = minecraftStream.ReadString();
                        pi.NumberOfProperties = minecraftStream.ReadVarInt();
                        pi.Properties = new List<PlayerInfo.Property>();
                        
                        for(int ii = 0; ii < pi.NumberOfProperties; ii++)
                        {
                            var prop = new PlayerInfo.Property();

                            prop.Name = minecraftStream.ReadString();
                            prop.Value = minecraftStream.ReadString();
                            prop.Singed = minecraftStream.ReadBool();

                            if(prop.Singed)
                            {
                                prop.Signature = minecraftStream.ReadString();
                            }

                            pi.Properties.Add(prop);
                        }

                        pi.GameMode = minecraftStream.ReadVarInt();
                        pi.Ping = minecraftStream.ReadVarInt();
                        pi.HasDisplayName = minecraftStream.ReadBool();

                        if(pi.HasDisplayName)
                        {
                            pi.DisplayName = minecraftStream.ReadString();
                        }

                        break;

                    case 1:
                        pi.GameMode = minecraftStream.ReadVarInt();
                        break;
                    case 2:
                        pi.Ping = minecraftStream.ReadVarInt();
                        break;
                    case 3:
                        pi.HasDisplayName = minecraftStream.ReadBool();

                        if (pi.HasDisplayName)
                        {
                            pi.DisplayName = minecraftStream.ReadString();
                        }
                        break;
                }

                Players.Add(pi);
            }
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(Action);
            minecraftStream.WriteVarInt(NumberOfPlayers);

            foreach(var p in Players)
            {
                minecraftStream.WriteUuid(p.UUID);

                switch(Action)
                {
                    case 0:
                        minecraftStream.WriteString(p.Name);
                        minecraftStream.WriteVarInt(p.NumberOfProperties);

                        foreach(var prop in p.Properties)
                        {
                            minecraftStream.WriteString(prop.Name);
                            minecraftStream.WriteString(prop.Value);
                            minecraftStream.WriteBool(prop.Singed);

                            if(prop.Singed)
                            {
                                minecraftStream.WriteString(prop.Signature);
                            }
                        }

                        minecraftStream.WriteVarInt(p.GameMode);
                        minecraftStream.WriteVarInt(p.Ping);
                        minecraftStream.WriteBool(p.HasDisplayName);

                        if (p.HasDisplayName)
                            minecraftStream.WriteString(p.DisplayName);

                        break;

                    case 1:
                        minecraftStream.WriteVarInt(p.GameMode);
                        break;

                    case 2:
                        minecraftStream.WriteVarInt(p.Ping);
                        break;

                    case 3:
                        minecraftStream.WriteBool(p.HasDisplayName);

                        if (p.HasDisplayName)
                            minecraftStream.WriteString(p.DisplayName);

                        break;
                }
            }
        }

        public struct PlayerInfo
        {
            public UUID UUID { get; set; }
            public string Name { get; set; }
            public int NumberOfProperties { get; set; }
            public List<Property> Properties { get; set; }

            public struct Property
            {
                public string Name { get; set; }
                public string Value { get; set; }
                public bool Singed { get; set; }
                public string Signature { get; set; }
            }

            public int GameMode { get; set; }
            public int Ping { get; set; }
            public bool HasDisplayName { get; set; }
            public string DisplayName { get; set; }
        }
    }
}
