using System;
using System.Collections.Generic;
using System.Text;
using Ref.Core.Parser;
using Ref.Core.VM.Core.Ports;

namespace Ref.Core.VM.Instructions
{
    internal class OutInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.OUT;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var out_addr = (int)cmd[0];
            var out_value = (int)cmd[1];

            PortMappedDeviceManager.Write(out_addr, out_value, this);
        }
    }
}