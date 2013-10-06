using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantumShogi.Environment
{
    public class DXEnvironment
    {
        public static int SX
        {
            get { return 1280; }
        }

        public static int SY
        {
            get { return 720; }
        }

        public static int FontSize
        {
            get { return 16; }
        }

        public static int KeyCount
        {
            get { return 256; }
        }
    }
}
