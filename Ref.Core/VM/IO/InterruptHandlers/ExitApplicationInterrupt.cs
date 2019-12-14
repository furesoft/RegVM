using Ref.Core.VM.Core.Interrupts;
using System;

namespace Ref.Core.VM.IO.InterruptHandlers
{
    [Interrupt(0x1)]
    internal class ExitApplicationInterruptHandler : IInterruptHandler
    {
        public void Handle(VirtualMachine vm)
        {
            var errCode = vm.Stack.Pop();
            Environment.Exit(errCode);
        }
    }
}