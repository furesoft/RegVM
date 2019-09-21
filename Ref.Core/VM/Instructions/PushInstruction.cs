using System;

namespace Ref.Core
{
    public class PushInstruction : Instruction
    {
        public override OpCode OPCode => OpCode.PUSH;

        public override bool Invoke(VmReader reader, VM vm)
        {
            var val = reader.ReadOperand();

            if (val.Type == OperandType.Register)
            {
                var value = vm.Register[val.As<Registers>()];

                vm.Stack.Push(value);
            }

            return true;
        }
    }
}