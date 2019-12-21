namespace Ref.Core
{
    public enum Registers : byte
    {
        A, B, C, D, E, F,
        EAX, EBX,
        KDR, // Keyboard Device Ready Register
        IPR, ORE, BRR, ERR, ACC,
        KDS,
        HEAP,
    }
}