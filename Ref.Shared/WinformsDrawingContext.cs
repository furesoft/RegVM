using Ref.Core.VM;
using Ref.Core.VM.Core;
using System.Drawing;
using System.Windows.Forms;

namespace TestConsole
{
    public class WinformsDrawingContext : IDrawingContext
    {
        public void Cleanup()
        {
            Application.Exit();
        }

        public void Init(Rectangle rec)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _form = new Form();
            _form.Size = rec.Size;
            _form.Location = rec.Location;
            _form.FormBorderStyle = FormBorderStyle.None;
            _graphics = _form.CreateGraphics();

            Application.Run(_form);
        }

        public void SetPixel(Point pos, Pixel color)
        {
            _graphics.FillRectangle(new SolidBrush(Color.FromArgb(color.ToHex())), new Rectangle(pos, new Size(1, 1)));
        }

        private Form _form;
        private Graphics _graphics;
    }
}