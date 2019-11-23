namespace Ref.Core.VM.Core.Interrupts
{
    public interface IInterruptHandler
    {
        void Handle(VirtualMachine vm);
    }
}