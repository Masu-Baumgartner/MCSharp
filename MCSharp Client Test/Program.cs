using MCSharp.Network;
using MCSharp.Pakets.Server.Handshake;
using MCSharp.Pakets.Server.Login;

using System.Net.Sockets;

namespace MCSharp_Client_Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TcpClient cl = new TcpClient("127.0.0.1", 25565);

            var connection = new MinecraftConnection(cl, MCSharp.Enums.MinecraftFlow.ClientToServer);

            PaketRegistry writer = new PaketRegistry();
            PaketRegistry.RegisterClientPakets(writer);

            PaketRegistry reader = new PaketRegistry();
            PaketRegistry.RegisterServerPakets(reader);

            connection.WriterRegistry = writer;
            connection.ReaderRegistry = reader;

            connection.Handler = new TestHandler()
            {
                Connection = connection
            };

            connection.Start();

            connection.State = MCSharp.Enums.MinecraftState.Login;

            connection.SendPaket(new HandshakePaket()
            {
                NextState = 2,
                ProtocolVersion = 757,
                ServerAddress = "127.0.0.1",
                ServerPort = 25565
            });

            connection.SendPaket(new LoginStartPaket()
            {
                Username = "MCSharpClient"
            });
        }
    }
}