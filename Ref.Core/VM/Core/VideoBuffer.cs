using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Ref.Core.VM.Core
{
    public class VideoBuffer : IDisposable
    {
        public int this[int x, int y]
        {
            get
            {
                return _bufferData[x + (y * _rec.Width)];
            }
            set
            {
                _bufferData[x + (y * _rec.Width)] = value;
            }
        }

        public static VideoBuffer Create(Rectangle rec)
        {
            var buf = new VideoBuffer();
            buf._rec = rec;
            buf._bufferData = new int[rec.Width * rec.Height * 4];

            desktopPtr = GetDesktopWindow();
            buf._graphics = Graphics.FromHwnd(desktopPtr);

            return buf;
        }

        public void Clear()
        {
            for (int i = 0; i < _bufferData.Length; i++)
            {
                _bufferData[i] = 0x000000;
            }
        }

        public void Dispose()
        {
            ReleaseDC(IntPtr.Zero, desktopPtr);
        }

        public void Flush()
        {
            //ToDo: implement Flush Method on VideoBuffer to draw buffer on screen
        }

        private static IntPtr desktopPtr;
        private int[] _bufferData;
        private Graphics _graphics;
        private Rectangle _rec;

        [DllImport("user32.dll", SetLastError = false)]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("User32.dll")]
        private static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);
    }
}