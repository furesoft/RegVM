using System.Collections.Generic;

namespace Ref.Core.Parser
{
    public class AsmParser
    {
        public static AsmFile Parse(string src)
        {
            var res = new AsmFile();
            var lines = src.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            bool inLabel = false;
            AsmLabel label = null;

            foreach (var line in lines)
            {
                if (line.StartsWith(".")) // when line is a db entry
                {
                    var linecmd = line.Substring(1);
                    var spl = Utils.Split(linecmd);
                    var datacmd = SyntaxNode.CreateCommand(spl[0], ParseArgs(linecmd.Substring(spl[0].Length + 1)));

                    res.DataCommands.Add(datacmd);
                }
                else if (line.EndsWith(":")) // line is label definition
                {
                    var name = line.Substring(0, line.Length - 1);
                    inLabel = true;
                    label = new AsmLabel(name);
                }
                else //when line is a opcode
                {
                    if (!line.StartsWith("\t") && label != null)
                    {
                        inLabel = false;
                        //ToDo: add label to result
                        res.Labels.Add(label);
                        label = null;
                    }

                    List<AsmCommand> cmds = res.Commands;

                    if (inLabel)
                    {
                        cmds = label.Commands;
                    }

                    var opline = line.Replace("\t", "").Split(' ');
                    if (opline.Length > 1)
                    {
                        var opcmd = SyntaxNode.CreateCommand(opline[0], ParseArgs(line.Substring(opline[0].Length + 1)));
                        cmds.Add(opcmd);
                    }
                    else
                    {
                        var opcmd = SyntaxNode.CreateCommand(opline[0], new List<AsmCommandArg>());
                        cmds.Add(opcmd);
                    }
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