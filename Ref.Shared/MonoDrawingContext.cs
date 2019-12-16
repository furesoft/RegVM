using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ref.Core.VM;
using Ref.Core.VM.Core;
using System;
using System.Linq;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Ref.Shared
{
    public class MonoDrawingContext : IDrawingContext
    {
        public static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Func<int, Color> paint)
        {
            //initialize a texture
            Texture2D texture = new Texture2D(device, width, height);

            //the array holds the color for each pixel in the texture
            Color[] data = new Color[width * height];
            for (int pixel = 0; pixel < data.Count(); pixel++)
            {
                //the function applies the color according to the specified pixel
                data[pixel] = paint(pixel);
            }

            //set the color
            texture.SetData(data);

            return texture;
        }

        public void Cleanup()
        {
            _game.Exit();
        }

        public void Init(System.Drawing.Rectangle rec)
        {
            _game = new Game1();
            _game.Run();
        }

        public void SetPixel(System.Drawing.Point pos, Pixel color)
        {
            var tex = CreateTexture(_game.GraphicsDevice, 1, 1, _ =>
             {
                 return new Color((uint)color.ToHex());
             });

            _game.spriteBatch.Draw(tex, new Rectangle(pos.X, pos.Y, 1, 1), new Color((uint)color.ToHex()));
        }

        private Game1 _game;
    }
}