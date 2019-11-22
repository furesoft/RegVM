using Ref.Core.VM.Core.MemoryImpl;
using Ref.Core.VM.IO;

namespace Ref.Core
{
    public class Stack
    {
        public int Length { get; private set; }
        public Memory Memory => new StackMemory(_data);
        public int Position { get; private set; }

        public Stack(int capacity = 50)
        {
            _data = new int?[capacity];
            Position = -1;
        }

        public void Clear()
        {
            for (int i = 0; i < Position; i++)
            {
                _data[i] = null;
                Position = -1;
            }
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
            _data[++Position] = value;
            Length++;
        }

        private int?[] _data;
    }
}