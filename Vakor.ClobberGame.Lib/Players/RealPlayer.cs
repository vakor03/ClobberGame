using System;
using System.Collections.Generic;
using Vakor.ClobberGame.Lib.Boards;
using Vakor.ClobberGame.Lib.Chips;

namespace Vakor.ClobberGame.Lib.Players
{
    public class RealPlayer:Player
    {
        public RealPlayer(ChipColor playerColor) : base(playerColor)
        {
        }

        public override void MakeTurn(IBoard board)
        {
            board.FindAvailableTurnsCount(PlayerColor, out List<(Coordinates start, Coordinates dest)> turnsCoords);
            

            Random random = new Random();
            int i = random.Next(0, turnsCoords.Count);
                
           board.DeleteChip(turnsCoords[i].dest);
            board.MoveChip(turnsCoords[i].start, turnsCoords[i].dest);
            // var numbers = Console.ReadLine()?.Split().Select(int.Parse).ToArray();
            // Coordinates startCoordinates = new Coordinates(numbers[0], numbers[1]);
            // Coordinates destCoordinates = new Coordinates(numbers[2], numbers[3]);
            //
            //
            // while (!CouldMakeTurn(board, startCoordinates, destCoordinates))
            // {
            //     Console.WriteLine("Try another turn");
            //     numbers = Console.ReadLine()?.Split().Select(int.Parse).ToArray();
            //     startCoordinates = new Coordinates(numbers[0], numbers[1]);
            //     destCoordinates = new Coordinates(numbers[2], numbers[3]);
            // }
            // board.DeleteChip(destCoordinates);
            // board.MoveChip(startCoordinates, destCoordinates);
        }
        
        private bool CouldMakeTurn(IBoard board, Coordinates startCoordinates, Coordinates destCoordinates)
        {
            if (board[startCoordinates] is null || board[destCoordinates] is null)
            {
                return false;
            }
            
            if (Math.Abs(startCoordinates.X - destCoordinates.X) + Math.Abs(startCoordinates.Y - destCoordinates.Y) != 1)
            {
                return false;
            }

            if (board[startCoordinates].ChipColor != PlayerColor || board[destCoordinates].ChipColor == PlayerColor)
            {
                return false;
            }

            return true;
        }
    }
}