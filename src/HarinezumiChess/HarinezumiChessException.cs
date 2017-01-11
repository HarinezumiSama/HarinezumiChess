using System;
using System.Runtime.Serialization;

namespace HarinezumiChess
{
    [Serializable]
    public sealed class HarinezumiChessException : Exception
    {
        #region Constructors

        internal HarinezumiChessException(string message)
            : base(message)
        {
            // Nothing to do
        }

        internal HarinezumiChessException(string message, Exception innerException)
            : base(message, innerException)
        {
            // Nothing to do
        }

        private HarinezumiChessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Nothing to do
        }

        #endregion
    }
}