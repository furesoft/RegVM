namespace Ref.Core
{
    public class SubInstruction : Instruction
    {
        public override OpCode OPCode => OpCode.SUB;

        public override bool Invoke(VmReader reader, VM vm)
        {
            var reg1 = reader.ReadOperand();
            var reg2 = reader.ReadOperand();
            var regResult = reader.ReadOperand();

            var v1 = vm.Register[reg1.As<Registers>()];
            var v2 = vm.Register[reg2.As<Registers>()];

            vm.Register[regResult.As<Registers>()] = v1 - v2;

            vm.Register.ClearRegister(reg1.As<Registers>());
            vm.Register.ClearRegister(reg2.As<Registers>());

            return true;
        }
    }
}