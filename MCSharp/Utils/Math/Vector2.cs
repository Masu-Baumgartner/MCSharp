using System;
using System.Collections.Generic;
using System.Text;

namespace MCSharp.Utils.Math
{
    public class Vector2
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Vector2() { }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
