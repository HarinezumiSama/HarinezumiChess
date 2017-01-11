using System.Diagnostics;

namespace HarinezumiChess
{
    public struct GameMoveData
    {
        #region Constructors

        internal GameMoveData(GameMove move, GameMoveFlags moveFlags)
        {
            Move = move;
            MoveFlags = moveFlags;
        }

        #endregion

        #region Public Properties

        public GameMove Move
        {
            [DebuggerStepThrough]
            get;
        }

        public GameMoveFlags MoveFlags
        {
            [DebuggerStepThrough]
            get;
        }

        #endregion

        #region Public Methods

        public override string ToString() => $@"{Move} : {MoveFlags}";

        #endregion
    }
}