﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace HarinezumiChess
{
    public struct Square : IEquatable<Square>
    {
        #region Constants and Fields

        private static readonly string[] StringRepresentations = Enumerable
            .Range(0, ChessConstants.SquareCount)
            .Select(
                squareIndex =>
                    new string(new[] { GetFileChar(GetFile(squareIndex)), GetRankChar(GetRank(squareIndex)) }))
            .ToArray();

        private static readonly Bitboard[] Bitboards = Enumerable
            .Range(0, ChessConstants.SquareCount)
            .Select(Bitboard.FromSquareIndex)
            .ToArray();

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Square"/> structure
        ///     using the specified square index.
        /// </summary>
        [DebuggerNonUserCode]
        public Square(int squareIndex)
        {
            EnsureValidSquareIndex(squareIndex);

            SquareIndex = squareIndex;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Square"/> structure
        ///     using the specified file and rank.
        /// </summary>
        [DebuggerNonUserCode]
        public Square(int file, int rank)
        {
            #region Argument Check

            if ((file & ~ChessConstants.MaxFileIndex) != 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(file),
                    file,
                    $@"The value is out of the valid range {ChessConstants.FileRange}.");
            }

            if ((rank & ~ChessConstants.MaxRankIndex) != 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(rank),
                    rank,
                    $@"The value is out of the valid range {ChessConstants.FileRange}.");
            }

            #endregion

            SquareIndex = (rank << 3) | file;
        }

        #endregion

        #region Public Properties

        public int SquareIndex
        {
            [DebuggerNonUserCode]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        public Bitboard Bitboard
        {
            [DebuggerStepThrough]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Bitboards[SquareIndex];
            }
        }

        public int File
        {
            [DebuggerStepThrough]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return GetFile(SquareIndex);
            }
        }

        public int Rank
        {
            [DebuggerStepThrough]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return GetRank(SquareIndex);
            }
        }

        public char FileChar
        {
            [DebuggerStepThrough]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return GetFileChar(File);
            }
        }

        public char RankChar
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [DebuggerStepThrough]
            get
            {
                return GetRankChar(Rank);
            }
        }

        #endregion

        #region Operators

        [DebuggerNonUserCode]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Square(string algebraicNotation) => FromAlgebraic(algebraicNotation);

        [DebuggerNonUserCode]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Square left, Square right) => Equals(left, right);

        [DebuggerNonUserCode]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Square left, Square right) => !(left == right);

        [DebuggerNonUserCode]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Square? operator +(Square left, SquareShift right)
        {
            var file = left.File + right.FileOffset;
            var rank = left.Rank + right.RankOffset;

            return (file & ~ChessConstants.MaxFileIndex) == 0 && (rank & ~ChessConstants.MaxRankIndex) == 0
                ? new Square(file, rank)
                : default(Square?);
        }

        #endregion

        #region Public Methods

        [DebuggerNonUserCode]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals(Square left, Square right) => left.SquareIndex == right.SquareIndex;

        [DebuggerNonUserCode]
        public static Square FromAlgebraic(string algebraicNotation)
        {
            var square = TryFromAlgebraic(algebraicNotation);
            if (!square.HasValue)
            {
                throw new ArgumentException(
                    $@"Invalid algebraic notation '{algebraicNotation}'.",
                    nameof(algebraicNotation));
            }

            return square.Value;
        }

        [DebuggerNonUserCode]
        public static Square? TryFromAlgebraic(string algebraicNotation)
        {
            if (algebraicNotation?.Length != 2)
            {
                return null;
            }

            var file = GetFileFromChar(algebraicNotation[0]);
            var rank = GetRankFromChar(algebraicNotation[1]);

            return ChessConstants.FileRange.Contains(file) && ChessConstants.RankRange.Contains(rank)
                ? new Square(file, rank)
                : default(Square?);
        }

        public static Square[] GenerateFile(int file)
        {
            #region Argument Check

            if (!ChessConstants.FileRange.Contains(file))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(file),
                    file,
                    $@"The value is out of the valid range {ChessConstants.FileRange}.");
            }

            #endregion

            return Enumerable
                .Range(0, ChessConstants.RankCount)
                .Select(rank => new Square(file, rank))
                .ToArray();
        }

        public static Square[] GenerateFile(char file)
        {
            var fileIndex = GetFileFromChar(file);

            if (!ChessConstants.FileRange.Contains(fileIndex))
            {
                throw new ArgumentOutOfRangeException(nameof(file), file, "The value is out of the valid range.");
            }

            return GenerateFile(fileIndex);
        }

        public static Square[] GenerateRank(int rank)
        {
            #region Argument Check

            if (!ChessConstants.RankRange.Contains(rank))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(rank),
                    rank,
                    $@"The value is out of the valid range {ChessConstants.RankRange}.");
            }

            #endregion

            return Enumerable
                .Range(0, ChessConstants.FileCount)
                .Select(file => new Square(file, rank))
                .ToArray();
        }

        public static Square[] GenerateRanks(params int[] ranks)
        {
            #region Argument Check

            if (ranks == null)
            {
                throw new ArgumentNullException(nameof(ranks));
            }

            #endregion

            return ranks.SelectMany(GenerateRank).Distinct().ToArray();
        }

        public override string ToString() => StringRepresentations[SquareIndex];

        public override bool Equals(object obj) => obj is Square && Equals((Square)obj);

        public override int GetHashCode() => SquareIndex;

        #endregion

        #region IEquatable<Square> Members

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Square other) => Equals(this, other);

        #endregion

        #region Internal Methods

        [DebuggerNonUserCode]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void EnsureValidSquareIndex(int squareIndex)
        {
            if (IsInvalidSquareIndex(squareIndex))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(squareIndex),
                    squareIndex,
                    $@"The value is out of the valid range ({0} .. {ChessConstants.MaxSquareIndex}).");
            }
        }

        [DebuggerNonUserCode]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetFileFromChar(char file) => checked(char.ToLowerInvariant(file) - 'a');

        [DebuggerNonUserCode]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetRankFromChar(char rank) => checked(rank - '1');

        #endregion

        #region Private Methods

        [DebuggerNonUserCode]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsInvalidSquareIndex(int squareIndex)
            => (squareIndex & ~ChessConstants.MaxSquareIndex) != 0;

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetFile(int squareIndex) => squareIndex & 0x07;

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetRank(int squareIndex) => (squareIndex >> 3) & 0x07;

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static char GetFileChar(int file) => (char)('a' + file);

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static char GetRankChar(int rank) => (char)('1' + rank);

        #endregion
    }
}