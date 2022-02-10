using System.Collections.Generic;

using Logging.Net;

using MCSharp.Enums;
using MCSharp.Pakets;
using MCSharp.Pakets.Client.Login;
using MCSharp.Pakets.Client.Play;
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

            var csd = Pakets[state];

            //Logger.Debug("Found registry: for " + state);

            foreach(var b in csd.Keys)
            {
                var pt = csd[b].GetType();

                if(pt.Name == paket.GetType().Name)
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
            registry.AddPaket(0x02, new LoginSuccessPaket(), MinecraftState.Login);
            registry.AddPaket(0x03, new SetCompressionPaket(), MinecraftState.Login);

            // Status
            registry.AddPaket(0x00, new StatusResponsePaket(), MinecraftState.Status);
            registry.AddPaket(0x01, new PongPaket(), MinecraftState.Status);

            // Play
            registry.AddPaket(0x26, new JoinGamePaket(), MinecraftState.Play);
            registry.AddPaket(0x21, new Pakets.Client.Play.KeepAlivePaket(), MinecraftState.Play);
            registry.AddPaket(0x04, new SpawnPlayerPaket(), MinecraftState.Play);
            registry.AddPaket(0x08, new AcknowledgePlayerDiggingPaket(), MinecraftState.Play);
            registry.AddPaket(0x0C, new BlockChangePaket(), MinecraftState.Play);
            registry.AddPaket(0x36, new PlayerInfoPaket(), MinecraftState.Play);
            registry.AddPaket(0x52, new UpdateHealthPaket(), MinecraftState.Play);
            registry.AddPaket(0x22, new ChunkDataUpdateLightPaket(), MinecraftState.Play);
        }

        // Pakets the server can understand
        public static void RegisterServerPakets(PaketRegistry registry)
        {
            // Handshake
            registry.AddPaket(0x00, new HandshakePaket(), MinecraftState.Handshaking);

            // Login
            registry.AddPaket(0x00, new LoginStartPaket(), MinecraftState.Login);
            registry.AddPaket(0x01, new EncryptionResponsePaket(), MinecraftState.Login);

            // Status
            registry.AddPaket(0x00, new StatusRequestPaket(), MinecraftState.Status);
            registry.AddPaket(0x01, new PingPaket(), MinecraftState.Status);

            // Play
            registry.AddPaket(0x0F, new Pakets.Server.Play.KeepAlivePaket(), MinecraftState.Play);
        }
    }
}
