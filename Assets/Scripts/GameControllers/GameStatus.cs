using System;
using Players;
using TMPro;
using UnityEngine;
using Vakor.ClobberGame.Lib.Chips;

namespace GameControllers
{
    public class GameStatus : MonoBehaviour
    {
        [SerializeField] private TMP_Text complexityTMP;
        [SerializeField] private TMP_Text currentPlayerTMP;
        [SerializeField] private TMP_Text gameIsFinishedTMP;
        public IPlayer CurrentPlayer
        {
            set
            {
                currentPlayerTMP.text = $"Current player: {value.CurrentColor}";
                _currentPlayer = value;
            }
        }

        private IPlayer _currentPlayer;

        public GameDifficulty GameDifficulty
        {
            set => complexityTMP.text = $"Current complexity: {value}";
        }

        public bool GameIsFinished
        {
            set =>gameIsFinishedTMP.text = value ? $"{_currentPlayer.CurrentColor} wins" : String.Empty;
        }
    }
}