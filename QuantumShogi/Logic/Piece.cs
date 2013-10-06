using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            }
            Number = number;
        }

        private List<int> VerticalPieces;

        /// <summary>
        /// 縦に並んだ駒を追加します。
        /// </summary>
        /// <param name="num"></param>
        public void AddPieceNumber(int num)
        {
            if (!VerticalPieces.Contains(num))
                VerticalPieces.Add(num);
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

        public Position Pos
        {
            set;
            get;
        }

        private int Number
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
                            {-1, -1},
                            {+0, -1},
                            {+1, -1},
                            {-1, +0},
                            {+1, +0},
                            {+0, +1},
                    });
                }), null);
                PrLance = new Type(((world, piece) =>
                {
                    return CreateMoveTo(world, piece, new int[,]{
                            {-1, -1},
                            {+0, -1},
                            {+1, -1},
                            {-1, +0},
                            {+1, +0},
                            {+0, +1},
                    });
                }), null);
                PrKnight = new Type(((world, piece) =>
                {
                    return CreateMoveTo(world, piece, new int[,]{
                            {-1, -1},
                            {+0, -1},
                            {+1, -1},
                            {-1, +0},
                            {+1, +0},
                            {+0, +1},
                    });
                }), null);
                PrSilver = new Type(((world, piece) =>
                {
                    return CreateMoveTo(world, piece, new int[,]{
                            {-1, -1},
                            {+0, -1},
                            {+1, -1},
                            {-1, +0},
                            {+1, +0},
                            {+0, +1},
                    });
                }), null);
                PrBishop = new Type(((world, piece) =>
                {
                    var q = Bishop.Moving(world, piece);
                    q.AddRange(CreateMoveTo(world, piece, new int[,]{
                        {+0, -1},
                        {-1, +0},
                        {+1, +0},
                        {+0, +1},
                    }));
                    return q;
                }), null);
                PrRook = new Type(((world, piece) =>
                {
                    var q = Rook.Moving(world, piece);
                    q.AddRange(CreateMoveTo(world, piece, new int[,]{
                        {+0, -1},
                        {-1, +0},
                        {+1, +0},
                        {+0, +1},
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
                        {-1, -2},
                        {+1, -2},
                    });
                }), PrKnight);
                Silver = new Type(((world, piece) =>
                {
                    return CreateMoveTo(world, piece, new int[,]{
                            {-1, -1},
                            {+0, -1},
                            {+1, -1},
                            {-1, +1},
                            {+1, +1},
                    });
                }), PrSilver);
                Gold = new Type(((world, piece) =>
                {
                    return CreateMoveTo(world, piece, new int[,]{
                            {-1, -1},
                            {+0, -1},
                            {+1, -1},
                            {-1, +0},
                            {+1, +0},
                            {+0, +1},
                    });
                }), null);
                King = new Type(((world, piece) =>
                {
                    return CreateMoveTo(world, piece, new int[,]{
                            {-1, -1},
                            {+0, -1},
                            {+1, -1},
                            {-1, +0},
                            {+1, +0},
                            {-1, +1},
                            {+0, +1},
                            {+1, +1},
                    });
                }), null);
                None = new Type(null, null, true);

                //TODO Lance, Bishop, Rook
                Lance = new Type(null, null);
                Bishop = new Type(null, null);
                Rook = new Type(null, null);

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

                Array = new Type[] { Pawn, Lance, Knight, Silver, Gold, Bishop, Rook, King };
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
