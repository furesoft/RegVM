using PipelineNet.Middleware;
using Ref.Core;
using System;
using System.IO;

namespace Ref_Compiler.MiddleWare
{
    public class AstMiddleware : IMiddleware<Options>
    {
        public void Run(Options parameter, Action<Options> next)
        {
            var source = File.ReadAllText(parameter.Input);
            var parsed = AssemblySource.Parse(source);

            parameter.Tags.Add("AST", parsed);

            next(parameter);
        }
    }
}