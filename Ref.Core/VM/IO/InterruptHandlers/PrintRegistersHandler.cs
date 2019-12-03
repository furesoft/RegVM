using Ref.Core.VM.Core.Interrupts;
using System;
using System.Threading;

namespace Ref.Core.VM.IO.InterruptHandlers
{
    [Interrupt(0x123)]
    internal class PrintRegistersHandler : IInterruptHandler
    {
        public void Handle(VirtualMachine vm)
        {
            Thread.Sleep(1500); //for demo propose only, should be removed
            Console.Clear();

            Utils.PrintRegisters(vm.Register);
        }
    }
}