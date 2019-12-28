using System;
using System.IO;
using System.Linq;
using LibObjectFile.Elf;
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

            var stringtable = writer.Elf.AddSection(new ElfStringTable());
            var meta = new AssemblyInfo();

            var roStrm = new MemoryStream();
            var bw = new BinaryWriter(roStrm);

            foreach (var ro in ast.DataCommands)
            {
                if (ro.Name == "db")
                {
                    if (ro.Args.First().Value.ToString() == "msg")
                    {
                        stringtable.GetOrCreateIndex(ro.Args[1].Value.ToString());

                        continue;
                    }
                    foreach (var arg in ro.Args)
                    {
                        var value = GetArg(arg);
                        bw.Write(value);
                    }
                }
                if (ro.Name == "meta")
                {
                    switch (ro.Args.First().Value)
                    {
                        case "id":
                            meta.ID = Guid.Parse(ro.Args[1].Value.ToString());
                            break;

                        case "version":
                            meta.Version = ro.Args[1].Value.ToString();
                            break;

                        case "name":
                            meta.Name = ro.Args[1].Value.ToString();
                            break;
                    }
                }
            }

            writer.CreateMetaSection(meta);
            writer.Elf.AddSection(new ElfCustomSection(roStrm)).ConfigureAs(ElfSectionSpecialType.ReadOnlyData);

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