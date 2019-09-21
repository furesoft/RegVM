namespace Ref.Core
{
    public class DivInstruction : Instruction
    {
        public override OpCode OPCode => OpCode.DIV;

        public override bool Invoke(VmReader reader, VM vm)
        {
            var reg1 = reader.ReadOperand<Registers>();
            var reg2 = reader.ReadOperand<Registers>();
            var regResult = reader.ReadOperand<Registers>();

            var v1 = vm.Register[reg1];
            var v2 = vm.Register[reg2];

            vm.Register[regResult] = v1 / v2;

            vm.Register.ClearRegister(reg1);
            vm.Register.ClearRegister(reg2);

            //set remainder
            vm.Register[Registers.ORE] = v1 % v2;

            return true;
        }
    }
}