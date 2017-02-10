using System;

namespace HarinezumiChess
{
    [Flags]
    public enum GameMoveFlags
    {
        None = 0,
        PawnPromotion = 0x01,
        RegularCapture = 0x02,
        EnPassantCapture = 0x04,
        KingCastling = 0x08,
        Check = 0x10 //// TODO [vmcl] NEW-DESIGN: Implement and use GameMoveFlags.Check
    }
}