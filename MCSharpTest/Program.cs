using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

using MCSharp.Network;

namespace MCSharpTest
{
    public class Program
    {
        static TcpListener listener;
        static byte[] Buffer = new byte[1024];
        public static List<MinecraftConnection> connections = new List<MinecraftConnection>();

        static void Main(string[] args)
        {
            listener = new TcpListener(IPAddress.Any, 25565);

            listener.Start();

            listener.BeginAcceptTcpClient(OnConnected, null);

            Console.ReadLine();
        }

        private static void OnConnected(IAsyncResult ar)
        {
            TcpClient cl = listener.EndAcceptTcpClient(ar);

            var connection = new MinecraftConnection(cl, MCSharp.Enums.MinecraftFlow.ClientToServer);

            PaketRegistry writer = new PaketRegistry();
            PaketRegistry.RegisterClientPakets(writer);

            PaketRegistry reader = new PaketRegistry();
            PaketRegistry.RegisterServerPakets(reader);

            connection.WriterRegistry = writer;
            connection.ReaderRegistry = reader;

            connection.Handler = new TestHandler()
            {
                con = connection
            };

            connection.Start();

            connections.Add(connection);

            listener.BeginAcceptTcpClient(OnConnected, null);
        }
    }
}