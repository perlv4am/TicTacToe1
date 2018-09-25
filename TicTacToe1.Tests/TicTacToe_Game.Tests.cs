using System;
using NUnit.Framework;
using TicTacToe1;

namespace TicTacToe1.Tests
{
    [TestFixture]
    public class TicTacToeGameTests
    {
        [Test]
        [TestCase(1, 1)]
        public void AddCross_WhenCalled_CrossAdded(int x, int y)
        {
            //Arrange
            TicTacToe_Game game = new TicTacToe_Game(dimension: 3);
            var xy = new[] {x, y};

            //Act 
            var result = game.AddCross(Coordinates: xy);

            //Assert
            Assert.That((result==1));

        }
    }
}
