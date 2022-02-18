using System.Collections.Generic;
using Vakor.ClobberGame.Lib.Chips;

namespace Vakor.ClobberGame.Lib.Boards
{
    public interface IGameState 
    {
        public int GetEvaluation(ChipColor playerColor);
        public IGameState[] GetAllChildren(ChipColor color);
    }
}