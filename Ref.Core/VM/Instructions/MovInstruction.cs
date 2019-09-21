namespace RefVM
{
    public class MovInstruction : Instruction
    {
        public override OpCode OPCode => OpCode.MOV;

        public override bool Invoke(VmReader reader, VM vm)
        {
            var reg = reader.ReadOperand();
            var val = reader.ReadOperand();

            if (reg.Type == OperandType.Register && val.Type == OperandType.Value)
            {
                vm.SetValue((Registers)reg.Value, (int)val.Value);
            }
            if (reg.Type == OperandType.Register && val.Type == OperandType.Register)
            {
                vm.SetValue((Registers)reg.Value, vm.GetValue((Registers)val.Value));
            }

            return true;
        }
    }
}