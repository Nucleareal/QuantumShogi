using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantumShogi.Util;

namespace QuantumShogi.Logic
{
    public class Rectangle
    {
        private int SX
        {
            set;
            get;
        }

        private int SY
        {
            set;
            get;
        }

        private int EX
        {
            set;
            get;
        }

        private int EY
        {
            set;
            get;
        }

        public int XSize
        {
            get { return NumberUtil.Size(SX, EX); }
        }

        public int YSize
        {
            get { return NumberUtil.Size(SY, EY); }
        }

        public bool Contains(Position p)
        {
            return XContains(p) && YContains(p);
        }

        public bool Contains(int x, int y)
        {
            return Contains(new Position(x, y, Position.Orientation.None));
        }

        public bool XContains(Position p)
        {
            return NumberUtil.Contains(SX, EX, p.X);
        }

        public bool YContains(Position p)
        {
            return NumberUtil.Contains(SY, EY, p.Y);
        }

        public Rectangle(int SX_, int SY_, int EX_, int EY_)
        {
            SX = SX_;
            SY = SY_;
            EX = EX_;
            EY = EY_;
        }

        public Rectangle(int X, int Y)
            : this(0, 0, X - 1, Y - 1)
        {
        }
    }
}
