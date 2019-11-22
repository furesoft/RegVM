using Ref.Core.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ref.Core.VM.IO
{
    public class CommandWriter
    {
        public List<AsmCommand> Commands { get; set; } = new List<AsmCommand>();

        public void Add(OpCode op, params int[] args)
        {
            Commands.Add(new AsmCommand { OpCode = op, Args = args.Select(_ => new AsmCommandArg { Value = _ }).ToList() });
        }

        public byte[] Save()
        {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);

            foreach (var cmd in Commands)
            {
                bw.Write((int)cmd.OpCode);
                bw.Write(cmd.Args.Count);
                foreach (var arg in cmd.Args)
                {
                    bw.Write(arg.Value);
                }
            }

            return ms.ToArray();
        }
    }
}