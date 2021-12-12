namespace MCSharp.Pakets
{
    public interface IPaketHandler
    {
        public void Handshake(IPaket paket);
        public void Status(IPaket paket);
        public void Login(IPaket paket);
        public void Play(IPaket paket);
    }
}
