using System;

namespace Ref.Core
{
    public class CompareInstruction : Instruction
    {
        public override OpCode OPCode => OpCode.CMP;

        public override bool Invoke(VmReader reader, VM vm)
        {
            var lhsReg = reader.ReadOperand<Registers>();
            var rhsReg = reader.ReadOperand<Registers>();

            var op = (OperatorType)reader.ReadWord();
            var resultReg = Registers.BRR;

            //ToDo: implement checkinstruction
            bool result = false;

            if (op == OperatorType.EQUAL)
            {
                result = vm.Register[lhsReg] == vm.Register[rhsReg];
            }
            else if (op == OperatorType.NOTEQUAL)
            {
                result = vm.Register[lhsReg] != vm.Register[rhsReg];
            }
            else if (op == OperatorType.LESS)
            {
                result = lhsReg < rhsReg;
            }
            else if (op == OperatorType.LESSEQUAL)
            {
                result = lhsReg <= rhsReg;
            }
            else if (op == OperatorType.GREATER)
            {
                result = lhsReg > rhsReg;
            }
            else if (op == OperatorType.GREATHEREQUAL)
            {
                result = lhsReg <= rhsReg;
            }

            vm.Register[resultReg] = result ? 1 : 0;
            return true;
        }
    }
}