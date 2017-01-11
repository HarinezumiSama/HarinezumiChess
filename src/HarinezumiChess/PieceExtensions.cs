﻿using System;
using System.Runtime.CompilerServices;

namespace HarinezumiChess
{
    public static class PieceExtensions
    {
        #region Public Methods

        /// <summary>
        ///     Returns the game side of the specified piece, or <c>null</c> if the piece is
        ///     <see cref="Piece.None"/>, that is, represents an empty square.
        /// </summary>
        /// <param name="piece">
        ///     The piece to get the game side of.
        /// </param>
        /// <returns>
        ///     The game side of the specified piece, or <c>null</c> if the piece is
        ///     <see cref="Piece.None"/>, that is, represents an empty square.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameSide? GetSide(this Piece piece)
            => piece == Piece.None
                ? default(GameSide?)
                : (GameSide)(((int)piece & PieceConstants.SideMask) >> PieceConstants.BlackSideShift);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PieceType GetPieceType(this Piece piece) => (PieceType)((int)piece & PieceConstants.TypeMask);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Is(this Piece piece, PieceType pieceType) => GetPieceType(piece) == pieceType;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Is(this Piece piece, GameSide gameSide) => GetSide(piece) == gameSide;

        public static char GetFenChar(this Piece piece)
        {
            char result;
            if (!ChessConstants.PieceToFenCharMap.TryGetValue(piece, out result))
            {
                throw new ArgumentException($@"Invalid piece '{piece}'.", nameof(piece));
            }

            return result;
        }

        public static string GetDescription(this Piece piece)
        {
            var side = piece.GetSide();
            var pieceType = piece.GetPieceType();

            if (!side.HasValue || pieceType == PieceType.None)
            {
                return "Empty Square";
            }

            return $@"{side.Value} {pieceType}";
        }

        #endregion
    }
}