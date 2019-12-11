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

        public static VideoBuffer Create(Rectangle rec, IDrawingContext drawingContext)
        {
            var buf = new VideoBuffer();
            buf._rec = rec;
            buf._bufferData = new int[rec.Width * rec.Height * 4];

            buf._context = drawingContext;
            buf._context.Init(buf._rec);

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
                    _context.SetPixel(new Point(x, y), Color.FromArgb(this[x, y]));
                }
            }
        }

        public void SetContext(Rectangle rec, IDrawingContext context)
        {
            _context = context;
            _context.Init(rec);
        }

        protected virtual void Dispose(bool disposed)
        {
            if (disposed)
            {
                _context.Cleanup();
            }
        }

        private int[] _bufferData;

        private IDrawingContext _context;

        private Rectangle _rec;

        ~VideoBuffer()
        {
            Dispose(false);
        }
    }
}