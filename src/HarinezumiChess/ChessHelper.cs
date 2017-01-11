﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using HarinezumiChess.Internal;
using Omnifactotum;

namespace HarinezumiChess
{
    public static class ChessHelper
    {
        #region Constants and Fields

        public const double DefaultZeroTolerance = 1E-7d;

        public static readonly string PlatformVersion = typeof(ChessHelper)
            .Assembly
            .GetSingleCustomAttribute<AssemblyInformationalVersionAttribute>(false)
            .InformationalVersion;

        public static readonly Omnifactotum.ReadOnlyDictionary<CastlingType, CastlingInfo> CastlingTypeToInfoMap =
            ChessConstants.AllCastlingInfos.ToDictionary(obj => obj.CastlingType).AsReadOnly();

        public static readonly Omnifactotum.ReadOnlyDictionary<CastlingOptions, CastlingInfo>
            CastlingOptionToInfoMap =
                ChessConstants.AllCastlingInfos.ToDictionary(obj => obj.CastlingType.ToOption()).AsReadOnly();

        public static readonly Omnifactotum.ReadOnlyDictionary<GameMove, CastlingInfo> KingMoveToCastlingInfoMap =
            ChessConstants.AllCastlingInfos.ToDictionary(obj => obj.KingMove).AsReadOnly();

        public static readonly Omnifactotum.ReadOnlyDictionary<GameSide, ReadOnlySet<CastlingOptions>>
            GameSideToCastlingOptionSetMap =
                new Omnifactotum.ReadOnlyDictionary<GameSide, ReadOnlySet<CastlingOptions>>(
                    new Dictionary<GameSide, ReadOnlySet<CastlingOptions>>
                    {
                        {
                            GameSide.White,
                            new[] { CastlingOptions.WhiteKingSide, CastlingOptions.WhiteQueenSide }
                                .ToHashSet()
                                .AsReadOnly()
                        },
                        {
                            GameSide.Black,
                            new[] { CastlingOptions.BlackKingSide, CastlingOptions.BlackQueenSide }
                                .ToHashSet()
                                .AsReadOnly()
                        }
                    });

        public static readonly Omnifactotum.ReadOnlyDictionary<GameSide, CastlingOptions>
            GameSideToCastlingOptionsMap =
                GameSideToCastlingOptionSetMap
                    .ToDictionary(
                        pair => pair.Key,
                        pair => pair.Value.Aggregate(CastlingOptions.None, (a, item) => a | item))
                    .AsReadOnly();

        public static readonly Omnifactotum.ReadOnlyDictionary<GameSide, int> GameSideToPawnPromotionRankMap =
            new Omnifactotum.ReadOnlyDictionary<GameSide, int>(
                new EnumFixedSizeDictionary<GameSide, int>
                {
                    { GameSide.White, ChessConstants.WhitePawnPromotionRank },
                    { GameSide.Black, ChessConstants.BlackPawnPromotionRank }
                });

        public static readonly ReadOnlyCollection<Square> AllSquares =
            Enumerable
                .Range(0, ChessConstants.SquareCount)
                .Select(squareIndex => new Square(squareIndex))
                .ToArray()
                .AsReadOnly();

        public static readonly PieceType DefaultPromotion = PieceType.Queen;

        internal const int MaxSlidingPieceDistance = 8;
        internal const int MaxPawnAttackOrMoveDistance = 1;
        internal const int MaxKingMoveOrAttackDistance = 1;

        internal static readonly ReadOnlyCollection<SquareShift> StraightRays =
            new ReadOnlyCollection<SquareShift>(
                new[]
                {
                    new SquareShift(0, 1),
                    new SquareShift(1, 0),
                    new SquareShift(0, -1),
                    new SquareShift(-1, 0)
                });

        internal static readonly ReadOnlyCollection<SquareShift> DiagonalRays =
            new ReadOnlyCollection<SquareShift>(
                new[]
                {
                    new SquareShift(1, 1),
                    new SquareShift(1, -1),
                    new SquareShift(-1, 1),
                    new SquareShift(-1, -1)
                });

        internal static readonly ReadOnlyCollection<SquareShift> AllRays =
            new ReadOnlyCollection<SquareShift>(StraightRays.Concat(DiagonalRays).ToArray());

        internal static readonly ReadOnlySet<SquareShift> KnightAttackOrMoveOffsets =
            new ReadOnlySet<SquareShift>(
                new HashSet<SquareShift>(
                    new[]
                    {
                        new SquareShift(+2, +1),
                        new SquareShift(+1, +2),
                        new SquareShift(+2, -1),
                        new SquareShift(+1, -2),
                        new SquareShift(-2, +1),
                        new SquareShift(-1, +2),
                        new SquareShift(-2, -1),
                        new SquareShift(-1, -2)
                    }));

