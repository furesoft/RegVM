using System;

namespace Ref.Core
{
    public class PopInstruction : Instruction
    {
        public override OpCode OPCode => OpCode.POP;

        public override bool Invoke(VmReader reader, VM vm)
        {
            var value = vm.Stack.Pop();
            var reg = reader.ReadOperand<Registers>();

            vm.Register[reg] = (int)value;

            return true;
        }
    }
}