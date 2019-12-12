namespace Ref.Core.VM.Core
{
    public class Pixel
    {
        /// <summary>
        /// The Alpha Value
        /// </summary>
        public byte A { get; set; }

        public byte B
        {
            get { return _b; }
            set
            {
                _b = value;
                Hex = ((R << 16) | (G << 8) | B);
            }
        }

        public byte G
        {
            get { return _g; }
            set
            {
                _g = value;
                Hex = ((R << 16) | (G << 8) | B);
            }
        }

        public byte R
        {
            get { return _r; }
            set
            {
                _r = value;
                Hex = ((R << 16) | (G << 8) | B);
            }
        }

        public Pixel(byte r, byte g, byte b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            A = 255;
        }

        public Pixel(byte r, byte g, byte b, byte a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            A = a;
        }

        public Pixel(Pixel c, byte a)
        {
            this.R = c.R;
            this.G = c.G;
            this.B = c.B;
            A = a;
        }

        public Pixel(int hex)
        {
            R = ((byte)(hex >> 16));
            G = ((byte)(hex >> 8));
            B = ((byte)(hex >> 0));
            A = 255;
        }

        public Pixel(uint hex)
        {
            R = ((byte)(hex >> 16));
            G = ((byte)(hex >> 8));
            B = ((byte)(hex >> 0));
            A = 255;
        }

        public Pixel()
        {
        }

        public static implicit operator int(Pixel c)
        {
            return c.ToHex();
        }

        public static implicit operator Pixel(int c)
        {
            return new Pixel(c);
        }

        public static implicit operator Pixel(uint c)
        {
            return new Pixel(c);
        }

        public int ToHex()
        {
            return Hex;
        }

        private byte _b;
        private byte _g;
        private byte _r;

        /// <summary>
        /// Helper Field to hold the color as int
        /// </summary>
        private int Hex { get; set; }
    }
}