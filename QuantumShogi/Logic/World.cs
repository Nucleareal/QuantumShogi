using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantumShogi.Environment;
using DxLibDLL;
using QuantumShogi.Image;
using QuantumShogi.Util;
using QuantumShogi.Proxy;

namespace QuantumShogi.Logic
{
    public class World
    {
        private Piece[,] Board;
        private List<Piece>[] PlayerStock;
        private Rectangle Rect;
        private Rectangle GraphRect;
        private int PieceCounter;
        private int FontSize;
        private List<Hand> PlayerHands;

        private static readonly int[] CountPlys = { 9, 2, 2, 2, 2, 1, 1, 1 };
        private int[,] PieceCounters;

        private int cx = -1;
        private int cy = -1;
        private List<Position> ClickedList;

        public World()
        {
            PieceCounter = 0;
            PlayerStock = new List<Piece>[ShogiEnvironment.Player.Count];
            PlayerStock[0] = new List<Piece>();
            PlayerStock[1] = new List<Piece>();
            Rect = new Rectangle(ShogiEnvironment.Board_X, ShogiEnvironment.Board_Y);
            GraphRect = new Rectangle(ShogiEnvironment.Piece.SizeX*ShogiEnvironment.Board_X, ShogiEnvironment.Piece.SizeY*ShogiEnvironment.Board_Y);
            FontSize = DXEnvironment.FontSize;
            ClickedList = new List<Position>();
            PlayerHands = new List<Hand>();
            DisappearNumbers = new List<int>();
        }

        public void InitBoard()
        {
            Board = new Piece[ShogiEnvironment.Board_X, ShogiEnvironment.Board_Y];
            PieceCounters = new int[ShogiEnvironment.Player.Count, CountPlys.Length];
            PieceCounter = 0;
        }

        public bool GetMoveableTo(Piece pc, int dx, int dy)
        {
            ConvertToRealValue(pc, ref dx, ref dy);
            return Rect.Contains(pc.Pos) && IsMoveable(pc, dx, dy);
        }

        public Position CreateNewPosition(Piece pc, int dx, int dy)
        {
            ConvertToRealValue(pc, ref dx, ref dy);
            return new Position(dx, dy, Position.Orientation.None);
        }

        public bool IsEmpty(Piece pc, int dx, int dy)
        {
            ConvertToRealValue(pc, ref dx, ref dy);
            return Board[dx, dy].Orient == Position.Orientation.None;
        }

