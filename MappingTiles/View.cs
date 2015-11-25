using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MappingTiles
{
    public class View
    {
        public View()
        { }

        public View(double width, double height, double resolution)
        {
            this.Width = width;
            this.Height = height;
            this.ZoomLevel = new ZoomLevel(resolution);
        }

        public double Width
        {
            get;
            set;
        }

        public double Height
        {
            get;
            set;
        }

        public Coordinate Center
        {
            get;
            set;
        }

        public BoundingBox BoundingBox
        {
            get;
            set;
        }

        public Collection<ZoomLevel> ZoomLevels
        {
            get;
            private set;
        }
        public ZoomLevel ZoomLevel
        {
            get;
            set;
        }

        public bool RotationEnabled
        {
            get;
            set;
        }

        public double Rotation
        {
            get;
            set;
        }

        public Coordinate ToWorldCoordinate(Pixel pixel)
        {
            double screenCenterX = Width / 2.0;
            double screenCenterY = Height / 2.0;

            if (RotationEnabled)
            {
                Coordinate screen = new Coordinate(pixel.X, pixel.Y).Rotate(Rotation, screenCenterX, screenCenterY);
                pixel.X = (float)screen.X;
                pixel.Y = (float)screen.Y;
            }

            double worldX = Center.X + (pixel.X - screenCenterX) * ZoomLevel.Resolution;
            double worldY = Center.Y - ((pixel.Y - screenCenterY) * ZoomLevel.Resolution);

            return new Coordinate(worldX, worldY);
        }

        public Pixel ToScreenPixel(Coordinate coordinate)
        {
            Pixel pixel = ToScreenPixelUnrotated(coordinate.X, coordinate.Y);
            if (RotationEnabled)
            {
                double screenCenterX = Width * 0.5;
                double screenCenterY = Height * 0.5;

                Coordinate tempCoordinate = new Coordinate(pixel.X, pixel.Y);
                Coordinate rotatedCoordinate = tempCoordinate.Rotate(-Rotation, screenCenterX, screenCenterY);
                pixel = new Pixel((float)rotatedCoordinate.X, (float)rotatedCoordinate.Y);
            }

            return pixel;
        }

        private Pixel ToScreenPixelUnrotated(double worldX, double worldY)
        {
            double screenCenterX = Width / 2.0;
            double screenCenterY = Height / 2.0;
            double screenX = (worldX - Center.X) / ZoomLevel.Resolution + screenCenterX;
            double screenY = (Center.Y - worldY) / ZoomLevel.Resolution + screenCenterY;

            return new Pixel((float)screenX, (float)screenY);
        }
    }
}
