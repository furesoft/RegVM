using Ref.Core.Parser;
using Ref.Core.VM.Core;
using System;

namespace Ref.Core.VM.Instructions
{
    internal class StoreInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.STORE;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var addr = vm.Register[Registers.HEAP];

            unsafe
            {
                int* ptr = (int*)new IntPtr(addr);
                *ptr = (int)cmd[0];
            }
        }
    }
}