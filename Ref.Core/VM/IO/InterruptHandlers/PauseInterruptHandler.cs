using Ref.Core.VM.Core.Interrupts;
using System;

namespace Ref.Core.VM.IO.InterruptHandlers
{
    [Interrupt(0x6A03E)]
    public class PauseInterruptHandler : IInterruptHandler
    {
        public void Handle(VirtualMachine vm)
        {
            Console.ReadLine();
        }
    }
}