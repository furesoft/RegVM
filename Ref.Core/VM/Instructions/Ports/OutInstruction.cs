using Ref.Core.Parser;
using Ref.Core.VM.Core.MappedIO;
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

            if (PortMappedDeviceManager.IsRegistered(out_addr))
            {
                PortMappedDeviceManager.Write(out_addr, out_value);
            }
            else
            {
                MemoryMappedDeviceManager.Write(out_addr, out_value, vm);
            }
        }
    }
}