using System.Net;

namespace MCSharp.Network
{
    interface IMinecraftConnection
    {
        public void Connect(IPAddress address, int port);
        public bool IsConnected { get; }
    }
}
