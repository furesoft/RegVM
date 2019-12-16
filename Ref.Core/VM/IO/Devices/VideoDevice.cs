using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Ref.Core.VM.Core;
using Ref.Core.VM.Core.Ports;
using Ref.Core.VM.IO.MappedIO;

namespace Ref.Core.VM.IO.Devices
{
    [Port(0xFFAF, PortAccess.Write)] // control port
    [Port(0xFFAA, PortAccess.Write)] // Init Port (Size)
    [Port(0xFFAB, PortAccess.Write)] // Init Port (Location)
    [AddressRange(0xFFFF, 0xFFFFFFF)] // screen buffer range
    public class VideoDevice : IPortMappedDevice, IMemoryMappedDevice
    {
        public static VideoBuffer Buffer;
        public static IDrawingContext DefaultContext { get; set; } = new WinApiDrawingContext();
        public VirtualMachine VM { get; set; }

        public static void CleanUP()
        {
            Buffer.Dispose();
        }

        public static void Enable_ConsoleMode()
        {
            if (!AttachConsole(ATTACH_PARENT_PROCESS))
            {
                AllocConsole();
            }
        }

        public static void Enable_VideoMode(Rectangle bgRec)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                FreeConsole();

                Buffer = VideoBuffer.Create(bgRec, DefaultContext); //ToDo: enable option to seletc windowed mode
                Buffer.Clear();
                Buffer.Flush(); //ToDo: fix deadlock on flush
            }
            else
            {
                throw new RuntimeException("VideoMode is only available on Windows OS");
            }
        }

        public static void Write(char c)
        {
            try
            {
                Console.Write(c);
            }
            catch
            {
                //throw new RuntimeException("Cant write to Console in VideoMode");
            }
        }

        public void HandleMemoryMapped(int address, int value, VirtualMachine vm)
        {
            byte[] bytes = BitConverter.GetBytes(address);
            ushort x = BitConverter.ToUInt16(bytes, 0);
            ushort y = BitConverter.ToUInt16(bytes, 2);

            Buffer[x, y] = value;
        }

        public void HandleRead(int port, Registers reg)
        {
            throw new System.NotImplementedException();
        }

        public void HandleWrite(int port, int value)
        {
            if (port == 0xFFAF)
            {
                switch (value)
                {
                    case 0:
                        Enable_ConsoleMode();
                        break;

                    case 1:
                        Enable_VideoMode(bgRec);
                        break;

                    case 2:
                        Buffer.Flush();
                        break;
                }
            }
            else if (port == 0xFFAA)
            {
                byte[] bytes = BitConverter.GetBytes(value);
                short width = BitConverter.ToInt16(bytes, 0);
                short height = BitConverter.ToInt16(bytes, 2);

                bgRec = new Rectangle(bgRec.Location, new Size(width, height));
            }
            else if (port == 0xFFAB)
            {
                byte[] bytes = BitConverter.GetBytes(value);
                short x = BitConverter.ToInt16(bytes, 0);
                short y = BitConverter.ToInt16(bytes, 2);

                bgRec = new Rectangle(new Point(x, y), bgRec.Size);
            }
        }

        private const uint ATTACH_PARENT_PROCESS = 0x0ffffffff;
        private Rectangle bgRec;
        private bool windowedMode = false;

        [DllImport("kernel32")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AttachConsole(uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern bool FreeConsole();
    }
}