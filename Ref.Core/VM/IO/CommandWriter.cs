using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Ref.Core.VM.IO
{
    public class CommandWriter : IEnumerable
    {
        public CommandWriter()
        {
            _ms = new MemoryStream();
            _bw = new BinaryWriter(_ms);
        }

        public void Add(OpCode op, Registers reg, params int[] value)
        {
            var args = new List<int>();
            args.Add((int)reg);
            args.AddRange(value);

            Add(op, args.ToArray());
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

        public IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException();
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