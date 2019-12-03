using System;
using System.Linq;

namespace Ref.Core.VM.IO
{
    public class Memory
    {
        public virtual int Length { get; }

        public virtual void Clear()
        {
            throw new NotImplementedException();
        }

        public virtual byte[] GetMemory()
        {
            return null;
        }

        public virtual int GetValue(int index)
        {
            throw new NotImplementedException();
        }

        public virtual void SetValue(int index, int value)
        {
            throw new NotImplementedException();
        }

        public byte[] Slice(int n)
        {
            return GetMemory()?.Take(n).ToArray();
        }
    }
}