using UnityEngine;
using Vakor.ClobberGame.Lib.Chips;

namespace Algorithms
{
    public interface IMiniMaxAlgorithm
    {
        (Vector2Int, Vector2Int)FindBestTurn(IGameState board, ChipColor currentColor, GameDifficulty currentDifficulty);
        int GetEvaluation(IGameState currentState, ChipColor playerColor);
        
        IGameState[] GetAllChildren(IGameState currentState, ChipColor color);
    }
}