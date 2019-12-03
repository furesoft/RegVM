using System;
using System.IO;
using PipelineNet.Middleware;
using Ref.Core;

namespace Ref_Compiler.MiddleWare
{
    public class EmitMiddleWare : IMiddleware<Options>
    {
        public void Run(Options parameter, Action<Options> next)
        {
            var ast = (AssemblySource)parameter.Tags["AST"];
            var writer = new VmWriter();

            foreach (var line in ast.Lines)
            {
                writer.Write(line.Opcode);

                foreach (var ops in line.Operands)
                {
                    writer.Write(ops);
                }
            }

            File.WriteAllBytes(parameter.Output, writer.ToArray());

            next(parameter);
        }
    }
}