        internal static readonly ReadOnlyCollection<PieceType> NonDefaultPromotions =
            ChessConstants.ValidPromotions.Except(DefaultPromotion.AsArray()).ToArray().AsReadOnly();

        internal static readonly Omnifactotum.ReadOnlyDictionary<SquareBridgeKey, Bitboard> SquareBridgeMap =
            GenerateSquareBridgeMap();

        internal static readonly Bitboard InvalidPawnSquaresBitboard =
            new Bitboard(Square.GenerateRanks(ChessConstants.RankRange.Lower, ChessConstants.RankRange.Upper));

        private const string FenRankRegexSnippet = @"[1-8KkQqRrBbNnPp]{1,8}";

        private const string MoveSeparator = ", ";

        private static readonly Omnifactotum.ReadOnlyDictionary<Square, Square[]>
            KnightMoveSquareMap =
                AllSquares
                    .ToDictionary(Factotum.Identity, GetKnightMoveSquaresNonCached)
                    .AsReadOnly();

        private static readonly Regex ValidFenRegex = new Regex(
            string.Format(
                @"^ \s* {0}/{0}/{0}/{0}/{0}/{0}/{0}/{0} \s+ (?:w|b) \s+ (?:[KkQq]+|\-) \s+ (?:[a-h][1-8]|\-) \s+ \d+ \s+ \d+ \s* $",
                FenRankRegexSnippet),
            RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);

        #endregion

        #region Public Methods

        public static bool IsZero(this double value, double tolerance = DefaultZeroTolerance)
        {
            return Math.Abs(value) <= DefaultZeroTolerance;
        }

        public static int ToSign(this bool value)
        {
            return value ? 1 : -1;
        }

        public static bool IsValidFenFormat(string fen)
        {
            return !fen.IsNullOrEmpty() && ValidFenRegex.IsMatch(fen);
        }

        public static Square[] GetOnboardSquares(Square square, IEnumerable<SquareShift> shifts)
        {
            #region Argument Check

            if (shifts == null)
            {
                throw new ArgumentNullException(nameof(shifts));
            }

            #endregion

            return shifts.Select(shift => square + shift).Where(p => p.HasValue).Select(p => p.Value).ToArray();
        }

        #endregion

        #region Internal Methods

        internal static Square[] GetKnightMoveSquares(Square square)
        {
            return KnightMoveSquareMap[square];
        }

        internal static bool TryParseInt(string value, out int result)
        {
            return int.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out result);
        }

        internal static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> collection)
        {
            #region Argument Check

            if (hashSet == null)
            {
                throw new ArgumentNullException(nameof(hashSet));
            }

            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            #endregion

            collection.DoForEach(item => hashSet.Add(item));
        }

        internal static PieceType ToPieceType(this char fenChar)
        {
            PieceType result;
            if (!ChessConstants.FenCharToPieceTypeMap.TryGetValue(fenChar, out result))
            {
                throw new ArgumentException($@"Invalid FEN character ({fenChar}).", nameof(fenChar));
            }

            return result;
        }

        #endregion

        #region Private Methods

        private static Square[] GetKnightMoveSquaresNonCached(Square square)
        {
            return GetOnboardSquares(square, KnightAttackOrMoveOffsets);
        }

        private static Square[] GetMoveSquareArraysByRays(
            Square sourceSquare,
            IEnumerable<SquareShift> rays,
            int maxDistance)
        {
            var resultList = new List<Square>(AllSquares.Count);

            foreach (var ray in rays)
            {
                var distance = 1;
                for (var square = sourceSquare + ray;
                    square.HasValue && distance <= maxDistance;
                    square = square.Value + ray, distance++)
                {
                    resultList.Add(square.Value);
                }
            }

            return resultList.ToArray();
        }

        private static Omnifactotum.ReadOnlyDictionary<SquareBridgeKey, Bitboard> GenerateSquareBridgeMap()
        {
            var resultMap = new Dictionary<SquareBridgeKey, Bitboard>(AllSquares.Count * AllSquares.Count);

            var allSquares = AllSquares.ToArray();
            for (var outerIndex = 0; outerIndex < allSquares.Length; outerIndex++)
            {
                var first = allSquares[outerIndex];
                for (var innerIndex = outerIndex + 1; innerIndex < allSquares.Length; innerIndex++)
                {
                    var second = allSquares[innerIndex];

                    foreach (var ray in AllRays)
                    {
                        var squares = GetMoveSquareArraysByRays(first, ray.AsArray(), MaxSlidingPieceDistance);
                        var index = Array.IndexOf(squares, second);
                        if (index < 0)
                        {
                            continue;
                        }

                        var key = new SquareBridgeKey(first, second);
                        var squareBridge = new Bitboard(squares.Take(index));
                        resultMap.Add(key, squareBridge);
                        break;
                    }
                }
            }

            return resultMap.AsReadOnly();
        }

        #endregion
    }
}