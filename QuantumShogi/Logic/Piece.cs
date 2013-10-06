using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantumShogi.Util;
using QuantumShogi.Environment;

namespace QuantumShogi.Logic
{
    public class Piece
    {
        public Piece(int number)
        {
            bool IsNone = number < 0;
            AllTypes = new List<Type>();
            SingleType = Type.None;
            if (!IsNone)
            {
                AddAllTypes();
                VerticalPieces = new List<int>();
                AroundPieces = new List<int>();
            }
            Number = number;
        }

        private List<int> VerticalPieces;
        private List<int> AroundPieces;

        /// <summary>
        /// 縦に並んだ駒を追加します。
        /// </summary>
        /// <param name="num"></param>
        public void AddTwoPawnNumber(int num)
        {
            if (!VerticalPieces.Contains(num))
                VerticalPieces.Add(num);
        }

        /// <summary>
        /// 周りに来た駒を追加します。
        /// </summary>
        /// <param name="num"></param>
        public void AddAroundKingNumber(int num)
        {
            if (!AroundPieces.Contains(num))
                AroundPieces.Add(num);
        }

        /// <summary>
        /// 過去に二歩を許したことがあったかどうかを求めます。
        /// </summary>
        /// <param name="num">駒番号</param>
        /// <returns></returns>
        public bool IsAllowedVertical(int num)
        {
            return VerticalPieces.Contains(num);
        }

        /// <summary>
        /// 過去に王の周りに王が来ていたかどうか求めます
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool IsAllowedKing(int num)
        {
            return AroundPieces.Contains(num);
        }

        public override string ToString()
        {
            if (AllTypes.Count < 1) return "None\n";

            if (SingleType != Type.None) return string.Format("{0}\n", SingleType.ToString());

            double d = 1D / AllTypes.Count;
            string result = "";
            foreach (var v in AllTypes)
            {
                result += string.Format("[{0}]:{1,0:P4}\n", v.ToString(), d);
            }
            return result;
        }

        public Position Pos
        {
            set;
            get;
        }

        public int Number
        {
            set;
            get;
        }

        public int X
        {
            set { Pos.X = value; }
            get { return Pos.X; }
        }

        public int Y
        {
            set { Pos.Y = value; }
            get { return Pos.Y; }
        }

        public Position.Orientation Orient
        {
            set { Pos.Orient = value; }
            get { return Pos.Orient; }
        }

        public void AddAllTypes()
        {
            foreach (var v in Type.Array)
            {
                AllTypes.Add(v);
            }
        }

        public void RemoveType(Type t)
        {
            AllTypes.Remove(t);
        }

        public void MovedAndRemove(World world, Position pos)
        {
            List<Type> reml = new List<Type>();

            foreach (var v in AllTypes)
            {
                if (!v.Moving(world, this).Contains(pos))
                {
                    reml.Add(v);
                }
            }
            foreach (var v in reml)
            {
                AllTypes.Remove(v);
            }
        }

        public bool RemoveConvergencedType(World world, Type t)
        {
            if (AllTypes.Contains(t))
            {
                AllTypes.Remove(t);
                if (AllTypes.Count == 0)
                {
                    Orient = Position.Orientation.None;
                    world.EnqueueContradiction(Number);
                }
                return AllTypes.Count == 1;
            }
            return false;
        }

        public bool CheckConvergence(World world)
        {
            if (AllTypes.Count == 1 && SingleType == Type.None)
            {
                SingleType = AllTypes[0];
                return true;
            }
            return false;
        }

        public List<Type> AllTypes
        {
            set;
            get;
        }

        public Type SingleType
        {
            set;
            get;
        }

        public bool IsEmpty()
        {
            return AllTypes.Count == 0;
        }

        public class Type
        {
            public Type(Func<World, Piece, List<Position>> func, Type Promoted, bool Empty = false)
            {
                Moving = func;
                IsEmpty = Empty;
                PromotedTo = Promoted;
            }

            public Func<World, Piece, List<Position>> Moving
            {
                set;
                get;
            }

            public bool IsEmpty
            {
                set;
                get;
            }

            public Type PromotedTo
            {
                set;
                get;
            }

            public int GrHandle
            {
                set;
                get;
            }

            private string Name
            {
                set;
                get;
            }

            public override string ToString()
            {
                return Name;
            }

            public static readonly Type Pawn;
            public static readonly Type Lance;
            public static readonly Type Knight;
            public static readonly Type Silver;
            public static readonly Type Gold;
            public static readonly Type Bishop;
            public static readonly Type Rook;
            public static readonly Type King;

            public static readonly Type PrPawn;
            public static readonly Type PrLance;
            public static readonly Type PrKnight;
            public static readonly Type PrSilver;
            public static readonly Type PrGold;
            public static readonly Type PrBishop;
            public static readonly Type PrRook;
            public static readonly Type PrKing;

            public static readonly Type None;

            public static readonly Type[] Array;

            private static List<Position> newList()
            {
                return new List<Position>();
            }

