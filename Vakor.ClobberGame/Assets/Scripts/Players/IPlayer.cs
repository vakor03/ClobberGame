using Boards;
using GameControllers;
using Vakor.ClobberGame.Lib.Chips;

namespace Players
{
    public interface IPlayer
    {
        public ChipColor CurrentColor { get; set; }

        GameController.PassTurnDelegate PassTurn { get; set; }
        //public void MakeTurn(IBoard board);
        public bool Active { get; set; }
    }
}