using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vakor.ClobberGame.Lib.Chips;

namespace Algorithms
{
    public class MiniMaxAlgorithm : IMiniMaxAlgorithm
    {
        private int _recursionDepth;
        private GameDifficulty _currentDifficulty;

        public (Vector2Int, Vector2Int) FindBestTurn(IGameState board, ChipColor currentColor,
            GameDifficulty currentDifficulty)
        {
            _currentDifficulty = currentDifficulty;
            SetRecursionDepth(currentDifficulty);
            int maxValue = int.MinValue;

            var allTurns = FindAllPossibleTurns(board, currentColor);
            ((int x, int y) coords, MoveDirection) bestTurn = allTurns[0];

            for (var i = 0; i < allTurns.Count; i++)
            {
                IGameState childState = (IGameState) board.Clone();
                MakeTurn(childState, allTurns[i], currentColor);

                int value = Minimax(childState, _recursionDepth, int.MinValue, int.MaxValue,
                    false, currentColor);
                if (value > maxValue)
                {
                    maxValue = value;
                    bestTurn = allTurns[i];
                }
            }

            if (maxValue == int.MinValue)
            {
                throw new ArgumentException();
            }


            Vector2Int destVector = FindDestVectorFromTurn(bestTurn);
            (Vector2Int, Vector2Int) bestTurnInVector =
                (new Vector2Int(bestTurn.coords.x, bestTurn.coords.y), destVector);
            return bestTurnInVector;
        }

        private int Minimax(IGameState state, int depth, int alpha, int beta, bool maximizingPlayer,
            ChipColor currentColor)
        {
            if (depth == 0)
            {
                return GetEvaluation(state, currentColor);
            }

            IGameState[] allChildren = GetAllChildren(state, currentColor);
            if (allChildren.Length == 0)
            {
                return GetEvaluation(state, currentColor);
            }

            if (maximizingPlayer)
            {
                int maxEvaluation = int.MinValue;
                foreach (IGameState child in allChildren)
                {
                    int evaluation = Minimax(child, depth - 1, alpha, beta, false,
                        currentColor);

                    maxEvaluation = Math.Max(maxEvaluation, evaluation);
                    alpha = Math.Max(alpha, evaluation);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                return maxEvaluation;
            }
            else
            {
                int minEvaluation = int.MaxValue;
                foreach (var child in allChildren)
                {
                    int evaluation = Minimax(child, depth - 1, alpha, beta, true,
                        currentColor);
                    minEvaluation = Math.Min(minEvaluation, evaluation);
                    beta = Math.Min(beta, evaluation);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                return minEvaluation;
            }
        }

        public int GetEvaluation(IGameState currentState, ChipColor playerColor)
        {
            int evaluation = 0;
            if (_currentDifficulty == GameDifficulty.Easy)
            {
                ChipColor opponentColor = (ChipColor) (((int) playerColor + 1) % 2);

                for (int i = 0; i < currentState.Height; i++)
                {
                    for (int j = 0; j < currentState.Width; j++)
                    {
                        if (currentState[i, j] == playerColor)
                        {
                            if (i > 0 && currentState[i - 1, j] == opponentColor)
                            {
                                evaluation++;
                            }else if (j > 0 && currentState[i, j - 1] == opponentColor)
                            {
                                evaluation++;
                            }else if (i < currentState.Height - 1 && currentState[i + 1, j] == opponentColor)
                            {
                                evaluation++;
                            }else if (j < currentState.Width - 1 && currentState[i, j + 1] == opponentColor)
                            {
                                evaluation++;
                            }
                        }
                    }
                }
            }
            else
            {
                evaluation = currentState.GetAvailableTurnsCount(playerColor);
            }


            return evaluation;
        }

        private ChipColor GetOpponentColor(ChipColor playerColor)
        {
            return (ChipColor) (((int) playerColor + 1) % 2);
        }

        public IGameState[] GetAllChildren(IGameState currentState, ChipColor color)
        {
            var possibleTurns = FindAllPossibleTurns(currentState, color);

            IGameState[] children = new IGameState[possibleTurns.Count];
            for (int i = 0; i < possibleTurns.Count; i++)
            {
                IGameState child = currentState.Clone() as IGameState;
                MakeTurn(child, possibleTurns[i], color);
                children[i] = child;
            }

            return children;
        }

        private void MakeTurn(IGameState gameState, ((int x, int y) coords, MoveDirection) move, ChipColor color)
        {
            gameState[move.coords.x, move.coords.y] = ChipColor.NoChip;
            switch (move.Item2)
            {
                case MoveDirection.Down:
                    gameState[move.coords.x + 1, move.coords.y] = color;
                    break;

                case MoveDirection.Up:
                    gameState[move.coords.x - 1, move.coords.y] = color;
                    break;

                case MoveDirection.Right:
                    gameState[move.coords.x, move.coords.y + 1] = color;
                    break;

                case MoveDirection.Left:
                    gameState[move.coords.x, move.coords.y - 1] = color;
                    break;
            }
        }

        private List<((int x, int y) coords, MoveDirection)> FindAllPossibleTurns(IGameState gameState,
            ChipColor playerColor)
        {
            List<((int, int), MoveDirection)> possibleTurns = new List<((int, int), MoveDirection)>();
            ChipColor opponentColor = GetOpponentColor(playerColor);

            for (int i = 0; i < gameState.Height; i++)
            {
                for (int j = 0; j < gameState.Width; j++)
                {
                    if (gameState[i, j] == playerColor)
                    {
                        if (i > 0 && gameState[i - 1, j] == opponentColor)
                        {
                            possibleTurns.Add(((i, j), MoveDirection.Up));
                        }

                        if (j > 0 && gameState[i, j - 1] == opponentColor)
                        {
                            possibleTurns.Add(((i, j), MoveDirection.Left));
                        }

                        if (i < gameState.Height - 1 && gameState[i + 1, j] == opponentColor)
                        {
                            possibleTurns.Add(((i, j), MoveDirection.Down));
                        }

                        if (j < gameState.Width - 1 && gameState[i, j + 1] == opponentColor)
                        {
                            possibleTurns.Add(((i, j), MoveDirection.Right));
                        }
                    }
                }
            }

            return possibleTurns;
        }

        private Vector2Int FindDestVectorFromTurn(((int x, int y) coords, MoveDirection) turn)
        {
            Vector2Int destVector = new Vector2Int();
            switch (turn.Item2)
            {
                case MoveDirection.Down:
                    destVector = new Vector2Int(turn.coords.x + 1, turn.coords.y);
                    break;

                case MoveDirection.Up:
                    destVector = new Vector2Int(turn.coords.x - 1, turn.coords.y);
                    break;

                case MoveDirection.Right:
                    destVector = new Vector2Int(turn.coords.x, turn.coords.y + 1);
                    break;

                case MoveDirection.Left:
                    destVector = new Vector2Int(turn.coords.x, turn.coords.y - 1);
                    break;
            }

            return destVector;
        }

        private void SetRecursionDepth(GameDifficulty gameDifficulty)
        {
            switch (gameDifficulty)
            {
                case GameDifficulty.Easy:
                    _recursionDepth = 0;
                    break;
                case GameDifficulty.Medium:
                    _recursionDepth = 2;
                    break;
                case GameDifficulty.Hard:
                    _recursionDepth = 4;
                    break;
            }
        }
    }
}