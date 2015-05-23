using System;
using System.Runtime.Serialization;

namespace MappingTiles
{
    /// <summary>
    /// This class represents a screen coordinate, in x and y coordinates
    /// </summary>
    [DataContract]
    public class Pixel : IConable, IEquatable<Point>
    {
        /// <summary>
        /// Initializes a new instance of the Pixel object.
        /// </summary>
        public Pixel()
            :this(0,0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Pixel object.
        /// </summary>
        /// <param name="x">The Pixel's X coordinate</param>
        /// <param name="y">The Pixel's Y coordinate</param>
        /// <seealso cref="Pixel.X"/>
        /// <seealso cref="Pixel.Y"/>
        public Pixel(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Gets or sets the Pixel's X coordinate.
        /// </summary>
        [DataMember]
        public int X
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Pixel's Y coordinate.
        /// </summary>
        [DataMember]
        public int Y
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the distance to the pixel point passed in as a parameter.
        /// </summary>
        /// <returns>The pixel point passed in as parameter to calculate the distance to</returns>
        public double DistanceTo (Pixel other)
        {
            return Math.Sqrt(Math.Pow(this.X - other.X, 2) + Math.Pow(this.Y - other.Y, 2));
        }

        public bool Equals(Point other)
        {
            bool equals = false;
            if (other != null)
            {
                equals = ((this.X == other.X && this.Y == other.Y)
                    || (double.IsNaN(this.X) && double.IsNaN(other.Y) && double.IsNaN(other.X) && double.IsNaN(other.Y)));
            }
            return equals;
        }

        public object Clone()
        {
            return new Pixel(X, Y);
        }

        public override string ToString()
        {
            return string.Format("x={0},y={1}", this.X, this.Y);
        }
    }
}
