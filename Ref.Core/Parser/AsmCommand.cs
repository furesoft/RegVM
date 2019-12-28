using System.Collections.Generic;

namespace Ref.Core.Parser
{
    public enum ArgType
    {
        Register, Literal, Label,
        Option
    }

    public class AsmCommand
    {
        public List<AsmCommandArg> Args { get; set; } = new List<AsmCommandArg>();
        public string Name { get; set; }
        public OpCode OpCode { get; set; }
        public object this[int index] => Args[index].Value;
    }

    public class AsmCommandArg
    {
        public ArgType Type { get; set; }
        public object Value { get; set; }
    }
}