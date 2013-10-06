using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantumShogi.Environment;
using DxLibDLL;
using QuantumShogi.Image;

namespace QuantumShogi.Logic
{
    public class World
    {
        private Piece[,] Board;
        private Piece[,] PlayerStock;
        private Rectangle Rect;
        private int PieceCounter;

        public World()
        {
            PieceCounter = 0;
            PlayerStock = new Piece[ShogiEnvironment.Player.Count, ShogiEnvironment.Piece.Count];
            Rect = new Rectangle(ShogiEnvironment.Board_X, ShogiEnvironment.Board_Y);
            Board = new Piece[ShogiEnvironment.Board_X, ShogiEnvironment.Board_Y];
        }

        public bool GetMoveableTo(Piece pc, int dx, int dy)
        {
            ConvertToRealValue(pc, ref dx, ref dy);
            return Rect.Contains(pc.Pos) && IsMoveable(pc, dx, dy);
        }

        public Position CreateNewPosition(Piece pc, int dx, int dy)
        {
            ConvertToRealValue(pc, ref dx, ref dy);
            return new Position(pc);
        }

        //private Methods

        private void ConvertToRealValue(Piece pc, ref int x, ref int y)
        {
            double rect = GetRect(pc.Orient);
            int dx = (int)Math.Round(Math.Sin(rect) * x - Math.Cos(rect) * y);
            int dy = (int)Math.Round(Math.Cos(rect) * x + Math.Sin(rect) * y);
            x = pc.Pos.X + dx;
            y = pc.Pos.Y + dy;
        }

        private double GetRect(Position.Orientation orient)
        {
            switch (orient)
            {
                case Position.Orientation.Up: return PI(0D);
                case Position.Orientation.Down: return PI(1D);
                //case Position.Orientation.Left: return PI(0.5D);
                //case Position.Orientation.Right: return PI(1.5D);
            }
            return double.NaN;
        }

        private double PI(double rate)
        {
            return Math.PI * rate;
        }

        private bool IsMoveable(Piece pc, int x, int y)
        {
            return Board[x, y].IsEmpty() || Board[x, y].Pos.Orient != pc.Pos.Orient;
        }

        private void Move(int frX, int frY, int toX, int toY)
        {
            Piece p = Board[frX, frY];
        }

        public void PlacePiece(int x, int y, Position.Orientation orient)
        {
            Console.WriteLine("({0},{1}) Board.Size[{2},{3}]", x, y, Board.GetLength(0), Board.GetLength(1));
            if (orient == Position.Orientation.None)
            {
                Board[x, y] = new Piece(-1);
            }
            else
            {
                Board[x, y] = new Piece(PieceCounter++);
            }
            Board[x, y].Pos = new Position(x, y, orient);
        }

        public void Draw()
        {
            int X = ShogiEnvironment.Board_X;
            int Y = ShogiEnvironment.Board_Y;
            int PX = ShogiEnvironment.Piece.SizeX;
            int PY = ShogiEnvironment.Piece.SizeY;
            int Types = ShogiEnvironment.Piece.Types;

            DX.DrawGraph(0, 0, ImageLoader.BoardHandler, DX.TRUE);

            for(int i = 0; i < X; i++)
                for (int j = 0; j < Y; j++)
                {
                    Piece p = Board[i, j];
                    foreach (var v in p.AllTypes)
                    {
                        bool isDown = p.Orient == Position.Orientation.Down;
                        int TypeHandle = v.GrHandle + (isDown ? Types * 2 : 0);
                        int GrHandle = ImageLoader.PieceHandler[TypeHandle/Types, TypeHandle%Types];
                        DX.DrawGraph(j*PX, i*PY, GrHandle, DX.TRUE);
                    }
                }


            DX.DrawGraph(0, 0, ImageLoader.PieceHandler[0,0], DX.TRUE);
        }
    }
}
