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

        public override bool Equals(Object obj)
        {
            var v = obj as Position;
            if (v != null)
            {
                return X == v.X && Y == v.Y && Orient == v.Orient;
            }
            return false;
        }

        public Position(int X_, int Y_, Orientation Orient_)
        {
            X = X_;
            Y = Y_;
            Orient = Orient_;
        }

        public override string ToString()
        {
            return string.Format("({0},{1}):{2}", X, Y, Orient);
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
