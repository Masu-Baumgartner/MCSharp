using MCSharp.Network;

namespace MCSharp.Pakets.Client.Login
{
    public class EncryptionRequestPaket : IPaket
    {
        public string ServerId { get; set; }
        public int PublicKeyLenght { get; set; }
        public byte[] PublicKey { get; set; }
        public int VerifyTokenLenght { get; set; }
        public byte[] VerifyToken { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            ServerId = minecraftStream.ReadString();
            PublicKeyLenght = minecraftStream.ReadVarInt();
            minecraftStream.Read(PublicKey, PublicKeyLenght);
            VerifyTokenLenght = minecraftStream.ReadVarInt();
            minecraftStream.Read(VerifyToken, VerifyTokenLenght);
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteString(ServerId);
            minecraftStream.WriteVarInt(PublicKey.Length);
            minecraftStream.Write(PublicKey);
            minecraftStream.WriteVarInt(VerifyToken.Length);
            minecraftStream.Write(VerifyToken);
        }
    }
}
