using System;
using System.Runtime.InteropServices;
using Ref.Core.VM.Core.Ports;

namespace Ref.Core.VM.IO.Devices
{
    [Port(0xFFAF, PortAccess.Write)] // control port
    public class VideoDevice : IPortMappedDevice
    {
        public static void Enable_ConsoleMode()
        {
            if (!AttachConsole(ATTACH_PARENT_PROCESS))
            {
                AllocConsole();
            }
        }

        public static void Enable_VideoMode()
        {
            FreeConsole();

            //draw to screen
        }

        public static void Write(char c)
        {
            try
            {
                Console.Write(c);
            }
            catch
            {
                throw new RuntimeException("Cant write to Console in VideoMode");
            }
        }

        public void HandleRead(int port, Registers reg, VirtualMachine vm)
        {
            throw new System.NotImplementedException();
        }

        public void HandleWrite(int port, int value, VirtualMachine vm)
        {
            switch (value)
            {
                case 0:
                    Enable_ConsoleMode();
                    break;

                case 1:
                    Enable_VideoMode();
                    break;
            }
        }

        private const uint ATTACH_PARENT_PROCESS = 0x0ffffffff;

        [DllImport("kernel32")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AttachConsole(uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern bool FreeConsole();
    }
}