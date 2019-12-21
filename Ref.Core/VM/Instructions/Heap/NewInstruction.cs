using Ref.Core.Parser;
using Ref.Core.VM.Core;
using System;

namespace Ref.Core.VM.Instructions
{
    internal class NewInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.NEW;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var size = (int)cmd[0];

            unsafe
            {
                var ptr = new IntPtr(Heap.Alloc(size, "[VM]"));

                vm.Register[Registers.HEAP] = ptr.ToInt32();
            }
        }
    }
}