using System;
using System.Collections.Generic;
using System.Text;

using MCSharp.Pakets;

namespace MCSharpTest
{
    public class TestHandler : IPaketHandler
    {
        public void Handshake(IPaket paket)
        {
            if(paket is HandshakePaket)
            {
                Console.WriteLine("Serveraddress: " + ((HandshakePaket)paket).ServerAddress);
            }
        }

        public void Login(IPaket paket)
        {
            
        }

        public void Play(IPaket paket)
        {
            
        }

        public void Status(IPaket paket)
        {
            
        }
    }
}
