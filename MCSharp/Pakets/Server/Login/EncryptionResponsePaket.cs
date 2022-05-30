using MCSharp.Network;

namespace MCSharp.Pakets.Server.Login
{
    public class EncryptionResponsePaket : IPaket
    {
        public int SharedKeyLenght { get; set; }
        public byte[] SharedKey { get; set; }
        public int VerifyTokenLenght { get; set; }
        public byte[] VerifyToken { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            SharedKeyLenght = minecraftStream.ReadVarInt();
            SharedKey = minecraftStream.Read(SharedKeyLenght);
            VerifyTokenLenght = minecraftStream.ReadVarInt();
            VerifyToken = minecraftStream.Read(VerifyTokenLenght);
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(SharedKeyLenght);
            minecraftStream.Write(SharedKey);
            minecraftStream.WriteVarInt(VerifyTokenLenght);
            minecraftStream.Write(VerifyToken);
        }
    }
}
