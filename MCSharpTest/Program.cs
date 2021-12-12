using System;

using System.Net;
using System.Net.Sockets;

using MCSharp.Network;

namespace MCSharpTest
{
    class Program
    {
        static TcpListener listener;
        static byte[] Buffer = new byte[1024];
        static MinecraftConnection connection;

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

            connection = new MinecraftConnection(cl, MCSharp.Enums.MinecraftFlow.ClientToServer);
        }
    }
}
