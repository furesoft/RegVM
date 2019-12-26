using Ref.Core.Parser;

namespace Ref.Core.VM.Instructions
{
    internal class NEqualInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.NEQUAL;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var neq_f = (Registers)(int)cmd[0];
            var neq_s = (Registers)(int)cmd[1];

            vm.Register[Registers.BRR] = neq_f != neq_s ? 1 : 0;
        }
    }
}