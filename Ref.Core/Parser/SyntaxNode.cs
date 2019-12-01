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
            return new AsmCommandArg { Value = int.Parse(s) };
        }

        public static string CreateBinInteger(string bin)
        {
            return Convert.ToInt32(bin.Replace("_", ""), 2).ToString();
        }

        public static AsmCommand CreateCommand(string op, IList<AsmCommandArg> args)
        {
            return new AsmCommand { Name = op, Args = args.ToList() };
        }

        public static string CreateHexInteger(string hex)
        {
            return int.Parse(hex, NumberStyles.HexNumber).ToString();
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