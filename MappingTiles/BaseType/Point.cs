using System;
using System.Runtime.Serialization;

namespace MappingTiles
{
    [DataContract]
    public class Point : IComparable<Point>, IConable, IEquatable<Point>
    {
        private bool isEmpty;
        private double x;
        private double y;

        /// <summary>
        /// Creates a new empty Point.
        /// </summary>
        public Point()
            : this(0, 0)
        {
            isEmpty = true;
        }

        /// <summary>
        /// Initializes a new Point.
        /// </summary>
        /// <param name="x">X (longitude) coordinate</param>
        /// <param name="y">Y (latitude) coordinate</param>
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Gets or sets the X (longitude) coordinate of the point.
        /// </summary>
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                if (x != 0)
                {
                    isEmpty = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Y (latitude) coordinate of the point.
        /// </summary>
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
                if (y != 0)
                {
                    isEmpty = false;
                }
            }
        }

        /// <summary>
        /// Gets whether the point is empty. 
        /// </summary>
        public bool IsEmpty
        {
            get { return isEmpty; }
        }

        /// <summary>
        /// Returns part of coordinate. Index 0 = X, Index 1 = Y.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual double this[uint index]
        {
            get
            {
                if (index == 0)
                {
                    return X;
                }
                else if (index == 1)
                {
                    return Y;
                }
                else
                {
                    throw new IndexOutOfRangeException(ApplicationMessages.ValueOutOfRange);
                }
            }
            set
            {
                if (index == 0)
                {
                    X = value;
                }
                else if (index == 1)
                {
                    Y = value;
                }
                else
                {
                    throw new IndexOutOfRangeException(ApplicationMessages.ValueOutOfRange);
                }
                isEmpty = false;
            }
        }

        /// <summary>
        /// point1 + point2
        /// </summary>
        /// <param name="point1">Vector</param>
        /// <param name="point2">Vector</param>
        /// <returns></returns>
        public static Point operator +(Point point1, Point point2)
        {
            return new Point(point1.X + point2.X, point1.Y + point2.Y);
        }

        /// <summary>
        /// point1 - point2
        /// </summary>
        /// <param name="point1">Vector</param>
        /// <param name="point2">Vector</param>
        /// <returns>Cross product</returns>
        public static Point operator -(Point point1, Point point2)
        {
            return new Point(point1.X - point2.X, point1.Y - point2.Y);
        }

        /// <summary>
        /// Vector * Scaling
        /// </summary>
        /// <param name="point">Vector</param>
        /// <param name="scaling">Scalar (double)</param>
        /// <returns></returns>
        public static Point operator *(Point point, double scaling)
        {
            return new Point(point.X * scaling, point.Y * scaling);
        }

        public int CompareTo(Point other)
        {
            if (X < other.X || X == other.X && Y < other.Y)
            {
                return -1;
            }

            if (X > other.X || X == other.X && Y > other.Y)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public object Clone()
        {
            return new Point(X, Y);
        }

        /// <summary>
        /// Provides a hash algorithm.
        /// </summary>
        /// <returns>A hash code for the current <see cref="GetHashCode"/>.</returns>
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ isEmpty.GetHashCode();
        }

        public bool Equals(Point other)
        {
            return CompareTo(other) == 1;
        }
    }
}
