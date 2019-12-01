using Ref.Core.Parser;

namespace Ref.Core.VM.Instructions
{
    internal class MovInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.MOV;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var fromReg = (Registers)(int)cmd[0];
            var toReg = (Registers)(int)cmd[1];

            vm.Register[toReg] = vm.Register[fromReg];
        }
    }
}