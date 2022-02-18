using System;
using Boards;
using Chips;
using GameControllers;
using UnityEngine;
using Vakor.ClobberGame.Lib.Chips;

namespace Players
{
    public class RealPlayer : MonoBehaviour, IPlayer
    {
        [SerializeField] private Camera camera;
        [SerializeField] private GameController gameController;
        
        private Chip _currentChip = null;
        public GameController.PassTurnDelegate PassTurn { get; set; }

        public ChipColor CurrentColor { get; set; }

        public bool Active { get; set; }

        public void TryMoveChip(Chip chip)
        {
            Vector3 destPosition = chip.transform.position;
            destPosition.x = (int) Math.Round(destPosition.x);
            destPosition.y = (int) Math.Round(destPosition.y);
            chip.transform.position = destPosition;
            
            Debug.Log($"{_currentChip.CurrentPosition.x}, {_currentChip.CurrentPosition.y}");
            bool turnIsOk = gameController.TryMakeTurn(chip, Vector2Int.RoundToInt(destPosition));
            chip.transform.position = new Vector3(chip.CurrentPosition.x, chip.CurrentPosition.y, 0);
            
            if (turnIsOk)
            {
                PassTurn();
            }
        }

        private void Update()
        {
            if (Active)
            {
                Vector3 cursorPosition = camera.ScreenToWorldPoint(Input.mousePosition);
                cursorPosition.z = 0;

                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Drag real player");
                    var collider = Physics2D.OverlapPoint(cursorPosition);
                    if (collider != null)
                    {
                        _currentChip = collider.GetComponent<Chip>();
                    }
                    if (_currentChip != null)
                    {
                        if (_currentChip.ChipColor != gameController.CurrentPlayer.CurrentColor)
                        {
                            _currentChip = null;
                        }
                    }
                }

                if (_currentChip != null)
                {
                    _currentChip.transform.position = cursorPosition;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    if (_currentChip != null)
                    {
                        TryMoveChip(_currentChip);
                    }

                    _currentChip = null;
                }
            }
        }
    }
}