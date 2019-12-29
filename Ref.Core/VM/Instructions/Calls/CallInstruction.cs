using Ref.Core.Parser;
using Ref.Core.VM.Core;
using System;

namespace Ref.Core.VM.Instructions
{
    internal class CallInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.CALL;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var cll = (int)cmd[0];

            if (vm.Functions.ContainsKey(cll))
            {
                var del = (Delegate)vm.Functions[cll];
                del.DynamicInvoke();
            }
            else
            {
                vm.Stack.PushRegisters(vm.Register);
                vm.Register[Registers.IPR] = cll;
            }
        }
    }
}