using System;
using System.Globalization;

namespace MappingTiles
{
    public struct Rectangle
    {
        private int x;
        private int y;
        private int width;
        private int height;

        public Rectangle(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return height == 0 && width == 0 && x == 0 && y == 0;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Rectangle))
            {
                return false;
            }

            Rectangle comp = (Rectangle)obj;
            return (comp.X == this.X) && (comp.Y == this.Y) && (comp.Width == this.Width) && (comp.Height == this.Height);
        }

        public static bool operator ==(Rectangle left, Rectangle right)
        {
            return (left.X == right.X && left.Y == right.Y && left.Width == right.Width && left.Height == right.Height);
        }

        public static bool operator !=(Rectangle left, Rectangle right)
        {
            return !(left == right);
        }

        public bool Contains(int x, int y)
        {
            return this.X <= x && x < this.X + this.Width && this.Y <= y && y < this.Y + this.Height;
        }

        public bool Contains(Rectangle rect)
        {
            return (this.X <= rect.X) && ((rect.X + rect.Width) <= (this.X + this.Width)) && (this.Y <= rect.Y) && ((rect.Y + rect.Height) <= (this.Y + this.Height));
        }

        public override int GetHashCode()
        {
            return (int)((UInt32)X ^ (((UInt32)Y << 13) | ((UInt32)Y >> 19)) ^ (((UInt32)Width << 26) | ((UInt32)Width >> 6)) ^ (((UInt32)Height << 7) | ((UInt32)Height >> 25)));
        }

        public bool IsIntersected(Rectangle rect)
        {
            return (rect.X < this.X + this.Width) && (this.X < (rect.X + rect.Width)) && (rect.Y < this.Y + this.Height) && (this.Y < rect.Y + rect.Height);
        }

        public void Offset(int x, int y)
        {
            this.X += x;
            this.Y += y;
        }

        public override string ToString()
        {
            return "{X=" + X.ToString(CultureInfo.CurrentCulture) + ",Y=" + Y.ToString(CultureInfo.CurrentCulture) +
                    ",Width=" + Width.ToString(CultureInfo.CurrentCulture) + ",Height=" + Height.ToString(CultureInfo.CurrentCulture) + "}";
        }
    }
}
