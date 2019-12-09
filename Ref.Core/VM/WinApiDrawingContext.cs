using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Ref.Core.VM
{
    public class WinApiDrawingContext : IDrawingContext
    {
        public void Cleanup()
        {
            ReleaseDC(IntPtr.Zero, desktopPtr);
        }

        public void Init(Rectangle rec)
        {
            desktopPtr = GetDC(IntPtr.Zero);
            _graphics = Graphics.FromHdc(desktopPtr);
        }

        public void SetPixel(Point pos, Color color)
        {
            _graphics.FillRectangle(new SolidBrush(Color.FromArgb(color)), new Rectangle(pos.X, pos.Y, 1, 1));
        }

        private static IntPtr desktopPtr;
        private Graphics _graphics;

        [DllImport("User32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("User32.dll")]
        private static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);
    }
}