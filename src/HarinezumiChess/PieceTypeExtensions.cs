using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HarinezumiChess.Internal;

namespace HarinezumiChess
{
    public static class PieceTypeExtensions
    {
        #region Public Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PieceType EnsureDefined(this PieceType pieceType)
        {
            if (DebugConstants.EnsureEnumValuesDefined && !ChessConstants.PieceTypes.Contains(pieceType))
            {
                throw new InvalidEnumArgumentException(nameof(pieceType), (int)pieceType, pieceType.GetType());
            }

            return pieceType;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Piece ToPiece(this PieceType pieceType, GameSide side)
        {
            var result = pieceType == PieceType.None
                ? Piece.None
                : (Piece)((int)pieceType | ((int)side << PieceConstants.BlackSideShift));

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char GetFenChar(this PieceType pieceType)
        {
            char result;
            if (!ChessConstants.PieceTypeToFenCharMap.TryGetValue(pieceType, out result))
            {
                throw new ArgumentException($@"Invalid piece type ({pieceType}).", nameof(pieceType));
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSlidingDiagonally(this PieceType pieceType)
            => ((int)pieceType & (int)PieceTypeMask.SlidingDiagonally) == (int)PieceTypeMask.SlidingDiagonally;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSlidingStraight(this PieceType pieceType)
            => ((int)pieceType & (int)PieceTypeMask.SlidingStraight) == (int)PieceTypeMask.SlidingStraight;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSliding(this PieceType pieceType)
            => ((int)pieceType & (int)PieceTypeMask.Sliding) == (int)PieceTypeMask.Sliding;

        #endregion
    }
}