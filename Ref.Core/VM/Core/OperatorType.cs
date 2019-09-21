using System;

namespace Ref.Core
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