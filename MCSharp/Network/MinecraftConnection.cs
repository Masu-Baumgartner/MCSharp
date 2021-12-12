using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

using MCSharp.Enums;

namespace MCSharp.Network
{
    public class MinecraftConnection
    {
        private TcpClient Tcp { get; set; }
        public MinecraftFlow Flow { get; private set; }
        public MinecraftState State { get; private set; }

        public MinecraftConnection(TcpClient tcp)
        {
            Tcp = tcp;
        }

        public MinecraftConnection(TcpClient tcp, MinecraftFlow flow) : this(tcp)
        {
            Flow = flow;
        }
    }
}
