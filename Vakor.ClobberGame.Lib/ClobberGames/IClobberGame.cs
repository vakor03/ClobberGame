namespace Vakor.ClobberGame.Lib.ClobberGames
{
    public interface IClobberGame
    {
        bool GameIsFinished { get; }
        void NewGame();
        void MakeTurn();
        void StartGame();
        public int White { get; set; }
        public int Black { get; set; }
    }
}