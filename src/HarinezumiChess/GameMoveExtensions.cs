using System;
using System.Collections.Generic;
using System.Linq;
using Omnifactotum.Annotations;

namespace HarinezumiChess
{
    public static class GameMoveExtensions
    {
        #region Constants and Fields

        private const string MoveSeparator = ", ";

        #endregion

        #region Public Methods

        public static string ToUciNotation(this GameMove move)
        {
            var isPromotion = move.PromotionResult != PieceType.None;

            var chars = new[]
            {
                move.From.FileChar,
                move.From.RankChar,
                move.To.FileChar,
                move.To.RankChar,
                isPromotion ? char.ToLowerInvariant(move.PromotionResult.GetFenChar()) : '\0'
            };

            return new string(chars, 0, isPromotion ? chars.Length : chars.Length - 1);
        }

        public static string ToUciNotation([NotNull] this ICollection<GameMove> moves)
        {
            #region Argument Check

            if (moves == null)
            {
                throw new ArgumentNullException(nameof(moves));
            }

            #endregion

            return moves.Select(ToUciNotation).Join(MoveSeparator);
        }

        #endregion
    }
}