        public void PlacePiece(int x, int y, Position.Orientation orient)
        {
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

        private int mx;
        private int my;
        private int rx;
        private int ry;

        public void OnFocus(int x, int y)
        {
            if (GraphRect.Contains(x, y))
            {
                mx = x;
                my = y;
            }
            else
            {
                mx = -1;
            }
        }

        public void OnClick(int Code)
        {
            if (mx < 0) return;
            rx = mx / ShogiEnvironment.Piece.SizeX;
            ry = my / ShogiEnvironment.Piece.SizeY;
            if (Code == (int)MouseCode.LEFTCLICK)
            {
                if (ClickedList.Count > 0)
                {
                    Position pos = new Position(rx, ry, Position.Orientation.None);

                    if (ClickedList.Contains(pos))
                    {
                        Move(cx, cy, rx, ry);
                    }
                    ClickedList.Clear();
                }
                else
                {
                    ClickedList.Clear();
                    foreach (var v in Board[rx, ry].AllTypes)
                    {
                        List<Position> res = v.Moving(this, Board[rx, ry]);
                        foreach (var r in res)
                            if (!ClickedList.Contains(r))
                                ClickedList.Add(r);
                    }
                }
            }
            cx = rx;
            cy = ry;
        }

        public void Draw()
        {
            int X = ShogiEnvironment.Board_X;
            int Y = ShogiEnvironment.Board_Y;
            int PX = ShogiEnvironment.Piece.SizeX;
            int PY = ShogiEnvironment.Piece.SizeY;
            int Types = ShogiEnvironment.Piece.Types;

            DX.DrawGraph(0, 0, ImageLoader.BoardHandler, DX.TRUE);

            int px = mx + 16;
            int py = my + 16;

            for (int i = 0; i < X; i++)
                for (int j = 0; j < Y; j++)
                {
                    Piece p = Board[i, j];
                    foreach (var v in p.AllTypes)
                    {
                        bool isDown = p.Orient == Position.Orientation.Down;
                        int TypeHandle = v.GrHandle + (isDown ? Types * 2 : 0);
                        int GrHandle = ImageLoader.PieceHandler[TypeHandle / Types, TypeHandle % Types];
                        DX.DrawRotaGraph(i * PX + PX / 2, j * PY + PY / 2, 1D, GetRect(p.Orient), GrHandle, DX.TRUE);
                        DX.DrawString(i*PX, j*PY, string.Format("{0}", p.Number), DX.GetColor(255,0,0));
                    }
                }

            if (PlayerStock[0].Count > 0)
            {
                for (int i = 0; i < PlayerStock[0].Count; i++)
                {
                    Piece p = PlayerStock[0][i];
                    foreach (var v in p.AllTypes)
                    {
                        bool isDown = p.Orient == Position.Orientation.Down;
                        int TypeHandle = v.GrHandle + (isDown ? Types * 2 : 0);
                        int GrHandle = ImageLoader.PieceHandler[TypeHandle / Types, TypeHandle % Types];
                        DX.DrawRotaGraph((9+i) * PX + PX / 2, PY / 2, 1D, GetRect(p.Orient), GrHandle, DX.TRUE);
                        DX.DrawString((9+i) * PX, 0, string.Format("{0}", p.Number), DX.GetColor(255, 0, 0));
                    }
                }
            }

            if (ClickedList.Count > 0)
            {
                foreach (var v in ClickedList)
                {
                    DX.DrawGraph(v.X * PX, v.Y * PY, ImageLoader.PieceOverrideHandler[0, 0], DX.TRUE);
                }
            }

            if (mx > 0 && ClickedList.Count == 0)
            {
                int rx = mx / ShogiEnvironment.Piece.SizeX;
                int ry = my / ShogiEnvironment.Piece.SizeY;
                string[] state = Board[rx, ry].ToString().Split('\n');
                DxLibProxy.DrawWideableBox(px, py, 180, 40 + (state.Length-1) * FontSize, DX.GetColor(0, 0, 0), DX.GetColor(255, 255, 255));
                DxLibProxy.DrawSplitString(px + 20, py + 20, state, DX.GetColor(0, 0, 0), FontSize);
            }

            if (IsConted)
            {
                int XS = 400;
                int YS = 100;
                int wid = 80;
                DxLibProxy.DrawWideableBox(X * PX / 2 - XS / 2, Y * PY / 2 - YS / 2, XS, YS, DX.GetColor(0, 0, 0), DX.GetColor(255, 255, 255));
                DX.DrawString(X*PX/2-(XS-wid)/2, PY*Y/2-8, "矛盾する駒が吹き飛びました。", DX.GetColor(0,0,0));

                IsConted = --ContedCounter != 0;
            }
        }

        private Boolean IsConted = false;
        private int ContedCounter;
        private List<int> DisappearNumbers;

        public void EnqueueContradiction(int num)
        {
            IsConted = true;
            ContedCounter = 128;
            DisappearNumbers.Add(num);
        }

        //private Methods

        private void ConvertToRealValue(Piece pc, ref int x, ref int y)
        {
            double rect = GetRect(pc.Orient);
            int dx = (int)Math.Round(Math.Cos(rect) * x - Math.Sin(rect) * y);
            int dy = (int)Math.Round(Math.Sin(rect) * x + Math.Cos(rect) * y);
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
            return Rect.Contains(new Position(x, y, Position.Orientation.None)) && (Board[x, y].IsEmpty() || Board[x, y].Pos.Orient != pc.Pos.Orient);
        }

        private int GetPlayerNumber(Position.Orientation orient)
        {
            switch (orient)
            {
                case Position.Orientation.Up: return 0;
                case Position.Orientation.Down: return 1;
            }
            return -1;
        }

        private void Move(int frX, int frY, int toX, int toY)
        {
            Board[frX, frY].MovedAndRemove(this, new Position(toX, toY, Position.Orientation.None));

            Piece from = Board[frX, frY];
            Board[frX, frY] = new Piece(-1);
            Board[frX, frY].Pos = new Position(frX, frY, Position.Orientation.None);
            Piece to = Board[toX, toY];
            if (to.Orient != Position.Orientation.None)
            {
                PlayerStock[0].Add(to);
                //TODO
            }
            Board[toX, toY] = from;
            from.X = toX;
            from.Y = toY;

            PlayerHands.Add(new Hand() { FromX = frX, FromY = frY, ToX = toX, ToY = toY });

            //収束処理
            CheckConvergence(Board[toX, toY]);
        }

        private void CheckConvergence(Piece p)
        {
            if (p.CheckConvergence(this)) //収束したら
            {
                Console.WriteLine("No.{0}が{1}に収束", p.Number, p.SingleType);
                // PC[PlayerNum,]++;
                int PlayerNum = GetPlayerNumber(p.Orient);
                int PieceNum = p.SingleType.GrHandle % CountPlys.Length;
                if(++PieceCounters[PlayerNum,PieceNum] >= CountPlys[PieceNum])
                {
                    List<Piece> list = new List<Piece>();

                    Console.WriteLine("矛盾調査開始");
                    foreach (var v in Board)
                    {
                        if (v.SingleType != p.SingleType && v.Orient == p.Orient) //同じプレイヤーのやつ
                        {
                            Console.WriteLine("\tNo.{0}を調査中", v.Number);
                            if (v.RemoveConvergencedType(this, p.SingleType))
                            {
                                list.Add(v);
                            }
                        }
                    }
                    Console.WriteLine("持ち駒を調査中");
                    foreach (var q in PlayerStock)
                    {
                        foreach (var v in q)
                        {
                            if (v.SingleType != p.SingleType && v.Orient == p.Orient) //同じプレイヤーのやつ
                            {
                                Console.WriteLine("\tNo.{0}を調査中", v.Number);
                                if (v.RemoveConvergencedType(this, p.SingleType))
                                {
                                    list.Add(v);
                                }
                            }
                        }
                    }
                    Console.WriteLine("調査終了");
                    foreach (var v in list)
                    {
                        CheckConvergence(v);
                    }
                }

                foreach (var q in PlayerStock)
                {
                    List<Piece> rem = new List<Piece>();
                    foreach (var v in q)
                    {
                        if (v.Orient == Position.Orientation.None)
                        {
                            rem.Add(v);
                        }
                    }
                    foreach (var v in rem)
                    {
                        q.Remove(v);
                    }
                }
            }
        }

        private class Hand
        {
            public int FromX;
            public int FromY;
            public int ToX;
            public int ToY;
        }
    }
}
