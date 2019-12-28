namespace Ref.Core
{
    public enum OpCode
    {
        NOP = 0x00,
        HLT = 0x01,

        LOAD = 0x4,

        LOADRO = 0x7,

        MOV = 0xB,

        JMP = 0xD,
        JMPR = 0xE,
        JMPE = 0xF,
        JMPNE = 0x10,

        PUSH = 0x13,

        POP = 0x19,

        CMP = 0x1B,

        INT = 0x1D,

        OUT = 0x1F,

        IN = 0x25,

        NEW = 0x2B,
        FREE = 0x2C,
        STORE = 0x2D,

        CALL = 0x2E,
        RET = 0x2F,

        ADD = 0x34,
        SUB = 0x35,
        MUL = 0x36,
        DIV = 0x37,

        INC = 0x38,
        DEC = 0x39,

        EQUAL = 0x3C,
        NEQUAL = 0x3D,
    }
}