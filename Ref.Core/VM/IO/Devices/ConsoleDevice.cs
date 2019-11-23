using Ref.Core.VM.Core.Ports;
using System;

namespace Ref.Core.VM.IO.Devices
{
    [Port(0xABC, PortAccess.Write)] //Control Port
    [Port(0xABC1, PortAccess.Read)] // Data Access Port
    public class ConsoleDevice : IPortMappedDevice
    {
        public void HandleRead(int port, Registers reg, VirtualMachine vm)
        {
            int result = 0;
            switch (port)
            {
                case 0xABC1:
                    result = Console.Read();
                    break;
            }

            vm.Register[reg] = result;
        }

        public void HandleWrite(int addr, int controlVal, VirtualMachine vm)
        {
            switch (controlVal)
            {
                case 0:
                    Console.Clear();
                    break;

                case 1:
                    Console.Write((char)vm.Stack.Pop());
                    break;
            }
        }
    }
}