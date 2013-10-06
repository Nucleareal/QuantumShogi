using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantumShogi.Util
{
    public class NumberUtil
    {
        public static bool Contains(int Start, int End, int val)
        {
            return Start <= val && val <= End;
        }

        public static int Size(int Start, int End)
        {
            return End - Start + 1;
        }
    }
}
