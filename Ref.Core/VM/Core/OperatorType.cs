using System;

namespace RefVM
{
    [Flags]
    public enum OperatorType
    {
        EQUAL,
        NOTEQUAL,
        LESS,
        LESSEQUAL,
        GREATER,
        GREATHEREQUAL
    }
}