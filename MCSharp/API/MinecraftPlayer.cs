using System;
using System.Collections.Generic;
using System.Text;

namespace MCSharp.API
{
    public class MinecraftPlayer
    {
        internal MinecraftClient Client { get; set; }
        public MinecraftPlayer(MinecraftClient client)
        {
            // Initialize here
            Client = client;
        }
    }
}
