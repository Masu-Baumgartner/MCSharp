using System.Collections.Generic;

using MCSharp.Enums;
using MCSharp.Pakets;
using MCSharp.Pakets.Handshake;
using MCSharp.Pakets.Login;

namespace MCSharp.Network
{
    public class PaketRegistry
    {
        public Dictionary<MinecraftState, Dictionary<byte, IPaket>> Pakets = new Dictionary<MinecraftState, Dictionary<byte, IPaket>>();

        public PaketRegistry()
        {
            Pakets.Add(MinecraftState.Handshaking, new Dictionary<byte, IPaket>());
            Pakets.Add(MinecraftState.Status, new Dictionary<byte, IPaket>());
            Pakets.Add(MinecraftState.Login, new Dictionary<byte, IPaket>());
            Pakets.Add(MinecraftState.Play, new Dictionary<byte, IPaket>());
        }

        public void AddPaket(byte b, IPaket p, MinecraftState state)
        {
            Pakets[state].Add(b, p);
        }

        public byte GetPaketId(IPaket paket, MinecraftState state)
        {
            byte res = 0x00;

            foreach (byte b in Pakets.Keys)
            {
                IPaket cp = Pakets[state][b];

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
            registry.AddPaket(0x00, new HandshakePaket(), MinecraftState.Handshaking);
            registry.AddPaket(0x00, new LoginStartPaket(), MinecraftState.Login);
        }
    }
}
