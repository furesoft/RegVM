using Ref.Core.VM.Core.Ports;
using Ref.Core.VM.IO.MappedIO;
using System;

namespace Ref.Core.VM.IO.Devices
{
    [Port(0xABC, PortAccess.Write)] //Control Port
    [Port(0xABC1, PortAccess.Read)] // Data Access Port
    [AddressRange(0xA, 0xFF)] // address range for console output
    public class ConsoleDevice : IPortMappedDevice, IMemoryMappedDevice
    {
        public void HandleMemoryMapped(int address, int value, VirtualMachine vm)
        {
            VideoDevice.Write((char)value);
        }

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
                    VideoDevice.Write((char)vm.Stack.Pop());
                    break;

                case 2:
                    Console.ForegroundColor = (ConsoleColor)vm.Stack.Pop();
                    break;

                case 3:
                    Console.BackgroundColor = (ConsoleColor)vm.Stack.Pop();
                    break;

                case 4:
                    Console.ResetColor();

                    break;

                case 5:
                    Console.Beep(vm.Stack.Pop(), vm.Stack.Pop());
                    break;
            }
        }
    }
}