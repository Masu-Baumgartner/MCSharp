using System;
using System.Collections.Generic;
using System.Text;

using MCSharp.Pakets;
using MCSharp.Pakets.Handshake;
using MCSharp.Pakets.Login;

namespace MCSharpTest
{
    public class TestHandler : IPaketHandler
    {
        public void Handshake(IPaket paket)
        {
            if(paket is HandshakePaket)
            {
                Console.WriteLine("Serveraddress: " + ((HandshakePaket)paket).ServerAddress);
                Program.connection.State = MCSharp.Enums.MinecraftState.Login;
            }
        }

        public void Login(IPaket paket)
        {
            if(paket is LoginStartPaket)
            {
                Console.WriteLine("Username: " + ((LoginStartPaket)paket).Username);
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
