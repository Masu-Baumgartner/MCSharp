using Logging.Net;

using MCSharp.Network;
using MCSharp.Pakets;
using MCSharp.Pakets.Client.Login;

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
                Connection.CompressionThreshold = compressionPaket.Threshold;
                Connection.CompressionEnabled = true;
            }

            if(paket is LoginSuccessPaket loginSuccessPaket)
            {
                Connection.State = MCSharp.Enums.MinecraftState.Play;
                Logger.Info("Login success");
            }
        }

        public void Play(IPaket paket)
        {
            
        }

        public void Status(IPaket paket)
        {
            
        }
    }
}
