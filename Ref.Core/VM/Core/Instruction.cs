namespace Ref.Core
{
    public abstract class Instruction
    {
        public abstract OpCode OPCode { get; }

        public abstract bool Invoke(VmReader reader, VM vm);
    }
}