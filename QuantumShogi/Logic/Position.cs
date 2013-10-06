using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantumShogi.Logic
{
    public class Position
    {
        public Position(Piece p)
            : this(p.Pos)
        {
        }

        public Position(Position p)
            : this(p.X, p.Y, p.Orient)
        {
        }

        public Position(int X_, int Y_, Orientation Orient_)
        {
            X = X_;
            Y = Y_;
            Orient = Orient_;
        }

        public int X
        {
            set;
            get;
        }

        public int Y
        {
            set;
            get;
        }

        public Orientation Orient
        {
            set;
            get;
        }

        public enum Orientation
        {
            None = 0, Left = 1, Up = 2, Right = 4, Down = 8,
            LeftUp = Left | Up, LeftDown = Left | Down,
            RightUp = Right | Up, RightDown = Right | Down
        }
    }
}
