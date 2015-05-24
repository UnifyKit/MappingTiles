using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MappingTiles
{
    public class Tile
    {
        public Tile(int column, int row, double resolution)
        {
            Width = 256;
            Height = 256;
            ZoomLevel = new ZoomLevel(resolution);
            BoundingBox = GetBoundingBoxByColumnRow(column, row);
        }

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public BoundingBox BoundingBox
        {
            get;
            set;
        }

        public int Column
        {
            get;
            set;
        }

        public int Row
        {
            get;
            set;
        }

        public ZoomLevel ZoomLevel
        {
            get;
            set;
        }

        public TileSchema Schema
        {
            get;
            private set;
        }

        public Pixel GetViewPortPosition(int viewPortWidth, int viewPortHeight)
        {
            InternalChecker.CheckParameterIsNull(Schema, "Schema");
            InternalChecker.CheckParameterIsNull(ZoomLevel, "ZoomLevel");

            Pixel pixel = null;



            return pixel;
        }

        private ZoomLevel GetZoomLevel()
        {
            InternalChecker.CheckParameterIsNull(Schema, "Schema");

            double resolutionX = BoundingBox.Width / Width;
            double resolutionY = BoundingBox.Height / Height;
            double resolution = Math.Max(resolutionX, resolutionY);

            return Schema.GetNearestZoomLevel(resolution);
        }

        private BoundingBox GetBoundingBoxByColumnRow(int column, int row)
        {
            double worldTileWidth = Width * ZoomLevel.Resolution;
            double worldTileHeight = Height * ZoomLevel.Resolution;

            double minX = Schema.BoundingBox.MinX + column * worldTileWidth;
            double maxX = minX + worldTileWidth;

            double maxY;
            if (Schema.IsYAxisReversed)
            {
                maxY = Schema.BoundingBox.MaxY - row * worldTileHeight;
            }
            else
            {
                maxY = Schema.BoundingBox.MinY + row * worldTileHeight;
            }
            double minY = maxY - worldTileHeight;

            BoundingBox tileBounds = new BoundingBox(minX, minY, maxX, maxY);
            return tileBounds;
        }
    }
}
