﻿namespace Ref.Core
{
    public class DivInstruction : Instruction
    {
        public override OpCode OPCode => OpCode.DIV;

        public override bool Invoke(VmReader reader, VM vm)
        {
            var reg1 = reader.ReadOperand<Registers>();
            var reg2 = reader.ReadOperand<Registers>();
            var regResult = reader.ReadOperand<Registers>();

            var v1 = vm.GetValue(reg1);
            var v2 = vm.GetValue(reg2);

            vm.SetValue(regResult, v1 / v2);
            vm.ClearRegister(reg1);
            vm.ClearRegister(reg2);

            //set remainder
            vm.SetValue(Registers.ORE, v1 % v2);

            return true;
        }
    }
}