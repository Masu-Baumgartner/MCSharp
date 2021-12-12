using System;

using MCSharp.Network;
using MCSharp.Utils.Data;

namespace MCSharp.Pakets.Client.Login
{
    public class LoginSuccessPaket : IPaket
    {
        public UUID Uuid { get; set; }
        public string Username { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Uuid = minecraftStream.ReadUuid();
            Username = minecraftStream.ReadString();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteUuid(Uuid);
            minecraftStream.WriteString(Username);
        }
    }
}
