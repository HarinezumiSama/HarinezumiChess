using System.Runtime.CompilerServices;

namespace HarinezumiChess
{
    public static class CastlingSideExtensions
    {
        #region Public Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CastlingType ToCastlingType(this CastlingSide castlingSide, GameSide gameSide)
        {
            return unchecked((CastlingType)(((int)gameSide << 1) + castlingSide));
        }

        #endregion
    }
}