            static Type()
            {
                PrPawn = new Type(((world, piece) =>
                {
                    return CreateMoveTo(world, piece, new int[,]{
                            {-1, -1}, {+0, -1}, {+1, -1},
                            {-1, +0},           {+1, +0},
                                      {+0, +1},
                    });
                }), null);
                PrLance  = new Type(PrPawn.Moving, null);
                PrKnight = new Type(PrPawn.Moving, null);
                PrSilver = new Type(PrPawn.Moving, null);
                PrBishop = new Type(((world, piece) =>
                {
                    var q = Bishop.Moving(world, piece);
                    q.AddRange(CreateMoveTo(world, piece, new int[,]{
                                {+0, -1},
                        {-1, +0},       {+1, +0},
                                {+0, +1},
                    }));
                    return q;
                }), null);
                PrRook = new Type(((world, piece) =>
                {
                    var q = Rook.Moving(world, piece);
                    q.AddRange(CreateMoveTo(world, piece, new int[,]{
                        {+0, -1},       {-1, +0},
                        
                        {+1, +0},       {+0, +1},
                    }));
                    return q;
                }), null);

                Pawn = new Type(((world, piece) =>
                {
                    return CreateMoveTo(world, piece, new int[,]{
                                {+0, -1},
                    });
                }), PrPawn);
                Knight = new Type(((world, piece) =>
                {
                    return CreateMoveTo(world, piece, new int[,]{
                        {-1, -2},       {+1, -2},
                    });
                }), PrKnight);
                Silver = new Type(((world, piece) =>
                {
                    return CreateMoveTo(world, piece, new int[,]{
                            {-1, -1}, {+0, -1}, {+1, -1},
                            
                            {-1, +1},           {+1, +1},
                    });
                }), PrSilver);
                Gold = new Type(PrPawn.Moving, null);
                King = new Type(((world, piece) =>
                {
                    return CreateMoveTo(world, piece, new int[,]{
                            {-1, -1}, {+0, -1}, {+1, -1},
                            {-1, +0},           {+1, +0},
                            {-1, +1}, {+0, +1}, {+1, +1},
                    });
                }), null);

                Lance = new Type(((world, piece) =>
                {
                    var v = newList();
                    for (int i = 1; world.GetMoveableTo(piece, 0, -i); i++)
                    {
                        v.Add(world.CreateNewPosition(piece, 0, -i));
                        if (!world.IsEmpty(piece, 0, -i)) break;
                    }
                    return v;
                }), PrLance);
                Bishop = new Type(((world, piece) =>
                {
                    var v = newList();
                    for (int i = 1; world.GetMoveableTo(piece, -i, -i); i++) { v.Add(world.CreateNewPosition(piece, -i, -i)); if (!world.IsEmpty(piece, -i, -i)) break; }
                    for (int i = 1; world.GetMoveableTo(piece, +i, -i); i++) { v.Add(world.CreateNewPosition(piece, +i, -i)); if (!world.IsEmpty(piece, +i, -i)) break; }
                    for (int i = 1; world.GetMoveableTo(piece, -i, +i); i++) { v.Add(world.CreateNewPosition(piece, -i, +i)); if (!world.IsEmpty(piece, -i, +i)) break; }
                    for (int i = 1; world.GetMoveableTo(piece, +i, +i); i++) { v.Add(world.CreateNewPosition(piece, +i, +i)); if (!world.IsEmpty(piece, +i, +i)) break; }
                    return v;
                }), PrBishop);
                Rook = new Type(((world, piece) =>
                {
                    var v = newList();
                    for (int i = 1; world.GetMoveableTo(piece, +0, -i); i++) { v.Add(world.CreateNewPosition(piece, +0, -i)); if (!world.IsEmpty(piece, +0, -i)) break; }
                    for (int i = 1; world.GetMoveableTo(piece, +0, +i); i++) { v.Add(world.CreateNewPosition(piece, +0, +i)); if (!world.IsEmpty(piece, +0, +i)) break; }
                    for (int i = 1; world.GetMoveableTo(piece, -i, +0); i++) { v.Add(world.CreateNewPosition(piece, -i, +0)); if (!world.IsEmpty(piece, -i, +0)) break; }
                    for (int i = 1; world.GetMoveableTo(piece, +i, +0); i++) { v.Add(world.CreateNewPosition(piece, +i, +0)); if (!world.IsEmpty(piece, +i, +0)) break; }
                    return v;
                }), PrRook);

                Func<World, Piece, List<Position>> def = ((World, piece) => new List<Position>());
                PrGold = new Type(def, null);
                PrKing = new Type(def, null);

                None = new Type(null, null, true);

                Pawn.GrHandle = 0;
                Lance.GrHandle = 1;
                Knight.GrHandle = 2;
                Silver.GrHandle = 3;
                Gold.GrHandle = 4;
                Bishop.GrHandle = 5;
                Rook.GrHandle = 6;
                King.GrHandle = 7;

                PrPawn.GrHandle = 8;
                PrLance.GrHandle = 9;
                PrKnight.GrHandle = 10;
                PrSilver.GrHandle = 11;
                //PrGold.GrHandle = 12;
                PrBishop.GrHandle = 13;
                PrRook.GrHandle = 14;
                //PrKing.GrHandle = 15;
                try
                {

                    string[] names = ShogiEnvironment.Piece.Names;
                    Pawn.Name = names[0];
                    Lance.Name = names[1];
                    Knight.Name = names[2];
                    Silver.Name = names[3];
                    Gold.Name = names[4];
                    Bishop.Name = names[5];
                    Rook.Name = names[6];
                    King.Name = names[7];
                    PrPawn.Name = names[8];
                    PrLance.Name = names[9];
                    PrKnight.Name = names[10];
                    PrSilver.Name = names[11];
                    PrGold.Name = names[12];
                    PrBishop.Name = names[13];
                    PrRook.Name = names[14];
                    PrKing.Name = names[15];
                    None.Name = "None";

                    Array = new Type[] { Pawn, Lance, Knight, Silver, Gold, Bishop, Rook, King };
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            static List<Position> CreateMoveTo(World world, Piece piece, int[,] param)
            {
                var v = newList();
                for (int i = 0; i < param.GetLength(0); i++)
                {
                    if (world.GetMoveableTo(piece, param[i, 0], param[i, 1]))
                    {
                        Position p = world.CreateNewPosition(piece, param[i, 0], param[i, 1]);
                        v.Add(p);
                    }
                }
                return v;
            }
        }
    }
}
