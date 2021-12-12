using System;
using System.Collections.Generic;
using System.Text;

namespace MCSharp.Utils.Data
{
    public class GameMode
    {
        public int mode { get; set; }

        public GameMode(byte b)
        {
            mode = Convert.ToInt32(b);
        }

        public GameMode(int i)
        {
            mode = i;
        }
    }
}
