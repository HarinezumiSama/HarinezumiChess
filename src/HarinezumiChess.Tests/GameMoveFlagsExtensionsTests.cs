using System;
using System.Linq;
using NUnit.Framework;

namespace HarinezumiChess.Tests
{
    [TestFixture]
    public sealed class GameMoveFlagsExtensionsTests
    {
        #region Tests

        [Test]
        [TestCase(GameMoveFlags.None, GameMoveFlags.None, false)]
        [TestCase(GameMoveFlags.PawnPromotion, GameMoveFlags.None, false)]
        [TestCase(GameMoveFlags.RegularCapture, GameMoveFlags.None, false)]
        [TestCase(GameMoveFlags.EnPassantCapture, GameMoveFlags.None, false)]
        [TestCase(GameMoveFlags.KingCastling, GameMoveFlags.None, false)]
        [TestCase(GameMoveFlags.None, GameMoveFlags.PawnPromotion, false)]
        [TestCase(GameMoveFlags.None, GameMoveFlags.RegularCapture, false)]
        [TestCase(GameMoveFlags.None, GameMoveFlags.EnPassantCapture, false)]
        [TestCase(GameMoveFlags.None, GameMoveFlags.KingCastling, false)]
        [TestCase(GameMoveFlags.PawnPromotion, GameMoveFlags.PawnPromotion, true)]
        [TestCase(GameMoveFlags.PawnPromotion, GameMoveFlags.RegularCapture, false)]
        [TestCase(GameMoveFlags.PawnPromotion, GameMoveFlags.EnPassantCapture, false)]
        [TestCase(GameMoveFlags.PawnPromotion, GameMoveFlags.KingCastling, false)]
        [TestCase(
            GameMoveFlags.PawnPromotion | GameMoveFlags.RegularCapture,
            GameMoveFlags.PawnPromotion, true)]
        [TestCase(
            GameMoveFlags.PawnPromotion | GameMoveFlags.RegularCapture,
            GameMoveFlags.RegularCapture, true)]
        [TestCase(
            GameMoveFlags.PawnPromotion | GameMoveFlags.RegularCapture,
            GameMoveFlags.EnPassantCapture, false)]
        [TestCase(
            GameMoveFlags.PawnPromotion | GameMoveFlags.RegularCapture,
            GameMoveFlags.KingCastling, false)]
        public void TestIsAnySet(GameMoveFlags value, GameMoveFlags flagsToCheck, bool expectedResult)
            => Assert.That(value.IsAnySet(flagsToCheck), Is.EqualTo(expectedResult));

        [Test]
        [TestCase(GameMoveFlags.None, false)]
        [TestCase(GameMoveFlags.PawnPromotion, true)]
        [TestCase(GameMoveFlags.RegularCapture, false)]
        [TestCase(GameMoveFlags.EnPassantCapture, false)]
        [TestCase(GameMoveFlags.KingCastling, false)]
        [TestCase(GameMoveFlags.PawnPromotion | GameMoveFlags.RegularCapture, true)]
        public void TestIsPawnPromotion(GameMoveFlags value, bool expectedResult)
            => Assert.That(value.IsPawnPromotion(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase(GameMoveFlags.None, false)]
        [TestCase(GameMoveFlags.PawnPromotion, false)]
        [TestCase(GameMoveFlags.RegularCapture, true)]
        [TestCase(GameMoveFlags.EnPassantCapture, false)]
        [TestCase(GameMoveFlags.KingCastling, false)]
        [TestCase(GameMoveFlags.PawnPromotion | GameMoveFlags.RegularCapture, true)]
        public void TestIsRegularCapture(GameMoveFlags value, bool expectedResult)
            => Assert.That(value.IsRegularCapture(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase(GameMoveFlags.None, false)]
        [TestCase(GameMoveFlags.PawnPromotion, false)]
        [TestCase(GameMoveFlags.RegularCapture, false)]
        [TestCase(GameMoveFlags.EnPassantCapture, true)]
        [TestCase(GameMoveFlags.KingCastling, false)]
        [TestCase(GameMoveFlags.PawnPromotion | GameMoveFlags.RegularCapture, false)]
        public void TestIsEnPassantCapture(GameMoveFlags value, bool expectedResult)
            => Assert.That(value.IsEnPassantCapture(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase(GameMoveFlags.None, false)]
        [TestCase(GameMoveFlags.PawnPromotion, false)]
        [TestCase(GameMoveFlags.RegularCapture, true)]
        [TestCase(GameMoveFlags.EnPassantCapture, true)]
        [TestCase(GameMoveFlags.KingCastling, false)]
        [TestCase(GameMoveFlags.PawnPromotion | GameMoveFlags.RegularCapture, true)]
        public void TestIsAnyCapture(GameMoveFlags value, bool expectedResult)
            => Assert.That(value.IsAnyCapture(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase(GameMoveFlags.None, false)]
        [TestCase(GameMoveFlags.PawnPromotion, false)]
        [TestCase(GameMoveFlags.RegularCapture, false)]
        [TestCase(GameMoveFlags.EnPassantCapture, false)]
        [TestCase(GameMoveFlags.KingCastling, true)]
        [TestCase(GameMoveFlags.PawnPromotion | GameMoveFlags.RegularCapture, false)]
        public void TestIsKingCastling(GameMoveFlags value, bool expectedResult)
            => Assert.That(value.IsKingCastling(), Is.EqualTo(expectedResult));

        #endregion
    }
}