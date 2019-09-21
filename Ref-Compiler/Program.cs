using CommandLine;
using PipelineNet.MiddlewareResolver;
using PipelineNet.Pipelines;
using Ref_Compiler.MiddleWare;
using Serilog;
using System.Collections.Generic;

namespace Ref_Compiler
{
    internal class Program
    {
        private static void HandleParseError(IEnumerable<Error> errs)
        {
            var log = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            foreach (var e in errs)
            {
                log.Information(e.ToString());
            }
        }

        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(opts => RunOptionsAndReturnExitCode(opts))
                .WithNotParsed((errs) => HandleParseError(errs));
        }

        private static void RunOptionsAndReturnExitCode(Options opts)
        {
            var pipeline = new Pipeline<Options>(new ActivatorMiddlewareResolver());

            pipeline.Add<AstMiddleware>();
            pipeline.Add<EmitMiddleWare>();

            pipeline.Execute(opts);
        }
    }
}