using Players;

namespace Vakor.ClobberGame.Lib.ClobberGames
{
    public interface IGameController
    {
        bool GameIsFinished { get; }
        IPlayer CurrentPlayer { get; }
        void NewGame();
        void PassTurn();
    }
}