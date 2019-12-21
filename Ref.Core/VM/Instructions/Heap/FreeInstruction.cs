using Ref.Core.Parser;
using Ref.Core.VM.Core;
using System;

namespace Ref.Core.VM.Instructions
{
    internal class FreeInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.FREE;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            unsafe
            {
                var ptr = new IntPtr(vm.Register[Registers.HEAP]);

                Heap.Free(ptr.ToPointer());
            }
        }
    }
}