using System;
using Vakor.ClobberGame.Lib.ClobberGames;

namespace Vakor.ClobberGame.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IClobberGame clobberGame = new Lib.ClobberGames.ClobberGame();
            
            for (int i = 0; i < 100; i++)
            {
                clobberGame.NewGame();
                clobberGame.StartGame();
            }
            Console.WriteLine($"White {clobberGame.White}, Black: {clobberGame.Black}");
        }
    }
}