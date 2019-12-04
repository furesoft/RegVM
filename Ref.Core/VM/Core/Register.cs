using System;
using Ref.Core.VM;
using Ref.Core.VM.IO;

namespace Ref.Core
{
    [Error(0x1, "The Register is protected")]
    public struct Register : IMemory
    {
        public Action<int> OnChange;
        public RegisterAccess Access { get; set; }
        public Memory Memory => new Memory();
        public VirtualMachine Vm { get; set; }

        [Flags]
        public enum RegisterAccess { Read, Write, Protected }

        public int GetValue()
        {
            if (Access.HasFlag(RegisterAccess.Read))
            {
                return value;
            }

            return 0;
        }

        public void Increment()
        {
            value++;
        }

        public void SetValue(int value)
        {
            if (Access.HasFlag(RegisterAccess.Write))
            {
                if (!Access.HasFlag(RegisterAccess.Protected))
                {
                    this.value = value;

                    OnChange?.Invoke(value);
                }
                else
                {
                    Vm.Register[Registers.ERR] = 1;
                }
            }
            else
            {
                Vm.Register[Registers.ERR] = 1;
            }
        }

        public override string ToString()
        {
            return value.ToString();
        }

        internal int value;
    }
}