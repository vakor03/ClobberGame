﻿namespace Vakor.ClobberGame.Lib
{
    public struct Coordinates
    {
        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return $"{{{X};{Y}}}";
        }
    }
}