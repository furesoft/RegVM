using System;
using System.Collections.Generic;
using System.Text;
using Ref.Core.Parser;
using Ref.Core.VM.Core.Ports;

namespace Ref.Core.VM.Instructions
{
    internal class InInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.IN;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var in_addr = (int)cmd[0];
            var in_reg = (Registers)(int)cmd[1];

            PortMappedDeviceManager.Read(in_addr, in_reg, this);
        }
    }
}