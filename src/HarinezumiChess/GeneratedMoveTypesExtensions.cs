﻿using System.Runtime.CompilerServices;

namespace HarinezumiChess
{
    public static class GeneratedMoveTypesExtensions
    {
        #region Public Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAnySet(this GeneratedMoveTypes value, GeneratedMoveTypes flags)
        {
            return (value & flags) != 0;
        }

        #endregion
    }
}