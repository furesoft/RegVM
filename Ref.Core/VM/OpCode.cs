namespace Ref.Core
{
    public enum OpCode
    {
        NOP = 0x00,
        HLT = 0x01,

        LOAD8 = 0x4,
        LOAD16 = 0x5,
        LOAD32 = 0x6,

        LOADRO8 = 0x7,
        LOADRO16 = 0x8,
        LOADRO32 = 0x9,

        MOV = 0xB,

        JMP = 0xD,
        JMPR = 0xE,
        JMPE = 0xF,
        JMPNE = 0x10,

        PUSH8 = 0x13,
        PUSH16 = 0x14,
        PUSH32 = 0x15,

        POP = 0x19,

        CMP = 0x1B,

        INT = 0x1D,

        OUT8 = 0x1F,
        OUT16 = 0x20,
        OUT32 = 0x21,

        IN = 0x25,

        NEW = 0x2B,
        FREE = 0x2C,

        CALL = 0x2E,
        RET = 0x2F,

        ADD = 0x34,
        SUB = 0x35,
        MUL = 0x36,
        DIV = 0x37,

        INC = 0x38,
        DEC = 0x39,

        EQUAL = 0x3C,
        NEQUAL = 0x3C,
    }
}