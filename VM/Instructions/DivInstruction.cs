namespace RefVM
{
    public class DivInstruction : Instruction
    {
        public override OpCode OPCode => OpCode.DIV;

        public override bool Invoke(VmReader reader, VM vm)
        {
            var reg1 = reader.ReadOperand();
            var reg2 = reader.ReadOperand();
            var regResult = reader.ReadOperand();

            var v1 = vm.GetValue((Registers)reg1.Value);
            var v2 = vm.GetValue((Registers)reg2.Value);

            vm.SetValue((Registers)regResult.Value, v1 / v2);
            vm.ClearRegister((Registers)reg1.Value);
            vm.ClearRegister((Registers)reg2.Value);

            //set remainder
            vm.SetValue(Registers.ORE, v1 % v2);

            return true;
        }
    }
}