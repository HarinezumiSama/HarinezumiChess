﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace HarinezumiChess.Tests
{
    [TestFixture]
    public sealed class GameMoveTests
    {
        #region Constants and Fields

        private static readonly PieceType[] ValidPromotionArguments =
            ChessConstants.ValidPromotions.Concat(PieceType.None.AsArray()).ToArray();

        #endregion

        #region Tests

        [Test]
        public void TestConstruction()
        {
            for (var fromIndex = 0; fromIndex < ChessConstants.SquareCount; fromIndex++)
            {
                var from = new Square(fromIndex);

                for (var toIndex = 0; toIndex < ChessConstants.SquareCount; toIndex++)
                {
                    if (fromIndex == toIndex)
                    {
                        continue;
                    }

                    var to = new Square(toIndex);

                    var outerMove = new GameMove(from, to);
                    Assert.That(outerMove.From, Is.EqualTo(from));
                    Assert.That(outerMove.To, Is.EqualTo(to));
                    Assert.That(outerMove.PromotionResult, Is.EqualTo(PieceType.None));

                    foreach (var promotion in ValidPromotionArguments)
                    {
                        var innerMove = new GameMove(from, to, promotion);
                        Assert.That(innerMove.From, Is.EqualTo(from));
                        Assert.That(innerMove.To, Is.EqualTo(to));
                        Assert.That(innerMove.PromotionResult, Is.EqualTo(promotion));
                    }
                }
            }
        }

        [Test]
        [TestCaseSource(typeof(FromStringNotationCases))]
        public void TestFromStringNotation(
            string input,
            Square expectedFrom,
            Square expectedTo,
            PieceType expectedPromotionResult)
        {
            foreach (var useExplicitMethod in new[] { false, true })
            {
                var move = useExplicitMethod ? GameMove.FromStringNotation(input) : input;

                Assert.That(move, Is.Not.Null);
                Assert.That(move.From, Is.EqualTo(expectedFrom));
                Assert.That(move.To, Is.EqualTo(expectedTo));
                Assert.That(move.PromotionResult, Is.EqualTo(expectedPromotionResult));
            }
        }

        [Test]
        public void TestFromStringNotationNegativeCases()
        {
            //// ReSharper disable once AssignNullToNotNullAttribute - Negative test case
            Assert.That(
                (TestDelegate)(() => GameMove.FromStringNotation(null)),
                Throws.TypeOf<ArgumentNullException>());
            Assert.That(
                (TestDelegate)(() => GameMove.FromStringNotation("a2a1=q")),
                Throws.TypeOf<ArgumentException>());
            Assert.That(
                (TestDelegate)(() => GameMove.FromStringNotation("a2-a1Q")),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void TestOperatorFromStringNegativeCases()
        {
            Assert.That(() => (GameMove)(string)null, Throws.TypeOf<ArgumentNullException>());
            Assert.That(() => (GameMove)"a2a1=q", Throws.TypeOf<ArgumentException>());
            Assert.That(() => (GameMove)"a2-a1Q", Throws.TypeOf<ArgumentException>());
        }

        [Test]
        [TestCaseSource(typeof(EqualityCases))]
        public void TestEquality(string firstMoveStringNotation, GameMove second, bool expectedEquals)
        {
            var first = GameMove.FromStringNotation(firstMoveStringNotation);

            Assert.That(first, Is.Not.Null);
            Assert.That(second, Is.Not.Null);

            Assert.That(first.Equals(second), Is.EqualTo(expectedEquals));
            Assert.That(second.Equals(first), Is.EqualTo(expectedEquals));
            Assert.That(Equals(first, second), Is.EqualTo(expectedEquals));
            Assert.That(EqualityComparer<GameMove>.Default.Equals(first, second), Is.EqualTo(expectedEquals));
            Assert.That(first == second, Is.EqualTo(expectedEquals));
            Assert.That(first != second, Is.EqualTo(!expectedEquals));

            if (expectedEquals)
            {
                Assert.That(first.GetHashCode(), Is.EqualTo(second.GetHashCode()));
            }
        }

        #endregion

        #region FromStringNotationCases Class

        public sealed class FromStringNotationCases : IEnumerable<TestCaseData>
        {
            #region IEnumerable<TestCaseData> Members

            public IEnumerator<TestCaseData> GetEnumerator()
            {
                yield return new FromStringNotationCaseData("a1-c2", "a1", "c2", PieceType.None);
                yield return new FromStringNotationCaseData("a1c2", "a1", "c2", PieceType.None);

                yield return new FromStringNotationCaseData("c1-g5", "c1", "g5", PieceType.None);
                yield return new FromStringNotationCaseData("c1g5", "c1", "g5", PieceType.None);

                yield return new FromStringNotationCaseData("c7xd8=Q", "c7", "d8", PieceType.Queen);
                yield return new FromStringNotationCaseData("c7d8q", "c7", "d8", PieceType.Queen);

                yield return new FromStringNotationCaseData("h2xg1=R", "h2", "g1", PieceType.Rook);
                yield return new FromStringNotationCaseData("h2g1R", "h2", "g1", PieceType.Rook);

                yield return new FromStringNotationCaseData("A7-a8=B", "a7", "a8", PieceType.Bishop);
                yield return new FromStringNotationCaseData("A7a8B", "a7", "a8", PieceType.Bishop);

                yield return new FromStringNotationCaseData("b2-B1=n", "b2", "b1", PieceType.Knight);
                yield return new FromStringNotationCaseData("b2B1n", "b2", "b1", PieceType.Knight);
            }

            #endregion

            #region IEnumerable Members

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            #endregion
        }

        #endregion

        #region EqualityCases Class

        public sealed class EqualityCases : IEnumerable<TestCaseData>
        {
            #region IEnumerable<TestCaseData> Members

            public IEnumerator<TestCaseData> GetEnumerator()
            {
                yield return new TestCaseData("a1-g7", new GameMove("a1", "g7"), true);
                yield return new TestCaseData("a2-a1", new GameMove("a2", "a1", PieceType.Queen), false);
                yield return new TestCaseData("c2xb1=B", new GameMove("c2", "b1", PieceType.Bishop), true);
                yield return new TestCaseData("c2xb1=B", new GameMove("c2", "b1", PieceType.Rook), false);
            }

            #endregion

            #region IEnumerable Members

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            #endregion
        }

        #endregion

        #region FromStringNotationCaseData Class

        private sealed class FromStringNotationCaseData : TestCaseData
        {
            #region Constructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="FromStringNotationCaseData"/> class.
            /// </summary>
            internal FromStringNotationCaseData(
                string input,
                Square expectedFrom,
                Square expectedTo,
                PieceType expectedPromotionResult)
                : base(input, expectedFrom, expectedTo, expectedPromotionResult)
            {
                // Nothing to do
            }

            #endregion
        }

        #endregion
    }
}