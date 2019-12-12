using Ref.Core.VM.Core;
using System.Drawing;

namespace Ref.Core.VM
{
    public interface IDrawingContext
    {
        void Cleanup();

        void Init(Rectangle rec);

        void SetPixel(Point pos, Pixel color);
    }
}