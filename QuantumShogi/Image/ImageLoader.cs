using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using QuantumShogi.Environment;

namespace QuantumShogi.Image
{
    public class ImageLoader
    {
        public static int[,] PieceHandler;
        public static int[,] PieceOverrideHandler;
        public static int BoardHandler;
        public static int LoadingHandler;

        static ImageLoader()
        {
            LoadingHandler = DX.LoadGraph(@"img\Loading.png");
        }

        public static void ShowLoading()
        {
            DX.DrawGraph(0, 0, LoadingHandler, DX.TRUE);
        }

        public static void Load()
        {
            BoardHandler = DX.LoadGraph(@"img\Board.png");
            PieceHandler = new int[4, 8];
            PieceOverrideHandler = new int[2, 8];
            //DX.LoadDivGraph(@"img\Piece0.png", ShogiEnvironment.Player.Count*ShogiEnvironment.Piece.Count*2, ShogiEnvironment.Piece.Count, ShogiEnvironment.Player.Count*2, 64, 64, out PieceHandler[0,0]);
            DX.LoadDivGraph(@"img\Piece0.png", 32, 8, 4, 64, 64, out PieceHandler[0, 0]);
            DX.LoadDivGraph(@"img\Override.png", 16, 8, 2, 64, 64, out PieceOverrideHandler[0, 0]);
        }
    }
}
