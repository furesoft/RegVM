using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Ref.Core.VM.Core.Ports;
using Ref.Core.VM.IO.MappedIO;

namespace Ref.Core.VM.IO.Devices
{
    [Port(0xFFAF, PortAccess.Write)] // control port
    [Port(0xFFAA, PortAccess.Write)] // Init Port (Size)
    [Port(0xFFAB, PortAccess.Write)] // Init Port (Location)
    [AddressRange(0xFFFF, 0xFFFFFF)] // screen buffer range
    public class VideoDevice : IPortMappedDevice, IMemoryMappedDevice
    {
        public static void CleanUP()
        {
            ReleaseDC(IntPtr.Zero, desktopPtr);
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
            FreeConsole();

            //draw to screen
            desktopPtr = GetDC(IntPtr.Zero);
            graphics = Graphics.FromHdc(desktopPtr);

            //draw background
            graphics.FillRectangle(Brushes.Black, bgRec);
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
            short x = BitConverter.ToInt16(bytes, 0);
            short y = BitConverter.ToInt16(bytes, 2);

            graphics.DrawRectangle(new Pen(Color.FromArgb(value), 1), new Rectangle(new Point(x + bgRec.X, y + bgRec.Y), new Size(10, 10))); //Todo: change to real pixel
        }

        public void HandleRead(int port, Registers reg, VirtualMachine vm)
        {
            throw new System.NotImplementedException();
        }

        public void HandleWrite(int port, int value, VirtualMachine vm)
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
        private static IntPtr desktopPtr;
        private static Graphics graphics;
        private Rectangle bgRec;

        [DllImport("kernel32")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AttachConsole(uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern bool FreeConsole();

        [DllImport("User32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("User32.dll")]
        private static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);
    }
}