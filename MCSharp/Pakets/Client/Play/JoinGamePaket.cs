using System.Collections.Generic;

using fNbt;

using MCSharp.Network;

namespace MCSharp.Pakets.Client.Play
{
    public class JoinGamePaket : IPaket
    {
        public int EntityId { get; set; }
        public bool IsHardcore { get; set; }
        public int Gamemode { get; set; }
        public int PreviousGamemode { get; set; }
        public int WorldCount { get; set; }
        public string[] WorldNames { get; set; }
        public NbtCompound DimesionCodec { get; set; }
        public NbtCompound Dimesion { get; set; }
        public string WorldName { get; set; }
        public long HashedSeed { get; set; }
        public int MaxPlayers { get; set; }
        public int ViewDistance { get; set; }
        public int SimulationDistance { get; set; }
        public bool ReducedDebugInfo { get; set; }
        public bool EnableRespawnScreen { get; set; }
        public bool IsDebug { get; set; }
        public bool IsFlat { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            IsHardcore = minecraftStream.ReadBool();
            Gamemode = minecraftStream.ReadByte();
            PreviousGamemode = minecraftStream.ReadByte();
            WorldCount = minecraftStream.ReadVarInt();

            List<string> names = new List<string>();

            for (int i = 0; i <= WorldCount; i++)
            {
                names.Add(minecraftStream.ReadString());
            }

            WorldNames = names.ToArray();

            Dimesion = minecraftStream.ReadNbtCompound();
            DimesionCodec = minecraftStream.ReadNbtCompound();

            WorldName = minecraftStream.ReadString();
            HashedSeed = minecraftStream.ReadLong();
            MaxPlayers = minecraftStream.ReadVarInt();
            ViewDistance = minecraftStream.ReadVarInt();
            SimulationDistance = minecraftStream.ReadVarInt();
            ReducedDebugInfo = minecraftStream.ReadBool();
            EnableRespawnScreen = minecraftStream.ReadBool();
            IsDebug = minecraftStream.ReadBool();
            IsFlat = minecraftStream.ReadBool();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(EntityId);
            minecraftStream.WriteBool(IsHardcore);
            minecraftStream.WriteByte((byte)Gamemode);
            minecraftStream.WriteByte((byte)PreviousGamemode);
            minecraftStream.WriteVarInt(WorldCount);

            for(int i = 0; i <= WorldCount; i++)
            {
                minecraftStream.WriteString(WorldNames[i]);
            }

            minecraftStream.WriteNbtCompound(Dimesion);
            minecraftStream.WriteNbtCompound(DimesionCodec);
            minecraftStream.WriteString(WorldName);
            minecraftStream.WriteLong(HashedSeed);
            minecraftStream.WriteVarInt(MaxPlayers);
            minecraftStream.WriteVarInt(ViewDistance);
            minecraftStream.WriteVarInt(SimulationDistance);
            minecraftStream.WriteBool(ReducedDebugInfo);
            minecraftStream.WriteBool(EnableRespawnScreen);
            minecraftStream.WriteBool(IsDebug);
            minecraftStream.WriteBool(IsFlat);
        }
    }
}
