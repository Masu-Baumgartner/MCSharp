using MCSharp.Network;

using System;
using System.Collections.Generic;
using System.Text;

namespace MCSharp.Pakets.Server.Play
{
    public class PlayerPositionAndRotationPaket : IPaket
    {
        public double X { get; set; }
        public double FeetY { get; set; }
        public double Z { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public bool OnGround { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            X = minecraftStream.ReadDouble();
            FeetY = minecraftStream.ReadDouble();
            Z = minecraftStream.ReadDouble();
            Yaw = minecraftStream.ReadFloat();
            Pitch = minecraftStream.ReadFloat();
            OnGround = minecraftStream.ReadBool();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteDouble(X);
            minecraftStream.WriteDouble(FeetY);
            minecraftStream.WriteDouble(Z);
            minecraftStream.WriteFloat(Yaw);
            minecraftStream.WriteFloat(Pitch);
            minecraftStream.WriteBool(OnGround);
        }
    }
}
