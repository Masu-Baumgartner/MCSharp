using Logging.Net;

using MCSharp.Network;
using MCSharp.Pakets;
using MCSharp.Pakets.Client.Login;
using MCSharp.Pakets.Client.Play;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSharp_Client_Test
{
    public class TestHandler : IPaketHandler
    {
        public MinecraftConnection Connection { get; set; }

        public void Handshake(IPaket paket)
        {
            
        }

        public void Login(IPaket paket)
        {
            if(paket is SetCompressionPaket compressionPaket)
            {
                // Compresssion is automatic enabled so this is just a log
                Logger.Info("Enabled compression");
            }

            if(paket is LoginSuccessPaket loginSuccessPaket)
            {
                Connection.State = MCSharp.Enums.MinecraftState.Play;
                Logger.Info("Login success");
            }

            if(paket is DisconnectPaket disconnectPaket)
            {
                Logger.Info("Disconnected: " + disconnectPaket.Message);
            }
        }

        public void Play(IPaket paket)
        {
            if(paket is JoinGamePaket joinGamePaket)
            {
                Logger.Info("Joined the game");

                Logger.Info("Gamemode: " + joinGamePaket.Gamemode);
                Logger.Info("Sim: " + joinGamePaket.SimulationDistance);
            }

            if(paket is MCSharp.Pakets.Client.Play.KeepAlivePaket keepAlivePaket)
            {
                Logger.Info("Ping! Payload: " + keepAlivePaket.Payload);

                Connection.SendPaket(new MCSharp.Pakets.Server.Play.KeepAlivePaket()
                {
                    Payload = keepAlivePaket.Payload
                });
            }

            if(paket is SpawnPlayerPaket spawnPlayerPaket)
            {
                Logger.Info($"Spawning player at {spawnPlayerPaket.X} {spawnPlayerPaket.Y} {spawnPlayerPaket.Z}");
            }

            if(paket is AcknowledgePlayerDiggingPaket acknowledgePlayerDigging)
            {
                var pos = acknowledgePlayerDigging.Location;
                Logger.Info($"Player is digging at {pos.X} {pos.Y} {pos.Z}");
            }

            if(paket is BlockChangePaket blockChangePaket)
            {
                var pos = blockChangePaket.Location;
                Logger.Info($"Block changed at {pos.X} {pos.Y} {pos.Z} to {blockChangePaket.BlockId}");
            }

            if(paket is PlayerInfoPaket playerInfoPaket)
            {
                if(playerInfoPaket.Action == 0)
                {
                    Logger.Info("Players:");
                    foreach(var p in playerInfoPaket.Players)
                    {
                        Logger.Info($"{p.Name} > Gamemode: {p.GameMode} Ping: {p.Ping}");
                    }
                }
            }
        }

        public void Status(IPaket paket)
        {
            
        }
    }
}
