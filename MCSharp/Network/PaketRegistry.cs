using System.Collections.Generic;

using MCSharp.Pakets;

namespace MCSharp.Network
{
    public class PaketRegistry
    {
        public Dictionary<byte, IPaket> Pakets = new Dictionary<byte, IPaket>();

        public void AddPaket(byte b, IPaket p)
        {
            lock(Pakets)
            {
                Pakets.Add(b, p);
            }
        }

        public static void RegisterClientPakets(PaketRegistry registry)
        {

        }

        public static void RegisterServerPakets(PaketRegistry registry)
        {

        }
    }
}
