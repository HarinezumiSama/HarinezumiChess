using System.Diagnostics;

namespace HarinezumiChess.Internal
{
    internal struct InternalCastlingInfo
    {
        #region Constructors

        public InternalCastlingInfo(GameMove kingMove, Bitboard expectedEmptySquares)
        {
            KingMove = kingMove;
            ExpectedEmptySquares = expectedEmptySquares;
        }

        #endregion

        #region Public Properties

        public GameMove KingMove
        {
            [DebuggerStepThrough]
            get;
        }

        public Bitboard ExpectedEmptySquares
        {
            [DebuggerStepThrough]
            get;
        }

        #endregion
    }
}