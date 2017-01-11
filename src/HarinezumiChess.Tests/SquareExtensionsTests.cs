﻿using System;
using System.Linq;
using NUnit.Framework;

namespace HarinezumiChess.Tests
{
    [TestFixture]
    public sealed class SquareExtensionsTests
    {
        #region Tests

        [Test]
        public void TestIsDark()
        {
            for (var squareIndex = 0; squareIndex < ChessConstants.SquareCount; squareIndex++)
            {
                var rank = squareIndex / 8;
                var file = squareIndex % 8;
                var expectedIsDark = (rank + file) % 2 == 0;

                var square = new Square(squareIndex);
                var actualIsDark = square.IsDark();

                Assert.That(
                    actualIsDark,
                    Is.EqualTo(expectedIsDark),
                    $@"Failed for {nameof(Square.SquareIndex)} = {square.SquareIndex}.");
            }
        }

        #endregion
    }
}