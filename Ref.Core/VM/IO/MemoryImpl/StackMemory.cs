using Ref.Core.VM.IO;
using System;
using System.Collections.Generic;

namespace Ref.Core.VM.IO.MemoryImpl
{
    internal class StackMemory : Memory
    {
        public StackMemory(int?[] data)
        {
            _data = data;
        }

        public override byte[] GetMemory()
        {
            var result = new List<byte>();

            foreach (var item in _data)
            {
                if (item.HasValue)
                {
                    result.AddRange(BitConverter.GetBytes(item.Value));
                }
                else
                {
                    result.Add(0);
                }
            }

            return result.ToArray();
        }

        private int?[] _data;
    }
}