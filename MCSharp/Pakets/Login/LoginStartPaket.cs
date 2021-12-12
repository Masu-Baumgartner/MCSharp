using MCSharp.Network;

namespace MCSharp.Pakets.Login
{
    public class LoginStartPaket : IPaket
    {
        public string Username { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Username = minecraftStream.ReadString();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteString(Username);
        }
    }
}
