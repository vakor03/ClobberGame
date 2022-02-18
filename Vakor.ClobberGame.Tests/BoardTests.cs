using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vakor.ClobberGame.Lib;
using Vakor.ClobberGame.Lib.Boards;
using Vakor.ClobberGame.Lib.Chips;

namespace Vakor.ClobberGame.Tests
{
    [TestClass]
    public class BoardTests
    {
        private Board _board;

        [TestMethod]
        public void DeleteChipIncorrectCoordinatesTest()
        {
            Assert.ThrowsException<IndexOutOfRangeException>(() =>
                _board.DeleteChip(new Coordinates(_board.Configuration.Height, 0)));
            
            Assert.ThrowsException<IndexOutOfRangeException>(() =>
                _board.DeleteChip(new Coordinates(0, _board.Configuration.Width)));
        }

        [TestMethod]
        public void EvaluationTest()
        {
            IChip[] chips = {new Chip(ChipColor.White), new Chip(ChipColor.Black)};
            IBoard board = new Board(
                new IChip[,]
                {
                    {null, chips[1], chips[0], chips[1]},
                    {chips[0], chips[0], chips[1], chips[0]},
                    {chips[0], chips[1], chips[0], chips[1]},
                });
            int evaluation = board.GetEvaluation(ChipColor.Black);
            Assert.AreEqual(0, evaluation);
        }

        [TestInitialize]
        public void InitBoard()
        {
            _board = new Board();
        }
    }
}