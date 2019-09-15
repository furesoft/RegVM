namespace RefVM
{
    public class PrintRegisterInstruction : Instruction
    {
        public override OpCode OPCode => OpCode.PNT;

        public override bool Invoke(VmReader reader, VM vm)
        {
            Repl.PrintRegisters(vm.Register);

            return true;
        }
    }
}