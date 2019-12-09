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

            desktopPtr = GetDC(IntPtr.Zero);
            buf._graphics = Graphics.FromHdc(desktopPtr);

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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //ToDo: Enable Property for automatic Flushing to Screen in new Thread
        public void Flush()
        {
            for (int x = 0; x < _rec.Width; x++)
            {
                for (int y = 0; y < _rec.Height; y++)
                {
                    _graphics.FillRectangle(new SolidBrush(Color.FromArgb(this[x, y])), new Rectangle(x, y, 1, 1));
                }
            }
        }

        protected virtual void Dispose(bool disposed)
        {
            if (disposed)
            {
                ReleaseDC(IntPtr.Zero, desktopPtr);
            }
        }

        private static IntPtr desktopPtr;

        private int[] _bufferData;

        private Graphics _graphics;

        private Rectangle _rec;

        ~VideoBuffer()
        {
            Dispose(false);
        }

        [DllImport("User32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("User32.dll")]
        private static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);
    }
}