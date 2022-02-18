using System.Threading.Tasks;
using Algorithms;
using Boards;
using Chips;
using GameControllers;
using UnityEngine;
using Vakor.ClobberGame.Lib.Chips;

namespace Players
{
    public class AIPlayer : MonoBehaviour, IPlayer
    {
        [SerializeField] private GameController gameController;
        public ChipColor CurrentColor { get; set; }
        public GameController.PassTurnDelegate PassTurn { get; set; }
        private IMiniMaxAlgorithm _miniMaxAlgorithm = new MiniMaxAlgorithm();

        public bool Active { get; set; }
        private bool _isSearching = false;
        [SerializeField] private Board board;


        private async void Update()
        {
            if (Active && !_isSearching)
            {
                Debug.Log("Turn to AI");
                _isSearching = true;
                
                (Vector2Int start, Vector2Int dest) bestTurn = await Task.Run(() =>
                    _miniMaxAlgorithm.FindBestTurn(board.GenerateGameState(), CurrentColor,
                        gameController.CurrentDifficulty));
                
                Chip currentChip = board[bestTurn.start];
                bool turnResult = gameController.TryMakeTurn(currentChip, bestTurn.dest);
                currentChip.transform.position =
                    new Vector3(currentChip.CurrentPosition.x, currentChip.CurrentPosition.y, 0);
                _isSearching = false;
                if (turnResult)
                {
                    PassTurn();
                }
            }
        }
    }
}