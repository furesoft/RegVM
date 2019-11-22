using System;
using System.Collections.Generic;

namespace Ref.Core.VM.IO.MemoryImpl
{
    internal class RegisterMemory : Memory
    {
        public override int Length => register.Length * sizeof(int);

        public RegisterMemory(Register[] register)
        {
            this.register = register;
        }

        public override void Clear()
        {
            for (int i = 0; i < register.Length; i++)
            {
                register[i].SetValue(0);
            }
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

        public override int GetValue(int index)
        {
            return register[index].GetValue();
        }

        public override void SetValue(int index, int value)
        {
            register[index].SetValue(value);
        }

        private Register[] register;
    }
}