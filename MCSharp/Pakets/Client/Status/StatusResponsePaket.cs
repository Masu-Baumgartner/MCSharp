using MCSharp.Network;

namespace MCSharp.Pakets.Client.Status
{
    public class StatusResponsePaket : IPaket
    {
        public string Status { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Status = minecraftStream.ReadString();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteString(Status);
        }
    }
}
