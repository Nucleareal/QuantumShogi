using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantumShogi.Environment;

namespace QuantumShogi.Logic
{
    public class BoardInitializer
    {
        static BoardInitializer()
        {
        }

        public static void Init(World world)
        {
            for (int i = 0; i < ShogiEnvironment.Board_X; i++)
                for (int j = 0; j < ShogiEnvironment.Board_Y; j++)
                    PlacePiece(world, i, j, Position.Orientation.None);

            PlaceHorizonal(world, 0, Position.Orientation.Down);
            PlaceHorizonal(world, 2, Position.Orientation.Down);
            PlaceHorizonal(world, 6, Position.Orientation.Up);
            PlaceHorizonal(world, 8, Position.Orientation.Up);
            PlaceOptional(world);
        }

        private static void PlaceHorizonal(World world, int line, Position.Orientation orient)
        {
            for (int i = 0; i < ShogiEnvironment.Board_X; i++)
            {
                PlacePiece(world, i, line, orient);
            }
        }

        private static void PlacePiece(World world, int x, int y, Position.Orientation orient)
        {
            world.PlacePiece(x, y, orient);
        }

        private static void PlaceOptional(World world)
        {
            PlacePiece(world, 1, 1, Position.Orientation.Down);
            PlacePiece(world, 7, 1, Position.Orientation.Down);
            PlacePiece(world, 1, 7, Position.Orientation.Up);
            PlacePiece(world, 7, 7, Position.Orientation.Up);
        }

        private static void RefreshPieces()
        {
        }
    }
}
