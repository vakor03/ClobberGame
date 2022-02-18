using System.Collections.Generic;
using Algorithms;
using Chips;
using UnityEngine;
using Vakor.ClobberGame.Lib.Boards.BoardConfigurations;
using Vakor.ClobberGame.Lib.Chips;

namespace Boards
{
    public interface IBoard
    {
        BoardConfiguration Configuration { get; set; }
        Chip this[Vector2Int chipCoordinates] { get; set; }
        Chip[,] BoardMatrix { get; }
        
        
        void MoveChip(Chip chip, Vector2Int destPosition);
        void DeleteChip(Vector2Int chipCoords);

        void InitDefaultBoard();
        bool CoordsAreWithinBoard(Vector2Int coordinates);
        IGameState GenerateGameState();
    }
}