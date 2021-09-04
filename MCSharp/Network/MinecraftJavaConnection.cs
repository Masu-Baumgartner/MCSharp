using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

using MCSharp.Packets;

namespace MCSharp.Network
{
    public class MinecraftJavaConnection : IMinecraftConnection
    {
        public TcpClient TcpClient { get; private set; }
        public bool IsConnected { 
            get
            {
                return TcpClient.Connected;
            } 
        }
        private Queue<IPacket> PacketQueue { get; set; }
        private NetworkStream NetworkStream { get; set; }
        private bool IsWriting { get; set; } = false;
        private byte[] Buffer { get; set; }

        public MinecraftJavaConnection()
        {
            TcpClient = new TcpClient();
            PacketQueue = new Queue<IPacket>();

            Buffer = new byte[1024];
        }

        public void Connect(IPAddress address, int port)
        {
            TcpClient.BeginConnect(address, port, OnConnected_Callback, null);
        }

        public void SendPacket(IPacket packet)
        {
            if (PacketQueue.Count < 1 && !IsWriting)
                SendPacketNow(packet);
            else
                PacketQueue.Enqueue(packet);
        }

        private void SendPacketNow(IPacket packet)
        {
            IsWriting = true;
            byte[] data = packet.Serialize();

            // Compromize

            NetworkStream.BeginWrite(data, 0, data.Length, OnNetworkWrite_Callback, null);
        }

        #region Event Callbacks
        private void OnConnected_Callback(IAsyncResult ar)
        {
            TcpClient.EndConnect(ar);
            NetworkStream = TcpClient.GetStream();

            NetworkStream.BeginRead(Buffer, 0, Buffer.Length, OnNetworkRead_Callback, null);

            //TODO: Add event for connected
        }
        private void OnNetworkRead_Callback(IAsyncResult ar)
        {
            int size = NetworkStream.EndRead(ar);

            byte[] cache = new byte[size];
            Buffer.CopyTo(cache, 0);

            Buffer = new byte[Buffer.Length];
            NetworkStream.BeginRead(Buffer, 0, Buffer.Length, OnNetworkRead_Callback, null);

            
        }
        private void OnNetworkWrite_Callback(IAsyncResult ar)
        {
            NetworkStream.EndWrite(ar);
            IsWriting = false;

            if (PacketQueue.Count > 0)
                SendPacketNow(PacketQueue.Dequeue());
        }
        #endregion
    }
}
