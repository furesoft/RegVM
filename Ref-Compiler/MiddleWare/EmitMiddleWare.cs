using System;
using System.IO;
using System.Linq;
using PipelineNet.Middleware;
using Ref.Core;
using Ref.Core.Parser;
using Ref.Core.VM.IO;

namespace Ref_Compiler.MiddleWare
{
    public class EmitMiddleWare : IMiddleware<Options>
    {
        public void Run(Options parameter, Action<Options> next)
        {
            var ast = (AsmFile)parameter.Tags["AST"];
            var writer = new AssemblyWriter();
            var cmdBuffer = new CommandWriter();

            foreach (var line in ast.Commands)
            {
                cmdBuffer.Add(line.OpCode, line.Args.Select(_ => GetArg(_)).ToArray());
            }

            writer.CreateCodeSection(cmdBuffer);

            File.WriteAllBytes(parameter.Output, writer.Save());

            next(parameter);
        }

        private int GetArg(AsmCommandArg _)
        {
            if (_.Type == ArgType.Literal) return (int)_.Value;
            else if (_.Type == ArgType.Register) return (int)(Registers)Enum.Parse(typeof(Registers), _.Value.ToString(), true);

            return -1;
        }
    }
}