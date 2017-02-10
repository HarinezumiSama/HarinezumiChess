using System;

namespace HarinezumiChess
{
    [Flags]
    public enum GeneratedMoveTypes
    {
        Quiet = 0x00000001,
        Capture = 0x00000002,

        //// TODO [vmcl] NEW-DESIGN: Include GeneratedMoveTypes.Check for generating checks separately
        //// If this flag is set, then only check moves are generated (quiet or capture - depends on other flags).
        //// Hence, this flags does not make sense to be set alone.
        CheckOnly = unchecked((int)0x80000000),

        All = Quiet | Capture
    }
}