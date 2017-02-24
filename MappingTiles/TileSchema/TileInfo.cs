using System;
using System.Globalization;

namespace MappingTiles
{
    public class TileInfo
    {
        private int tileX;
        private int tileY;
        private ZoomLevel zoomLevel;
        private int width;
        private int height;

        private TileSchema tileSchema;

        private byte[] content;
        private Pixel drawingPosition;

        protected TileInfo()
        { }

        public TileInfo(BoundingBox extent, TileSchema tileSchema)
        {
            this.tileX = -1;
            this.tileY = -1;

            this.width = 256;
            this.height = 256;
            this.Extent = extent;
            this.TileSchema = tileSchema;

            this.ZoomLevel = GetZoomLevel();
            this.InitilizeColumnRowWithBounds();
        }

        public TileInfo(int column, int row, double resolution)
            : this(column, row, resolution, new SphericalMercatorTileSchema())
        { }

        public TileInfo(int column, int row, double resolution, TileSchema tileSchema)
        {
            this.tileX = -1;
            this.tileY = -1;

            this.width = 256;
            this.height = 256;
            this.zoomLevel = new ZoomLevel(resolution);
            this.Extent = GetBoundingBoxByColumnRow(column, row);
        }

        public string Id
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}_{1}_{2}", TileX, TileY, ZoomLevel.Id);
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

        public BoundingBox Extent
        {
            get;
            private set;
        }

        public int TileX
        {
            get { return tileX; }
        }

        public int TileY
        {
            get { return tileY; }
        }

        public ZoomLevel ZoomLevel
        {
            get { return zoomLevel; }
            private set { zoomLevel = value; }
        }

        public byte[] Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
            }
        }

        public Pixel DrawingPosition
        {
            get
            {
                return drawingPosition;
            }
            set
            {
                drawingPosition = value;
            }
        }

        public TileSchema TileSchema
        {
            get
            {
                return tileSchema;
            }
            set
            {
                tileSchema = value;
            }
        }

        public Pixel GetDrawingPosition(int viewPortWidth, int viewPortHeight)
        {
            InternalChecker.CheckParameterIsNull(ZoomLevel, "ZoomLevel");

            double pixelX = Extent.MinX / ZoomLevel.Resolution;
            double pixelY = Extent.MaxX / ZoomLevel.Resolution;

            return new Pixel((float)pixelX, (float)pixelY);
        }

        private ZoomLevel GetZoomLevel()
        {
            InternalChecker.CheckParameterIsNull(TileSchema, "Schema");

            double resolutionX = Extent.Width / Width;
            double resolutionY = Extent.Height / Height;
            double resolution = Math.Max(resolutionX, resolutionY);

            return TileSchema.GetNearestZoomLevel(resolution);
        }

        private BoundingBox GetBoundingBoxByColumnRow(int column, int row)
        {
            double worldTileWidth = Width * ZoomLevel.Resolution;
            double worldTileHeight = Height * ZoomLevel.Resolution;

            double minX = TileSchema.MaxExtent.MinX + column * worldTileWidth;
            double maxX = minX + worldTileWidth;

            double maxY;
            if (TileSchema.IsYAxisReversed)
            {
                maxY = TileSchema.MaxExtent.MaxY - row * worldTileHeight;
            }
            else
            {
                maxY = TileSchema.MaxExtent.MinY + row * worldTileHeight;
            }
            double minY = maxY - worldTileHeight;

            BoundingBox tileBounds = new BoundingBox(minX, minY, maxX, maxY);
            return tileBounds;
        }

        private void InitilizeColumnRowWithBounds()
        {
            double worldTileWidth = Width * ZoomLevel.Resolution;
            double worldTileHeight = Height * ZoomLevel.Resolution;

            tileX = (int)Math.Floor(Extent.MinX - TileSchema.MaxExtent.MinX / worldTileWidth);
            if (TileSchema.IsYAxisReversed)
            {
                tileY = (int)Math.Floor((-Extent.MaxY + TileSchema.MaxExtent.MaxY) / worldTileHeight);
            }
            else
            {
                tileY = (int)Math.Floor((Extent.MinY - TileSchema.MaxExtent.MinY) / worldTileHeight);
            }
        }

        private void TileSizePropertyChanged()
        {
            ZoomLevel = GetZoomLevel();
            Extent = GetBoundingBoxByColumnRow(TileX, TileY);
        }
    }
}
