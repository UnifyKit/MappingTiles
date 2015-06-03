using System;
using System.Globalization;

namespace MappingTiles
{
    public class TileInfo
    {
        private int width;
        private int height;
        private ZoomLevel zoomLevel;
        private TileSchema tileSchema;

        protected TileInfo()
        { }

        public TileInfo(BoundingBox boundingBox, TileSchema tileSchema)
        {
            Width = 256;
            Height = 256;
            BoundingBox = boundingBox;
            Schema = tileSchema;

            ZoomLevel = GetZoomLevel();
            InitilizeColumnRowWithBounds();
        }

        public TileInfo(int column, int row, double resolution)
            : this(column, row, resolution, new Wgs84TileSchema())
        { }

        public TileInfo(int column, int row, double resolution, TileSchema tileSchema)
        {
            Width = 256;
            Height = 256;
            ZoomLevel = new ZoomLevel(resolution);
            BoundingBox = GetBoundingBoxByColumnRow(column, row);
        }

        public string Id
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}_{1}_{2}", Column, Row, ZoomLevel.Id);
            }
        }

        public int Width
        {
            get { return width; }
            set
            {
                width = value;
                TileSizePropertyChanged();
            }
        }

        public int Height
        {
            get { return height; }
            set
            {
                height = value;
                TileSizePropertyChanged();
            }
        }

        public BoundingBox BoundingBox
        {
            get;
            private set;
        }

        public int Column
        {
            get;
            private set;
        }

        public int Row
        {
            get;
            private set;
        }

        public ZoomLevel ZoomLevel
        {
            get { return zoomLevel; }
            private set { zoomLevel = value; }
        }

        public TileSchema Schema
        {
            get { return tileSchema; }
            private set { tileSchema = value; }
        }

        public Pixel GetViewPortPosition(int viewPortWidth, int viewPortHeight)
        {
            InternalChecker.CheckParameterIsNull(Schema, "Schema");
            InternalChecker.CheckParameterIsNull(ZoomLevel, "ZoomLevel");

            double pixelX = BoundingBox.MinX / ZoomLevel.Resolution;
            double pixelY = BoundingBox.MaxX / ZoomLevel.Resolution;

            return new Pixel((float)pixelX, (float)pixelY);
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

        private void InitilizeColumnRowWithBounds()
        {
            double worldTileWidth = Width * ZoomLevel.Resolution;
            double worldTileHeight = Height * ZoomLevel.Resolution;

            Column = (int)Math.Floor(BoundingBox.MinX - Schema.BoundingBox.MinX / worldTileWidth);
            if (Schema.IsYAxisReversed)
            {
                Row = (int)Math.Floor((-BoundingBox.MaxY + Schema.BoundingBox.MaxY) / worldTileHeight);
            }
            else
            {
                Row = (int)Math.Floor((BoundingBox.MinY - Schema.BoundingBox.MinY) / worldTileHeight);
            }
        }

        private void TileSizePropertyChanged()
        {
            ZoomLevel = GetZoomLevel();
            BoundingBox = GetBoundingBoxByColumnRow(Column, Row);
        }
    }
}
