using System;
using System.Collections.Generic;
using System.Text;

namespace MCSharp.API
{
    public class MinecraftClient
    {
        // Properties
        public string ServerAdress { get; private set; }
        public int Port { get; private set; }
        public bool Connected { get; private set; }
        public MinecraftPlayer Player { get; private set; }

        // Events
        public event BlockUpdateEventHandler BlockUpdate;

        // Event fire
        internal void OnBlockUpdate(BlockUpdateEventArgs e)
        {
            if (Connected)
                BlockUpdate?.Invoke(this, e);
        }

        // 
        public MinecraftClient()
        {
            // Initialize here
            Player = new MinecraftPlayer(this);
        }

        // Methods
        public void Connect(string serverAdress, int port)
        {
            ServerAdress = serverAdress;
            Port = port;
            Connected = true;
        }
        public void Disconnect()
        {
            Connected = false;
        }
    }
}
