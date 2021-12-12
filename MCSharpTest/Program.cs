using System;

using System.Net;
using System.Net.Sockets;

namespace MCSharpTest
{
    class Program
    {
        static TcpListener listener;
        static byte[] Buffer = new byte[1024];
        static TcpClient client;

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

            client = cl;

            listener.BeginAcceptTcpClient(OnConnected, null);

            cl.GetStream().BeginRead(Buffer, 0, Buffer.Length, OnRead, null);
        }

        private static void OnRead(IAsyncResult ar)
        {
            int size = client.GetStream().EndRead(ar);

            byte[] localData = new byte[size];
            Buffer.CopyTo(localData, 0);
            Buffer = new byte[1024];

            client.GetStream().BeginRead(Buffer, 0, Buffer.Length, OnRead, null);
        }
    }
}
