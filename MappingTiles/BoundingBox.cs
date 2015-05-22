using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MappingTiles
{
    public struct BoundingBox : IEquatable<BoundingBox>
    {
        public BoundingBox(double minX, double minY, double maxX, double maxY)
            : this()
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;

            if (minX > maxX || minY > maxY)
            {
                throw new ArgumentException("min should be smaller than max");
            }
        }

        public double MinX
        {
            get;
            private set;
        }

        public double MinY
        {
            get;
            private set;
        }

        public double MaxX
        {
            get;
            private set;
        }

        public double MaxY
        {
            get;
            private set;
        }

        public double CenterX
        {
            get { return (MinX + MaxX) / 2.0; }
        }

        public double CenterY
        {
            get { return (MinY + MaxY) / 2.0; }
        }

        public double Width
        {
            get { return MaxX - MinX; }
        }

        public double Height
        {
            get { return MaxY - MinY; }
        }

        public double Area
        {
            get { return Width * Height; }
        }

        public BoundingBox Intersect(BoundingBox other)
        {
            return new BoundingBox(
                Math.Max(MinX, other.MinX),
                Math.Max(MinY, other.MinY),
                Math.Min(MaxX, other.MaxX),
                Math.Min(MaxY, other.MaxY));
        }

        public bool Intersects(BoundingBox box)
        {
            return !(
                        box.MinX > MaxX ||
                        box.MaxX < MinX ||
                        box.MinY > MaxY ||
                        box.MaxY < MinY);
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture,
                                 "{0},{1},{2},{3}", MinX, MinY, MaxX, MaxY);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BoundingBox))
            {
                return false;
            }
            return Equals((BoundingBox)obj);
        }

        public bool Equals(BoundingBox extent)
        {
            if (MinX != extent.MinX)
            {
                return false;
            }

            if (MinY != extent.MinY)
            {
                return false;
            }

            if (MaxX != extent.MaxX)
            {
                return false;
            }

            if (MaxY != extent.MaxY)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return MinX.GetHashCode() ^ MinY.GetHashCode() ^ MaxX.GetHashCode() ^ MaxY.GetHashCode();
        }

        public static bool operator ==(BoundingBox extent1, BoundingBox extent2)
        {
            return extent1.Equals(extent2);
        }

        public static bool operator !=(BoundingBox extent1, BoundingBox extent2)
        {
            return !extent1.Equals(extent2);
        }
    }
}
