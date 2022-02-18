using Vakor.ClobberGame.Lib.Boards;
using Vakor.ClobberGame.Lib.Chips;

namespace Vakor.ClobberGame.Lib.Players
{
    public abstract class Player
    {
        protected Player(ChipColor playerColor)
        {
            PlayerColor = playerColor;
        }

        public ChipColor PlayerColor;

        public abstract void MakeTurn(IBoard board);
        

        

        public bool CanMakeTurn(IBoard gameBoard)
        {
            if (gameBoard.FindAvailableTurnsCount(PlayerColor, out _) > 0)
            {
                return true;
            }

            return false;
        }
    }
}