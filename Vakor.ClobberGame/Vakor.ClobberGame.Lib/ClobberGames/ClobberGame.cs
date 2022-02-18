using System;
using System.Linq;
using Vakor.ClobberGame.Lib.Boards;
using Vakor.ClobberGame.Lib.Chips;
using Vakor.ClobberGame.Lib.Players;

namespace Vakor.ClobberGame.Lib.ClobberGames
{
    public class ClobberGame:IClobberGame
    {
        public bool GameIsFinished => _gameIsFinished;

        private IBoard _gameBoard;
        private bool _gameIsFinished = true;
        private Player[] _players;
        private int _currentId;

        public int White { get; set; }
        public int Black { get; set; }

        public ClobberGame()
        {
            _players = new Player[]{new RealPlayer(ChipColor.White), new AIPlayer(ChipColor.Black)};
            _gameBoard = new Board();
        }

        public void NewGame()
        {
            _gameIsFinished = false;
            _gameBoard.InitDefaultBoard();
            _currentId = _players[0].PlayerColor == ChipColor.White ? 0 : 1;
        }

        public void MakeTurn()
        {
            if (_players[_currentId].CanMakeTurn(_gameBoard))
            {
                //Console.WriteLine($"\n {_players[_currentId].PlayerColor} turn");
                _players[_currentId].MakeTurn(_gameBoard);
                _currentId = (_currentId + 1) % 2;
                
                //Console.Clear();
                //Console.WriteLine(_gameBoard);
            }
            else
            {
                _gameIsFinished = true;
            }
        }

        public void StartGame()
        {
            if (!_gameIsFinished)
            {
                //Console.Clear();
                //Console.WriteLine(_gameBoard);
                
                while (!_gameIsFinished)
                {
                    MakeTurn();
                }

                if (_currentId == 0)
                {
                    Black += 1;
                }
                else
                {
                    White += 1;
                }

                //Console.WriteLine($"{_players[(_currentId+1)%2].PlayerColor} player win");
            }
        }
    }
}