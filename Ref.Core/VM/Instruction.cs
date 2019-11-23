using Ref.Core.Parser;

namespace Ref.Core.VM
{
    public abstract class Instruction
    {
        public abstract OpCode OpCode { get; }

        public abstract void Invoke(AsmCommand cmd, VirtualMachine vm);
    }
}