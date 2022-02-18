using System;
using System.Collections.Generic;
using System.Linq;
using Vakor.ClobberGame.Lib.Boards.BoardConfigurations;
using Vakor.ClobberGame.Lib.Chips;

namespace Vakor.ClobberGame.Lib.Boards
{
    public class Board : IBoard
    {
        public IChip[,] BoardMatrix => _boardMatrix;

        public BoardConfiguration Configuration { get; set; }

        public IChip this[Coordinates chipCoordinates]
        {
            get
            {
                if (!CoordsAreWithinBoard(chipCoordinates))
                {
                    throw new IndexOutOfRangeException();
                }

                return _boardMatrix[chipCoordinates.X, chipCoordinates.Y];
            }

            set
            {
                if (!CoordsAreWithinBoard(chipCoordinates))
                {
                    throw new IndexOutOfRangeException();
                }

                _boardMatrix[chipCoordinates.X, chipCoordinates.Y] = value;
            }
        }

        private IChip[,] _boardMatrix;

        public Board()
        {
            Configuration = new BoardConfiguration();
        }

        public Board(BoardConfiguration boardConfiguration)
        {
            Configuration = boardConfiguration;
        }

        public Board(IChip[,] boardMatrix)
        {
            _boardMatrix = boardMatrix;
            Configuration = new BoardConfiguration
            {
                Height = boardMatrix.GetLength(0),
                Width = boardMatrix.GetLength(1)
            };
        }

        public void MoveChip(Coordinates sourceCoords, Coordinates destCoords)
        {
            if (!CoordsAreWithinBoard(sourceCoords) || !CoordsAreWithinBoard(destCoords))
            {
                throw new IndexOutOfRangeException();
            }

            if (this[sourceCoords] == null || this[destCoords] != null)
            {
                throw new ArgumentException(
                    $"You don't have any chips on {sourceCoords} or you have chip on {destCoords}");
            }

            (this[sourceCoords], this[destCoords]) = (this[destCoords], this[sourceCoords]);
        }

        public void DeleteChip(Coordinates chipCoords)
        {
            if (!CoordsAreWithinBoard(chipCoords))
            {
                throw new IndexOutOfRangeException();
            }

            if (this[chipCoords] is null)
            {
                throw new ArgumentException($"You don't have any chips on {chipCoords.ToString()}");
            }

            _boardMatrix[chipCoords.X, chipCoords.Y] = null;
        }

        public void InitDefaultBoard()
        {
            _boardMatrix = new IChip[Configuration.Height, Configuration.Width];
            IChip[] chips = {new Chip(ChipColor.White), new Chip(ChipColor.Black)};
            for (int i = 0; i < Configuration.Height; i++)
            {
                for (int j = 0; j < Configuration.Width; j++)
                {
                    IChip chip = chips[((i + j) % 2)];
                    _boardMatrix[i, j] = chip;
                }
            }
        }

        public int FindAvailableTurnsCount(ChipColor color,
            out List<(Coordinates start, Coordinates dest)> turnsCoordinates)
        {
            int turnsCount = 0;
            turnsCoordinates = new List<(Coordinates start, Coordinates dest)>();
            for (int i = 0; i < Configuration.Height; i++)
            {
                for (int j = 0; j < Configuration.Width; j++)
                {
                    if (_boardMatrix[i, j] != null && _boardMatrix[i, j].ChipColor == color)
                    {
                        if (i > 0 && _boardMatrix[i - 1, j] != null && _boardMatrix[i - 1, j].ChipColor != color)
                        {
                            turnsCount++;
                            turnsCoordinates.Add((new Coordinates(i, j), new Coordinates(i - 1, j)));
                        }

                        if (j > 0 && _boardMatrix[i, j - 1] != null && _boardMatrix[i, j - 1].ChipColor != color)
                        {
                            turnsCount++;
                            turnsCoordinates.Add((new Coordinates(i, j), new Coordinates(i, j - 1)));
                        }

                        if (i < Configuration.Height - 1 && _boardMatrix[i + 1, j] != null &&
                            _boardMatrix[i + 1, j].ChipColor != color)
                        {
                            turnsCount++;
                            turnsCoordinates.Add((new Coordinates(i, j), new Coordinates(i + 1, j)));
                        }

                        if (j < Configuration.Width - 1 && _boardMatrix[i, j + 1] != null &&
                            _boardMatrix[i, j + 1].ChipColor != color)
                        {
                            turnsCount++;
                            turnsCoordinates.Add((new Coordinates(i, j), new Coordinates(i, j + 1)));
                        }
                    }
                }
            }

            return turnsCount;
        }

        public override string ToString()
        {
            string returnStr = String.Empty;
            for (int i = 0; i < Configuration.Height; i++)
            {
                for (int j = 0; j < Configuration.Width; j++)
                {
                    if (_boardMatrix[i, j] is null)
                    {
                        returnStr += " O ";
                    }
                    else if (_boardMatrix[i, j].ChipColor == ChipColor.White)
                    {
                        returnStr += " W ";
                    }
                    else
                    {
                        returnStr += " B ";
                    }
                }

                returnStr += '\n';
            }

            return returnStr;
        }

        private bool CoordsAreWithinBoard(Coordinates coordinates)
        {
            if (Configuration.Height <= coordinates.X || coordinates.X < 0)
            {
                return false;
            }

            if (Configuration.Width <= coordinates.Y || coordinates.Y < 0)
            {
                return false;
            }

            return true;
        }

        public int GetEvaluation(ChipColor playerColor)
        {
            return
                PlayerTurnsCount(playerColor)
                - PlayerTurnsCount((ChipColor) (((int) playerColor + 1) % 2))
                ;
        }

        private int PlayerTurnsCount(ChipColor playerColor)
        {
            int turnsCount = 0;

            for (int i = 0; i < Configuration.Height; i++)
            {
                for (int j = 0; j < Configuration.Width; j++)
                {
                    if (_boardMatrix[i, j] != null && _boardMatrix[i, j].ChipColor == playerColor)
                    {
                        if (i > 0 && _boardMatrix[i - 1, j] != null && _boardMatrix[i - 1, j].ChipColor == playerColor)
                        {
                            turnsCount++;
                        }

                        if (j > 0 && _boardMatrix[i, j - 1] != null && _boardMatrix[i, j - 1].ChipColor == playerColor)
                        {
                            turnsCount++;
                        }

                        if (i < Configuration.Height - 1 && _boardMatrix[i + 1, j] != null &&
                            _boardMatrix[i + 1, j].ChipColor == playerColor)
                        {
                            turnsCount++;
                        }

                        if (j < Configuration.Width - 1 && _boardMatrix[i, j + 1] != null &&
                            _boardMatrix[i, j + 1].ChipColor == playerColor)
                        {
                            turnsCount++;
                        }
                    }
                }
            }

            return turnsCount;
        }

        public IGameState[] GetAllChildren(ChipColor color)
        {
            int childrenCount =
                FindAvailableTurnsCount(color, out List<(Coordinates start, Coordinates dest)> turnsCoordinates);

            IGameState[] children = new IGameState[childrenCount];
            for (int i = 0; i < childrenCount; i++)
            {
                IBoard child = Clone() as IBoard;
                child.DeleteChip(turnsCoordinates[i].dest);
                child.MoveChip(turnsCoordinates[i].start, turnsCoordinates[i].dest);
                children[i] = child;
            }

            return children;
        }

        public object Clone()
        {
            IBoard cloneBoard = new Board(_boardMatrix.Clone() as IChip[,]);
            cloneBoard.Configuration = Configuration;
            return cloneBoard;
        }
    }
}