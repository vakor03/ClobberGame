using System;
using Algorithms;
using Chips;
using GameControllers;
using UnityEngine;
using Vakor.ClobberGame.Lib.Boards.BoardConfigurations;
using Vakor.ClobberGame.Lib.Chips;

namespace Boards
{
    public class Board : MonoBehaviour, IBoard
    {
        [SerializeField] private GameController gameController;
        public Chip[,] BoardMatrix => _boardMatrix;

        public BoardConfiguration Configuration { get; set; }

        public Chip this[Vector2Int chipCoordinates]
        {
            get
            {
                if (!CoordsAreWithinBoard(chipCoordinates))
                {
                    throw new IndexOutOfRangeException();
                }

                return _boardMatrix[chipCoordinates.x, chipCoordinates.y];
            }

            set
            {
                if (!CoordsAreWithinBoard(chipCoordinates))
                {
                    throw new IndexOutOfRangeException();
                }

                _boardMatrix[chipCoordinates.x, chipCoordinates.y] = value;
            }
        }

        private Chip[,] _boardMatrix;

        public Board()
        {
            Configuration = new BoardConfiguration();
        }

        public Board(BoardConfiguration boardConfiguration)
        {
            Configuration = boardConfiguration;
        }

        public Board(Chip[,] boardMatrix)
        {
            _boardMatrix = boardMatrix;
            Configuration = new BoardConfiguration
            {
                Height = boardMatrix.GetLength(0),
                Width = boardMatrix.GetLength(1)
            };
        }

        private void Start()
        {
            InitDefaultBoard();
            DrawBoard();
        }

        public void MoveChip(Chip chip, Vector2Int destPosition)
        {
            if (!CoordsAreWithinBoard(destPosition))
            {
                throw new IndexOutOfRangeException();
            }

            if (this[destPosition] != null)
            {
                throw new ArgumentException(
                    $"You have chip on {destPosition}");
            }

            (this[chip.CurrentPosition], this[destPosition]) = (this[destPosition], this[chip.CurrentPosition]);
            chip.CurrentPosition = destPosition;
        }

        public void DeleteChip(Vector2Int chipCoords)
        {
            if (!CoordsAreWithinBoard(chipCoords))
            {
                throw new IndexOutOfRangeException();
            }

            if (this[chipCoords] is null)
            {
                throw new ArgumentException($"You don't have any chips on {chipCoords.ToString()}");
            }

            Destroy(_boardMatrix[chipCoords.x, chipCoords.y].gameObject);
            _boardMatrix[chipCoords.x, chipCoords.y] = null;
        }

        public void InitDefaultBoard()
        {
            _boardMatrix = new Chip[Configuration.Height, Configuration.Width];
            for (int i = 0; i < Configuration.Height; i++)
            {
                for (int j = 0; j < Configuration.Width; j++)
                {
                    Chip chip = Instantiate((j + i) % 2 == 0 ? gameController.whiteChip : gameController.blackChip,
                        new Vector3(i, j), Quaternion.identity, transform);;
                    chip.CurrentPosition = new Vector2Int(i, j);
                    _boardMatrix[i, j] = chip;
                }
            }
        }

        private void DrawBoard()
        {
            for (int i = 0; i < Configuration.Height; i++)
            {
                for (int j = 0; j < Configuration.Width; j++)
                {
                    Instantiate((j + i) % 2 == 0 ? gameController.whiteCell : gameController.blackCell,
                        new Vector3(i, j), Quaternion.identity, transform);
                }
            }
        }

        public bool CoordsAreWithinBoard(Vector2Int coordinates)
        {
            if (Configuration.Height <= coordinates.x || coordinates.x < 0)
            {
                return false;
            }

            if (Configuration.Width <= coordinates.y || coordinates.y < 0)
            {
                return false;
            }

            return true;
        }

        public IGameState GenerateGameState()
        {
            ChipColor[,] chipColors = new ChipColor[Configuration.Height, Configuration.Width];
            for (int i = 0; i < Configuration.Height; i++)
            {
                for (int j = 0; j < Configuration.Width; j++)
                {
                    chipColors[i, j] = _boardMatrix[i, j] is null ? ChipColor.NoChip : _boardMatrix[i, j].ChipColor;
                }
            }

            return new GameState(chipColors);
        }

        // public int FindAvailableTurnsCount(CurrentColor color,
        //     out List<(Vector2Int start, Vector2Int dest)> turnsCoordinates)
        // {
        //     int turnsCount = 0;
        //     turnsCoordinates = new List<(Vector2Int start, Vector2Int dest)>();
        //     for (int i = 0; i < Configuration.Height; i++)
        //     {
        //         for (int j = 0; j < Configuration.Width; j++)
        //         {
        //             if (_boardMatrix[i, j] != null && _boardMatrix[i, j].CurrentColor == color)
        //             {
        //                 if (i > 0 && _boardMatrix[i - 1, j] != null && _boardMatrix[i - 1, j].CurrentColor != color)
        //                 {
        //                     turnsCount++;
        //                     turnsCoordinates.Add((new Vector2Int(i, j), new Vector2Int(i - 1, j)));
        //                 }
        //
        //                 if (j > 0 && _boardMatrix[i, j - 1] != null && _boardMatrix[i, j - 1].CurrentColor != color)
        //                 {
        //                     turnsCount++;
        //                     turnsCoordinates.Add((new Vector2Int(i, j), new Vector2Int(i, j - 1)));
        //                 }
        //
        //                 if (i < Configuration.Height - 1 && _boardMatrix[i + 1, j] != null &&
        //                     _boardMatrix[i + 1, j].CurrentColor != color)
        //                 {
        //                     turnsCount++;
        //                     turnsCoordinates.Add((new Vector2Int(i, j), new Vector2Int(i + 1, j)));
        //                 }
        //
        //                 if (j < Configuration.Width - 1 && _boardMatrix[i, j + 1] != null &&
        //                     _boardMatrix[i, j + 1].CurrentColor != color)
        //                 {
        //                     turnsCount++;
        //                     turnsCoordinates.Add((new Vector2Int(i, j), new Vector2Int(i, j + 1)));
        //                 }
        //             }
        //         }
        //     }
        //
        //     return turnsCount;
        // }
    }
}