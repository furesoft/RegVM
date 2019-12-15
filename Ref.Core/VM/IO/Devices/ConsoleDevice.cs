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
        public VirtualMachine VM { get; set; }

        public void HandleMemoryMapped(int address, int value, VirtualMachine vm)
        {
            VideoDevice.Write((char)value);
        }

        public void HandleRead(int port, Registers reg)
        {
            int result = 0;
            switch (port)
            {
                case 0xABC1:
                    result = Console.Read();
                    break;
            }

            VM.Register[reg] = result;
        }

        public void HandleWrite(int addr, int controlVal)
        {
            switch (controlVal)
            {
                case 0:
                    Console.Clear();
                    break;

                case 1:
                    VideoDevice.Write((char)VM.Stack.Pop());
                    break;

                case 2:
                    Console.ForegroundColor = (ConsoleColor)VM.Stack.Pop();
                    break;

                case 3:
                    Console.BackgroundColor = (ConsoleColor)VM.Stack.Pop();
                    break;

                case 4:
                    Console.ResetColor();

                    break;

                case 5:
                    Console.Beep(VM.Stack.Pop(), VM.Stack.Pop());
                    break;
            }
        }
    }
}