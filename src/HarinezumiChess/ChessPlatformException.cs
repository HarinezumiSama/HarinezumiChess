﻿using System;
using System.Runtime.Serialization;

namespace HarinezumiChess
{
    [Serializable]
    public sealed class ChessPlatformException : Exception
    {
        #region Constructors

        internal ChessPlatformException(string message)
            : base(message)
        {
            // Nothing to do
        }

        internal ChessPlatformException(string message, Exception innerException)
            : base(message, innerException)
        {
            // Nothing to do
        }

        private ChessPlatformException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Nothing to do
        }

        #endregion

        #region Internal Methods

        internal static ChessPlatformException CreateInconsistentStateError()
        {
            return new ChessPlatformException("Inconsistent state of the piece data.");
        }

        #endregion
    }
}