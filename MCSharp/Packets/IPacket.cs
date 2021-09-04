namespace MCSharp.Packets
{
    public interface IPacket
    {
        public byte[] Serialize();
        public IPacket Deserialize();
    }
}
