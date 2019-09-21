using System;

namespace RefVM
{
    public class JumpInstruction : Instruction
    {
        public override OpCode OPCode => OpCode.JMP;

        public override bool Invoke(VmReader reader, VM vm)
        {
            var addr = reader.ReadQWord();

            vm.SetValue(Registers.IPR, addr);
            //reader.SetPosition(addr);

            return true;
        }
    }
}