using System;
using System.Collections.Generic;
using System.Text;

namespace MCSharp.Utils.Data
{
    public class Color
    {
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }

        public Color(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }
    }
}
