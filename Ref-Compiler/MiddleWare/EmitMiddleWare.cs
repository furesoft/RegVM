using System;
using System.IO;
using System.Linq;
using PipelineNet.Middleware;
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
                cmdBuffer.Add(line.OpCode, line.Args.Select(_ => (int)_.Value).ToArray());
            }

            writer.CreateCodeSection(cmdBuffer);

            File.WriteAllBytes(parameter.Output, writer.Save());

            next(parameter);
        }
    }
}