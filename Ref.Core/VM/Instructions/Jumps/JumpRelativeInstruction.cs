using System;

namespace Ref.Core
{
    public class JumpRelativeInstruction : Instruction
    {
        public override OpCode OPCode => OpCode.JMPR;

        public override bool Invoke(VmReader reader, VM vm)
        {
            var val = reader.ReadOperand();
            var value = val.As<int>();

            if (val.Type == OperandType.Value)
            {
                if (value < 0)
                {
                    vm.Register[Registers.IPR] -= Math.Abs(value);
                }
                else
                {
                    vm.Register[Registers.IPR] += value;
                }
            }

            return true;
        }
    }
}