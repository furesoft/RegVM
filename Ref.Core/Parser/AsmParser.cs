using System.Collections.Generic;

namespace Ref.Core.Parser
{
    public class AsmParser
    {
        public static AsmFile Parse(string src)
        {
            var res = new AsmFile();
            var lines = src.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                if (line.StartsWith(".")) // when line is a db entry
                {
                    var linecmd = line.Substring(1);
                    var spl = Utils.Split(linecmd);
                    var datacmd = SyntaxNode.CreateCommand(spl[0], ParseArgs(linecmd.Substring(spl[0].Length + 1)));

                    res.DataCommands.Add(datacmd);
                }
                else //when line is a opcode
                {
                    var opline = line.Split(' ');
                    var opcmd = SyntaxNode.CreateCommand(opline[0], ParseArgs(line.Substring(opline[0].Length + 1)));
                    res.Commands.Add(opcmd);
                }
            }

            return res;
        }

        private static List<AsmCommandArg> ParseArgs(string line)
        {
            var res = new List<AsmCommandArg>();

            var spl = Utils.Split(line);
            foreach (var arg in spl)
            {
                res.Add(SyntaxNode.CreateArg(arg));
            }

            return res;
        }
    }
}