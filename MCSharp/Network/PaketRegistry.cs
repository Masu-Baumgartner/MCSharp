using System.Collections.Generic;

using MCSharp.Pakets;

namespace MCSharp.Network
{
    public class PaketRegistry
    {
        public Dictionary<byte, IPaket> Pakets = new Dictionary<byte, IPaket>();

        public void AddPaket(byte b, IPaket p)
        {
            lock (Pakets)
            {
                Pakets.Add(b, p);
            }
        }

        public byte GetPaketId(IPaket paket)
        {
            byte res = 0x00;

            foreach (byte b in Pakets.Keys)
            {
                IPaket cp = Pakets[b];

                if(cp.GetType() == paket.GetType())
                {
                    res = b;
                    break;
                }
            }

            return res;
        }

        // Pakets the client can understand
        public static void RegisterClientPakets(PaketRegistry registry)
        {

        }

        // Pakets the server can understand
        public static void RegisterServerPakets(PaketRegistry registry)
        {
            registry.AddPaket(0x00, new HandshakePaket());
        }
    }
}
