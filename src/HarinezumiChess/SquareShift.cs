using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace HarinezumiChess
{
    public struct SquareShift
    {
        #region Constructors

        public SquareShift(int fileOffset, int rankOffset)
        {
            FileOffset = fileOffset;
            RankOffset = rankOffset;
        }

        #endregion

        #region Public Properties

        public int FileOffset
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [DebuggerStepThrough]
            get;
        }

        public int RankOffset
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [DebuggerStepThrough]
            get;
        }

        #endregion
    }
}