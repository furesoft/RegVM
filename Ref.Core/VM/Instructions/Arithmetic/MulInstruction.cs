namespace Ref.Core
{
    public class MulInstruction : Instruction
    {
        public override OpCode OPCode => OpCode.MUL;

        public override bool Invoke(VmReader reader, VM vm)
        {
            var reg1 = reader.ReadOperand().As<Registers>();
            var reg2 = reader.ReadOperand().As<Registers>();
            var regResult = reader.ReadOperand().As<Registers>();

            var v1 = vm.Register[reg1];
            var v2 = vm.Register[reg2];

            vm.Register[regResult] = v1 * v2;
            vm.Register.ClearRegister(reg1);
            vm.Register.ClearRegister(reg2);

            return true;
        }
    }
}