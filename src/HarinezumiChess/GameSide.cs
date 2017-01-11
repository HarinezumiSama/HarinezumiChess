using HarinezumiChess.Internal;

namespace HarinezumiChess
{
    public enum GameSide
    {
        [FenChar('w')]
        White = 0,

        [FenChar('b')]
        Black = 1
    }
}