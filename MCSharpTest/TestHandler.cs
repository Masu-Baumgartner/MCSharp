using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using fNbt;

using MCSharp.Network;
using MCSharp.Pakets;
using MCSharp.Pakets.Client.Login;
using MCSharp.Pakets.Client.Play;
using MCSharp.Pakets.Client.Status;
using MCSharp.Pakets.Server.Handshake;
using MCSharp.Pakets.Server.Login;
using MCSharp.Pakets.Server.Status;

namespace MCSharpTest
{
    public class TestHandler : IPaketHandler
    {
        public MinecraftConnection con { get; set; }

        public void Handshake(IPaket paket)
        {
            if (paket is HandshakePaket)
            {
                Console.WriteLine("Serveraddress: " + ((HandshakePaket)paket).ServerAddress);

                if (((HandshakePaket)paket).NextState == 1)
                {
                    con.State = MCSharp.Enums.MinecraftState.Status;
                }
                else
                {
                    con.State = MCSharp.Enums.MinecraftState.Login;
                }
            }
        }

        public void Login(IPaket paket)
        {
            if (paket is LoginStartPaket)
            {
                Console.WriteLine("Username: " + ((LoginStartPaket)paket).Username);

                /*
                 *  DisconnectPaket dp = new DisconnectPaket()
                    {
                        Message = "{\"text\": \"foo\",\"bold\": \"true\",\"extra\": [{\"text\": \"bar\"},{\"text\": \"baz\",\"bold\": \"false\"},{\"text\": \"qux\",\"bold\": \"true\"}]}"
                    };

                    con.SendPaket(dp);
                 */

                SetCompressionPaket scp = new SetCompressionPaket()
                {
                    Threshold = 256
                };

                con.SendPaket(scp);

                LoginSuccessPaket lsp = new LoginSuccessPaket()
                {
                    Uuid = new MCSharp.Utils.Data.UUID("881ac95e-af99-4875-832c-b42d7ea82cb7"),
                    Username = "Masusniper"
                };

                con.SendPaket(lsp);

                Thread.Sleep(500);

                con.State = MCSharp.Enums.MinecraftState.Play;

                NbtFile file = new NbtFile();
                file.LoadFromFile("dimcodec.nbt");
                var dimcodec = file.RootTag;

                JoinGamePaket jgp = new JoinGamePaket()
                {
                    EntityId = 1234,
                    IsHardcore = true,
                    Gamemode = 1,
                    PreviousGamemode = 1,
                    WorldCount = 3,
                    WorldNames = new string[]
                    {
                        "minecraft:overworld",
                        "minecraft:nether",
                        "minecraft:the_end"
                    },
                    WorldName = "minecraft:overworld",
                    DimesionCodec = dimcodec,
                    Dimesion = dimcodec,
                    EnableRespawnScreen = true,
                    HashedSeed = 12345678987,
                    IsDebug = false,
                    IsFlat = true,
                    MaxPlayers = 100,
                    ReducedDebugInfo = false,
                    SimulationDistance = 10,
                    ViewDistance = 10
                };

                con.SendPaket(jgp);
            }
        }

        public void Play(IPaket paket)
        {

        }

        public void Status(IPaket paket)
        {
            if (paket is StatusRequestPaket)
            {
                Console.WriteLine("Status requested");

                StatusResponsePaket srp = new StatusResponsePaket()
                {
                    Status = "{\"version\": {\"name\": \"1.18.1\",\"protocol\": 757},\"players\": {\"max\": 100,\"online\": 5,\"sample\": [{\"name\": \"thinkofdeath\",\"id\": \"4566e69f-c907-48ee-8d71-d7ba5aa00d20\"}]},\"description\": {\"text\": \"Hello world\"},\"favicon\": \"data:image/png;base64,<data>\"}"
                };

                con.SendPaket(srp);
            }

            if (paket is PingPaket)
            {
                Console.WriteLine("Ping");
                PongPaket pp = new PongPaket()
                {
                    Payload = ((PingPaket)paket).Payload
                };

                con.SendPaket(pp);
            }
        }
    }
}
