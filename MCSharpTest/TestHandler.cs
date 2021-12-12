using System;
using System.Collections.Generic;
using System.Text;

using MCSharp.Network;
using MCSharp.Pakets;
using MCSharp.Pakets.Client.Login;
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
            if(paket is HandshakePaket)
            {
                Console.WriteLine("Serveraddress: " + ((HandshakePaket)paket).ServerAddress);
                
                if(((HandshakePaket)paket).NextState == 1)
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
            if(paket is LoginStartPaket)
            {
                Console.WriteLine("Username: " + ((LoginStartPaket)paket).Username);

                DisconnectPaket dp = new DisconnectPaket()
                {
                    Message = "{\"text\": \"foo\",\"bold\": \"true\",\"extra\": [{\"text\": \"bar\"},{\"text\": \"baz\",\"bold\": \"false\"},{\"text\": \"qux\",\"bold\": \"true\"}]}"
                };

                con.SendPaket(dp);
            }
        }

        public void Play(IPaket paket)
        {
            
        }

        public void Status(IPaket paket)
        {
            if(paket is StatusRequestPaket)
            {
                Console.WriteLine("Status requested");

                StatusResponsePaket srp = new StatusResponsePaket()
                {
                    Status = "{\"version\": {\"name\": \"1.18.1\",\"protocol\": 757},\"players\": {\"max\": 100,\"online\": 5,\"sample\": [{\"name\": \"thinkofdeath\",\"id\": \"4566e69f-c907-48ee-8d71-d7ba5aa00d20\"}]},\"description\": {\"text\": \"Hello world\"},\"favicon\": \"data:image/png;base64,<data>\"}"
                };

                con.SendPaket(srp);
            }

            if(paket is PingPaket)
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
