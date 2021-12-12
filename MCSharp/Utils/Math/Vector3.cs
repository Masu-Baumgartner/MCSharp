using System;
using System.Collections.Generic;
using System.Text;

namespace MCSharp.Utils.Math
{
    public class Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public static Vector3 Backward
        {
            get
            {
                return new Vector3(0, 0, -1);
            }
        }

        public static Vector3 Down
        {
            get
            {
                return new Vector3(0, -1, 0);
            }
        }
        public static Vector3 Up
        {
            get
            {
                return new Vector3(0, 1, 0);
            }
        }
        public static Vector3 Right
        {
            get
            {
                return new Vector3(1, 0, 0);
            }
        }
        public static Vector3 Left
        {
            get
            {
                return new Vector3(-1, 0, 0);
            }
        }
        public static Vector3 Forward
        {
            get
            {
                return new Vector3(0, 0, 1);
            }
        }
        public static Vector3 Zero
        {
            get
            {
                return new Vector3(0, 0, 0);
            }
        }

        public Vector3() { }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
