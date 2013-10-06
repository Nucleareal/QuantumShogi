using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantumShogi.Logic;

namespace QuantumShogi.Environment
{
    public class ShogiEnvironment
    {
        public static int Board_X
        {
            get { return 9; }
        }

        public static int Board_Y
        {
            get { return 9; }
        }

        public static class Player
        {
            public static int Count
            {
                get { return 2; }
            }
        }

        public static class Piece
        {
            public static int Count
            {
                get { return 40; }
            }

            public static int SizeX
            {
                get { return 64; }
            }

            public static int SizeY
            {
                get { return 64; }
            }

            public static int Types
            {
                get { return QuantumShogi.Logic.Piece.Type.Array.Length; }
            }

            public static string[] Names
            {
                get { return new string[]{
                    "歩兵", "香車", "桂馬", "銀将", "金将", "角行", "飛車", "玉将",
                    "と金", "成香", "成桂", "成銀", "　　", "竜馬", "竜王", "王将",
                }; }
            }
        }
    }
}
