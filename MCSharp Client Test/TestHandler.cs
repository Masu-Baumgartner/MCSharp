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
        }

        public void Play(IPaket paket)
        {
            if(paket is JoinGamePaket joinGamePaket)
            {
                Logger.Info("Joined the game");
            }

            if(paket is MCSharp.Pakets.Client.Play.KeepAlivePaket keepAlivePaket)
            {
                Logger.Info("Ping!");

                Connection.SendPaket(new MCSharp.Pakets.Server.Play.KeepAlivePaket()
                {
                    Payload = keepAlivePaket.Payload
                });
            }
        }

        public void Status(IPaket paket)
        {
            
        }
    }
}
