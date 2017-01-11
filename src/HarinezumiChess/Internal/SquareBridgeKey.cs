﻿using System;
using System.Diagnostics;
using Omnifactotum;

namespace HarinezumiChess.Internal
{
    internal struct SquareBridgeKey : IEquatable<SquareBridgeKey>
    {
        #region Constants and Fields

        private readonly int _hashCode;

        #endregion

        #region Constructors

        internal SquareBridgeKey(Square first, Square second)
        {
            #region Argument Check

            if (first == second)
            {
                throw new ArgumentException("Squares cannot be the same.");
            }

            #endregion

            if (first.SquareIndex > second.SquareIndex)
            {
                Factotum.Exchange(ref first, ref second);
            }

            First = first;
            Second = second;

            _hashCode = (First.SquareIndex << 8) | second.SquareIndex;
        }

        #endregion

        #region Public Properties

        public Square First
        {
            [DebuggerStepThrough]
            get;
        }

        public Square Second
        {
            [DebuggerStepThrough]
            get;
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj) => obj is SquareBridgeKey && Equals((SquareBridgeKey)obj);

        public override int GetHashCode() => _hashCode;

        public override string ToString() => $@"{{{First}:{Second}}}";

        #endregion

        #region IEquatable<SquareBridgeKey> Members

        public bool Equals(SquareBridgeKey other) => other.First == First && other.Second == Second;

        #endregion
    }
}