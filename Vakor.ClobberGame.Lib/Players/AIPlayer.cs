using System;
using System.Collections.Generic;
using System.Linq;
using Vakor.ClobberGame.Lib.Boards;
using Vakor.ClobberGame.Lib.Chips;

namespace Vakor.ClobberGame.Lib.Players
{
    public class AIPlayer : Player
    {
        public AIPlayer(ChipColor playerColor) : base(playerColor)
        {
        }

        public override void MakeTurn(IBoard board)
        {
            (Coordinates start, Coordinates dest) bestTurn = FindBestTurn(board);

            board.DeleteChip(bestTurn.dest);
            board.MoveChip(bestTurn.start, bestTurn.dest);
        }

        private (Coordinates start, Coordinates dest) FindBestTurn(IBoard board)
        {
            board.FindAvailableTurnsCount(PlayerColor, out List<(Coordinates start, Coordinates dest)> turnsCoords);
            int maxValue = int.MinValue;
            (Coordinates start, Coordinates dest) bestTurn = (new Coordinates(), new Coordinates());

            for (var i = 0; i < turnsCoords.Count; i++)
            {
                IBoard childBoard = (IBoard) board.Clone();
                childBoard.DeleteChip(turnsCoords[i].dest);
                childBoard.MoveChip(turnsCoords[i].start, turnsCoords[i].dest);

                int value = Minimax(childBoard, childBoard.Configuration.RecursionDepth, int.MinValue, int.MaxValue,
                    false, (ChipColor) (((int) PlayerColor + 1) % 2));
                if (value > maxValue)
                {
                    maxValue = value;
                    bestTurn = turnsCoords[i];
                }
            }

            if (maxValue == int.MinValue)
            {
                throw new ArgumentException();
            }

            return bestTurn;
        }

        private int Minimax(IGameState state, int depth, int alpha, int beta, bool maximizingPlayer,
            ChipColor currentColor)
        {
            IGameState[] allChildren;
            if (depth == 0)
            {
                return state.GetEvaluation(PlayerColor);
            }

            allChildren = state.GetAllChildren(currentColor);
            if (allChildren.Length == 0)
            {
                return state.GetEvaluation(currentColor);
            }

            //Console.WriteLine();
            if (maximizingPlayer)
            {
                int maxEvaluation = int.MinValue;
                foreach (IGameState child in allChildren)
                {
                    int evaluation = Minimax(child, depth - 1, alpha, beta, false,
                        (ChipColor) (((int) PlayerColor + 1) % 2));
                    maxEvaluation = Math.Max(maxEvaluation, evaluation);
                    alpha = Math.Max(alpha, evaluation);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                //Console.Write($"+{maxEvaluation}");
                return maxEvaluation;
            }
            else
            {
                int minEvaluation = int.MaxValue;
                foreach (var child in allChildren)
                {
                    int evaluation = Minimax(child, depth - 1, alpha, beta, true, PlayerColor);
                    minEvaluation = Math.Min(minEvaluation, evaluation);
                    beta = Math.Min(beta, evaluation);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                //Console.Write($"-{minEvaluation}");
                return minEvaluation;
            }

            
        }
    }
}