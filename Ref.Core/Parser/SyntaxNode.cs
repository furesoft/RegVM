using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Ref.Core.Parser
{
    public static class SyntaxNode
    {
        public static AsmCommandArg CreateArg(string s)
        {
            if (s.StartsWith("0x"))
            {
                return new AsmCommandArg { Value = Convert.ToInt32(s, 16), Type = ArgType.Literal };
            }
            if (s.StartsWith("$"))
            {
                return new AsmCommandArg { Value = s.Substring(1), Type = ArgType.Register };
            }
            if (s.StartsWith("#"))
            {
                return new AsmCommandArg { Value = int.Parse(s.Substring(1)), Type = ArgType.Literal };
            }

            return new AsmCommandArg { Type = ArgType.Option, Value = s };
        }

        public static AsmCommand CreateCommand(string op, IList<AsmCommandArg> args)
        {
            try
            {
                return new AsmCommand { Name = op, OpCode = (OpCode)Enum.Parse(typeof(OpCode), op, true), Args = args.ToList() };
            }
            catch { }

            return new AsmCommand
            {
                Name = op,
                Args = args.ToList()
            };
        }

        public static LabelDefintionNode CreateLabel(string name, IList<AsmCommand> body)
        {
            return new LabelDefintionNode { Name = name, Body = body.ToList() };
        }

        public static AsmCommandArg CreateLabelCall(string name)
        {
            return new AsmCommandArg { Type = ArgType.Label, Value = name };
        }

        public static AsmCommandArg CreateNumArg(string src)
        {
            return new AsmCommandArg { Type = ArgType.Literal, Value = int.Parse(src) };
        }

        public static AsmCommandArg CreateRegister(string name)
        {
            return new AsmCommandArg { Type = ArgType.Register, Value = name };
        }
    }
}