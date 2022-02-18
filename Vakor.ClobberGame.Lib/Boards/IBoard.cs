using System;
using System.Collections.Generic;
using Vakor.ClobberGame.Lib.Boards.BoardConfigurations;
using Vakor.ClobberGame.Lib.Chips;
using Vakor.ClobberGame.Lib.Players;

namespace Vakor.ClobberGame.Lib.Boards
{
    public interface IBoard : IGameState, ICloneable
    {
        BoardConfiguration Configuration { get; set; }
        IChip this[Coordinates chipCoordinates] { get; set; }
        IChip[,] BoardMatrix { get; }
        
        
        void MoveChip(Coordinates sourceCoords, Coordinates destCoords);
        void DeleteChip(Coordinates chipCoords);

        void InitDefaultBoard();

        int FindAvailableTurnsCount(ChipColor color,
            out List<(Coordinates start, Coordinates dest)> turnsCoordinates);
    }
}