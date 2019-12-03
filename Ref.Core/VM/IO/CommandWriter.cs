using System.Collections;
using System.IO;

namespace Ref.Core.VM.IO
{
    public class CommandWriter
    {
        public CommandWriter()
        {
            _ms = new MemoryStream();
            _bw = new BinaryWriter(_ms);
        }

        public void Add(OpCode op, Registers reg, int value)
        {
            Add(op, (int)reg, value);
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