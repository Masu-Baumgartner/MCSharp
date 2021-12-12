using MCSharp.Network;

namespace MCSharp.Pakets.Client.Login
{
    public class DisconnectPaket : IPaket
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
