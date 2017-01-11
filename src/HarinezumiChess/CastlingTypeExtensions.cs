using System.Runtime.CompilerServices;

namespace HarinezumiChess
{
    public static class CastlingTypeExtensions
    {
        #region Public Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CastlingOptions ToOption(this CastlingType castlingType)
            => unchecked((CastlingOptions)(1 << (int)castlingType));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CastlingSide GetSide(this CastlingType castlingType)
            => (CastlingSide)((int)castlingType & 1);

        #endregion
    }
}