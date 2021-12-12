using System.Collections.Generic;

using MCSharp.Enums;
using MCSharp.Pakets;
using MCSharp.Pakets.Client.Login;
using MCSharp.Pakets.Client.Status;
using MCSharp.Pakets.Server.Handshake;
using MCSharp.Pakets.Server.Login;
using MCSharp.Pakets.Server.Status;

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
            // Login
            registry.AddPaket(0x00, new DisconnectPaket(), MinecraftState.Login);
            registry.AddPaket(0x01, new EncryptionRequestPaket(), MinecraftState.Login);

            // Status
            registry.AddPaket(0x00, new StatusResponsePaket(), MinecraftState.Status);
            registry.AddPaket(0x01, new PongPaket(), MinecraftState.Status);
        }

        // Pakets the server can understand
        public static void RegisterServerPakets(PaketRegistry registry)
        {
            // Handshake
            registry.AddPaket(0x00, new HandshakePaket(), MinecraftState.Handshaking);

            // Login
            registry.AddPaket(0x00, new LoginStartPaket(), MinecraftState.Login);

            // Status
            registry.AddPaket(0x00, new StatusRequestPaket(), MinecraftState.Status);
            registry.AddPaket(0x01, new PingPaket(), MinecraftState.Status);
        }
    }
}
