namespace Ref.Core
{
    public class PrintRegisterInstruction : Instruction
    {
        public override OpCode OPCode => OpCode.PNT;

        public override bool Invoke(VmReader reader, VM vm)
        {
            Utils.PrintRegisters(vm.Register);

            return true;
        }
    }
}