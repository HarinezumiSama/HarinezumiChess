using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace HarinezumiChess
{
    //// ReSharper disable once UseNameofExpression - False positive
    [DebuggerDisplay("{ToString(),nq}")]
    public struct EnPassantCaptureInfo : IEquatable<EnPassantCaptureInfo>
    {
        #region Constructors

        internal EnPassantCaptureInfo(Square captureSquare, Square targetPieceSquare)
        {
            CaptureSquare = captureSquare;
            TargetPieceSquare = targetPieceSquare;
        }

        #endregion

        #region Public Properties

        public Square CaptureSquare
        {
            get;
        }

        public Square TargetPieceSquare
        {
            get;
        }

        #endregion

        #region Public Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals(EnPassantCaptureInfo left, EnPassantCaptureInfo right)
            => left.CaptureSquare == right.CaptureSquare && left.TargetPieceSquare == right.TargetPieceSquare;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is EnPassantCaptureInfo && Equals((EnPassantCaptureInfo)obj);

        public override int GetHashCode() => CaptureSquare.GetHashCode();

        public override string ToString()
            => $@"{{ {nameof(CaptureSquare)} = {CaptureSquare}, {nameof(TargetPieceSquare)} = {TargetPieceSquare} }}";

        #endregion

        #region Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(EnPassantCaptureInfo left, EnPassantCaptureInfo right) => Equals(left, right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(EnPassantCaptureInfo left, EnPassantCaptureInfo right) => !Equals(left, right);

        #endregion

        #region IEquatable<EnPassantCaptureData> Members

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(EnPassantCaptureInfo other) => Equals(this, other);

        #endregion
    }
}