﻿using System;
using System.Text;
using Omnifactotum;
using Omnifactotum.Annotations;

namespace HarinezumiChess
{
    public static class PiecePositionExtensions
    {
        #region Public Methods

        public static void GetFenSnippet([NotNull] this PiecePosition position, [NotNull] StringBuilder resultBuilder)
        {
            #region Argument Check

            if (position == null)
            {
                throw new ArgumentNullException(nameof(position));
            }

            if (resultBuilder == null)
            {
                throw new ArgumentNullException(nameof(resultBuilder));
            }

            #endregion

            var emptySquareCount = new ValueContainer<int>(0);

            void WriteEmptySquareCount()
            {
                //// ReSharper disable once InvertIf - Does not improve readability in this case
                if (emptySquareCount.Value > 0)
                {
                    resultBuilder.Append(emptySquareCount.Value);
                    emptySquareCount.Value = 0;
                }
            }

            for (var rank = ChessConstants.RankCount - 1; rank >= 0; rank--)
            {
                if (rank < ChessConstants.RankCount - 1)
                {
                    resultBuilder.Append(ChessConstants.FenRankSeparator);
                }

                for (var file = 0; file < ChessConstants.FileCount; file++)
                {
                    var square = new Square(file, rank);
                    var piece = position[square];
                    if (piece == Piece.None)
                    {
                        emptySquareCount.Value++;
                        continue;
                    }

                    WriteEmptySquareCount();
                    var fenChar = piece.GetFenChar();
                    resultBuilder.Append(fenChar);
                }

                WriteEmptySquareCount();
            }
        }

        public static string GetFenSnippet([NotNull] this PiecePosition position)
        {
            var resultBuilder = new StringBuilder();
            GetFenSnippet(position, resultBuilder);
            return resultBuilder.ToString();
        }

        #endregion
    }
}