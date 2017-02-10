using System;

namespace HarinezumiChess
{
    //// TODO [vmcl] Revise DoublePushInfo and its usages

    public sealed class DoublePushInfo
    {
        #region Constants and Fields

        private const int CaptureRankDistance = 1;
        private const int MoveDistance = 2;

        #endregion

        #region Constructors

        internal DoublePushInfo(GameSide side)
        {
            bool isWhite;
            switch (side)
            {
                case GameSide.White:
                    isWhite = true;
                    break;

                case GameSide.Black:
                    isWhite = false;
                    break;

                default:
                    throw side.CreateEnumValueNotSupportedException();
            }

            Side = side;
            StartRank = isWhite ? 1 : ChessConstants.RankCount - 2;

            EndRank = StartRank + (isWhite ? MoveDistance : -MoveDistance);
            CaptureTargetRank = StartRank + (isWhite ? CaptureRankDistance : -CaptureRankDistance);
        }

        #endregion

        #region Public Properties

        public GameSide Side
        {
            get;
        }

        public int StartRank
        {
            get;
        }

        public int EndRank
        {
            get;
        }

        public int CaptureTargetRank
        {
            get;
        }

        #endregion
    }
}