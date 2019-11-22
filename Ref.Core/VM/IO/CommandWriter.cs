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
        public CommandWriter()
        {
            _ms = new MemoryStream();
            _bw = new BinaryWriter(_ms);
        }

        public void Add(OpCode op, params int[] args)
        {
            _bw.Write((int)op);
            _bw.Write(args.Length);

            foreach (var arg in args)
            {
                _bw.Write(arg);
            }
        }

        public int MakeLabel() => (int)_ms.Position;

        public byte[] Save()
        {
            return _ms.ToArray();
        }

        private BinaryWriter _bw;
        private MemoryStream _ms;
    }
}