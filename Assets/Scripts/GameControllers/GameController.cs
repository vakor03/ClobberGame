using System;
using Algorithms;
using Boards;
using Chips;
using Players;
using UnityEngine;
using Vakor.ClobberGame.Lib.Chips;
using Vakor.ClobberGame.Lib.ClobberGames;

namespace GameControllers
{
    public class GameController : MonoBehaviour, IGameController
    {
        [SerializeField] private Board _gameBoard;
        [SerializeField] private RealPlayer realPlayer;
        [SerializeField] private AIPlayer aiPlayer;
        [SerializeField] private GameStatus gameStatus;
        [SerializeField] private GameObject newGameObject;

        public delegate void PassTurnDelegate();

        public GameObject whiteCell;
        public GameObject blackCell;
        public Chip whiteChip;
        public Chip blackChip;
        public bool GameIsFinished
        {
            get => _gameIsFinished;
            private set
            {
                _gameIsFinished = value;
                gameStatus.GameIsFinished = value;
                newGameObject.SetActive(value);
                
            }
        }

        public GameDifficulty CurrentDifficulty
        {
            get => _currentDifficulty;
            set
            {
                _currentDifficulty = value;
                gameStatus.GameDifficulty = _currentDifficulty;
            }
        }

        public IPlayer CurrentPlayer => _players[_currentId];

        private bool _gameIsFinished = true;
        private IPlayer[] _players;
        private int _currentId;
        private GameDifficulty _currentDifficulty = GameDifficulty.Easy;

        private void Start()
        {
            _players = new IPlayer[] {Instantiate(realPlayer), Instantiate(aiPlayer)};
            _players[0].CurrentColor = ChipColor.White;
            _players[1].CurrentColor = ChipColor.Black;
            _players[0].PassTurn += PassTurn;
            _players[1].PassTurn += PassTurn;
        }

        public void NewGame()
        {
            GameIsFinished = false;
            foreach (var child in _gameBoard.GetComponentsInChildren(typeof(Chip)))
            {
                Destroy(child.gameObject);
            }
            gameStatus.GameIsFinished = false;
            _gameBoard.InitDefaultBoard();
            _currentId = _players[0].CurrentColor == ChipColor.White ? 0 : 1;
            _players[0].Active = true;
            gameStatus.CurrentPlayer = _players[0];
            gameStatus.GameDifficulty = _currentDifficulty;
            
        }

        public void PassTurn()
        {
            _players[_currentId].Active = false;
            _currentId = (_currentId + 1) % 2;
            CheckGameForFinish();
            if (!_gameIsFinished)
            {
                _players[_currentId].Active = true;
                gameStatus.CurrentPlayer = _players[_currentId];
            }
        }

        public void ChangeDifficulty(float sliderValue)
        {
            CurrentDifficulty = (GameDifficulty) (int) sliderValue;
        }

        public bool TryMakeTurn(Chip chip, Vector2Int destPosition)
        {
            if (!_gameBoard.CoordsAreWithinBoard(destPosition))
            {
                return false;
            }

            if (Math.Abs(chip.CurrentPosition.x - destPosition.x) +
                Math.Abs(chip.CurrentPosition.y - destPosition.y) > 1)
            {
                return false;
            }

            if (_gameBoard[destPosition] == null)
            {
                return false;
            }

            if (_gameBoard[destPosition].ChipColor == chip.ChipColor)
            {
                return false;
            }

            _gameBoard.DeleteChip(destPosition);
            _gameBoard.MoveChip(chip, destPosition);
            return true;
        }

        private void CheckGameForFinish()
        {
            GameIsFinished = _gameBoard.GenerateGameState().GetAvailableTurnsCount(CurrentPlayer.CurrentColor) == 0;
        }
    }
}