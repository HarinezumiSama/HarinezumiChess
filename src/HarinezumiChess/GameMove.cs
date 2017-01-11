﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Omnifactotum.Annotations;

namespace HarinezumiChess
{
    public struct GameMove : IEquatable<GameMove>
    {
        #region Constants and Fields

        private const string FromGroupName = "from";
        private const string ToGroupName = "to";
        private const string PromotionGroupName = "promo";

        private const RegexOptions BasicPatternRegexOptions =
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace
                | RegexOptions.IgnoreCase;

        private static readonly string PromotionFenChars =
            new string(ChessConstants.GetValidPromotions().Select(item => item.GetFenChar()).ToArray());

        private static readonly Regex MainStringPatternRegex = new Regex(
            $@"^ (?<{FromGroupName}>[a-h][1-8]) (?:\-|x) (?<{ToGroupName}>[a-h][1-8]) (\=(?<{PromotionGroupName}>[{
                PromotionFenChars}]))? $",
            BasicPatternRegexOptions);

        private static readonly Regex UciStringPatternRegex = new Regex(
            $@"^ (?<{FromGroupName}>[a-h][1-8]) (?<{ToGroupName}>[a-h][1-8]) (?<{PromotionGroupName}>[{
                PromotionFenChars}])? $",
            BasicPatternRegexOptions);

        private static readonly Regex[] StringPatternRegexes = { MainStringPatternRegex, UciStringPatternRegex };

        private readonly int _hashCode;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameMove"/> class.
        /// </summary>
        public GameMove(Square from, Square to, PieceType promotionResult)
        {
            From = from;
            To = to;
            PromotionResult = promotionResult;

            _hashCode = ((byte)PromotionResult << 16) | (To.SquareIndex << 8) | From.SquareIndex;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameMove"/> class.
        /// </summary>
        public GameMove(Square from, Square to)
            : this(from, to, PieceType.None)
        {
            // Nothing to do
        }

        #endregion

        #region Public Properties

        public Square From
        {
            [DebuggerStepThrough]
            get;
        }

        public Square To
        {
            [DebuggerStepThrough]
            get;
        }

        public PieceType PromotionResult
        {
            [DebuggerStepThrough]
            get;
        }

        #endregion

        #region Operators

        public static bool operator ==(GameMove left, GameMove right) => Equals(left, right);

        public static bool operator !=(GameMove left, GameMove right) => !(left == right);

        [DebuggerNonUserCode]
        public static implicit operator GameMove(string stringNotation) => FromStringNotation(stringNotation);

        #endregion

        #region Public Methods

        public static bool Equals(GameMove left, GameMove right)
            => left.From == right.From && left.To == right.To && left.PromotionResult == right.PromotionResult;

        [DebuggerNonUserCode]
        public static GameMove FromStringNotation([NotNull] string stringNotation)
        {
            #region Argument Check

            if (stringNotation == null)
            {
                throw new ArgumentNullException(nameof(stringNotation));
            }

            #endregion

            foreach (var stringPatternRegex in StringPatternRegexes)
            {
                var match = stringPatternRegex.Match(stringNotation);
                if (!match.Success)
                {
                    continue;
                }

                var from = match.Groups[FromGroupName].Value;
                var to = match.Groups[ToGroupName].Value;
                var promotionGroup = match.Groups[PromotionGroupName];

                var pieceType = promotionGroup.Success
                    ? char.ToUpperInvariant(promotionGroup.Value.Single()).ToPieceType()
                    : PieceType.None;

                return new GameMove(Square.FromAlgebraic(from), Square.FromAlgebraic(to), pieceType);
            }

            throw new ArgumentException(
                $@"Invalid string notation of a move: '{stringNotation}'.",
                nameof(stringNotation));
        }

        public override bool Equals(object obj) => obj is GameMove && Equals((GameMove)obj);

        public override int GetHashCode() => _hashCode;

        public override string ToString() => ToString(false);

        public string ToString(bool renderCaptureSign)
            => $@"{From}{(renderCaptureSign ? ChessConstants.CaptureCharString : string.Empty)}{To}{
                (PromotionResult == PieceType.None
                    ? string.Empty
                    : ChessConstants.PromotionPrefixCharString + PromotionResult.GetFenChar())}";

        public GameMove MakePromotion(PieceType promotionResult)
        {
            #region Argument Check

            if (!ChessConstants.ValidPromotions.Contains(promotionResult))
            {
                throw new ArgumentException(
                    $@"Must be a valid promotion piece ({promotionResult}).",
                    nameof(promotionResult));
            }

            #endregion

            return new GameMove(From, To, promotionResult);
        }

        public GameMove[] MakeAllPromotions()
        {
            var from = From;
            var to = To;

            return ChessConstants.ValidPromotions.Select(item => new GameMove(from, to, item)).ToArray();
        }

        #endregion

        #region IEquatable<GameMove> Members

        public bool Equals(GameMove other) => Equals(this, other);

        #endregion
    }
}