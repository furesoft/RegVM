using System;

namespace Ref.Core
{
    public class PushInstruction : Instruction
    {
        public override OpCode OPCode => OpCode.PUSH;

        public override bool Invoke(VmReader reader, VM vm)
        {
            throw new NotImplementedException();
        }
    }
}