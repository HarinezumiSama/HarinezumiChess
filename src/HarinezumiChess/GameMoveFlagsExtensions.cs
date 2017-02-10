using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace HarinezumiChess
{
    public static class GameMoveFlagsExtensions
    {
        #region Public Methods

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAnySet(this GameMoveFlags value, GameMoveFlags flags) => (value & flags) != 0;

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPawnPromotion(this GameMoveFlags value) => value.IsAnySet(GameMoveFlags.PawnPromotion);

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsRegularCapture(this GameMoveFlags value)
            => value.IsAnySet(GameMoveFlags.RegularCapture);

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEnPassantCapture(this GameMoveFlags value)
            => value.IsAnySet(GameMoveFlags.EnPassantCapture);

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAnyCapture(this GameMoveFlags value)
            => value.IsAnySet(GameMoveFlags.RegularCapture | GameMoveFlags.EnPassantCapture);

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsKingCastling(this GameMoveFlags value) => value.IsAnySet(GameMoveFlags.KingCastling);

        #endregion
    }
}