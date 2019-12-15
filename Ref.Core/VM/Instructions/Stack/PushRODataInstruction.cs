using Ref.Core.Parser;
using Ref.Core.VM.IO;
using System;

namespace Ref.Core.VM.Instructions
{
    internal class PushRODataInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.PUSHRO;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var startIndex = (int)cmd[0];

            /*var section = vm.Assembly[AssemblySections.ReadOnly];
            var value = BitConverter.ToInt32(section.Raw, startIndex);

            vm.Stack.Push(value);*/
        }
    }
}