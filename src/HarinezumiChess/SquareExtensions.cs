using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace HarinezumiChess
{
    /// <summary>
    ///     Contains the extension methods for the <see cref="Square"/> structure.
    /// </summary>
    public static class SquareExtensions
    {
        #region Public Methods

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDark(this Square square) => ((square.File + square.Rank) & 0x1) == 0;

        #endregion
    }
}