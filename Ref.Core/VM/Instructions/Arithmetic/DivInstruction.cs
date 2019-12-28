using Ref.Core.Parser;

namespace Ref.Core.VM.Instructions
{
    internal class DivInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.DIV;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var add_left = vm.Register[(Registers)(int)cmd[0]];
            var add_right = vm.Register[(Registers)(int)cmd[1]];

            vm.Register[Registers.ACC] = add_left / add_right;
            vm.Register[Registers.REM] = add_left % add_right;
        }
    }
}