using Ref.Core.VM.IO;
using Ref.Core.VM.IO.MemoryImpl;
using System;

namespace Ref.Core
{
    public class Stack : IMemory
    {
        public int Length { get; private set; }
        public Memory Memory => new StackMemory(_data);
        public int Position { get; private set; }

        public Stack(int capacity = 500)
        {
            _data = new int?[capacity];
            Position = -1;
        }

        public void Clear()
        {
            Memory.Clear();

            Position = -1;
        }

        public int Pop()
        {
            var pos = Position--;
            Length--;
            _data[pos + 1] = null;

            return _data[pos].Value;
        }

        public void Push(int value)
        {
            if (Position < _data.Length - 1)
            {
                _data[++Position] = value;
                Length++;
                return;
            }

            throw new StackOverflowException();
        }

        private int?[] _data;
    }
}