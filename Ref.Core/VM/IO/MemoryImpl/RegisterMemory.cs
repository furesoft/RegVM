using System;
using System.Collections.Generic;

namespace Ref.Core.VM.IO.MemoryImpl
{
    internal class RegisterMemory : Memory
    {
        public RegisterMemory(Register[] register)
        {
            this.register = register;
        }

        public override byte[] GetMemory()
        {
            var result = new List<byte>();

            foreach (var item in register)
            {
                result.AddRange(BitConverter.GetBytes(item.value));
            }

            return result.ToArray();
        }

        private Register[] register;
    }
}