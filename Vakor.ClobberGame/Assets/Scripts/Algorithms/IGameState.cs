using System;
using Vakor.ClobberGame.Lib.Chips;

namespace Algorithms
{
    public interface IGameState : ICloneable
    {
        int Height { get; }
        int Width { get; }
        ChipColor this[int x, int y] { get; set; }
        ChipColor[,] StateMatrix { get; set; }
        int GetAvailableTurnsCount(ChipColor playerColor);
    }
}