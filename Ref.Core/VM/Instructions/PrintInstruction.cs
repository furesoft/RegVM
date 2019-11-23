using System;
using System.Threading;
using Ref.Core.Parser;

namespace Ref.Core.VM.Instructions
{
    internal class PrintInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.PRINT;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            Thread.Sleep(1500); //for demo propose only, should be removed
            Console.Clear();

            Utils.PrintRegisters(vm.Register);
        }
    }
}