using Vakor.ClobberGame.Lib.Chips;

namespace Algorithms
{
    public class GameState : IGameState
    {
        public GameState(ChipColor[,] stateMatrix)
        {
            StateMatrix = stateMatrix;
        }

        public int Height => StateMatrix.GetLength(0);
        public int Width => StateMatrix.GetLength(1);

        public ChipColor this[int x, int y]
        {
            get => StateMatrix[x, y];
            set => StateMatrix[x, y] = value;
        }

        public ChipColor[,] StateMatrix { get; set; }

        public int GetAvailableTurnsCount(ChipColor playerColor)
        {
            int turnsCount = 0;

            ChipColor opponentColor = (ChipColor) (((int) playerColor + 1) % 2);

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (this[i, j] == playerColor)
                    {
                        if (i > 0 && this[i - 1, j] == opponentColor)
                        {
                            turnsCount++;
                        }

                        if (j > 0 && this[i, j - 1] == opponentColor)
                        {
                            turnsCount++;
                        }

                        if (i < Height - 1 && this[i + 1, j] == opponentColor)
                        {
                            turnsCount++;
                        }

                        if (j < Width - 1 && this[i, j + 1] == opponentColor)
                        {
                            turnsCount++;
                        }
                    }
                }
            }

            return turnsCount;
        }


        public object Clone()
        {
            return new GameState(StateMatrix.Clone() as ChipColor[,]);
        }
    }
}