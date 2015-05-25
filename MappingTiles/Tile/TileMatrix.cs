using System;

namespace MappingTiles
{
    public class TileMatrix
    {
        protected TileMatrix()
        { }

        public TileMatrix(double resolution, TileSchema tileScehma)
            : this(resolution, tileScehma, Utilities.CreateUniqueId())
        { }

        public TileMatrix(double resolution,TileSchema tileSchema, string id)
        {
            this.ZoomLevel = new ZoomLevel(resolution);
            this.TileSchema = tileSchema;
            this.Id = id;
        }

        public string Id
        {
            get;
            private set;
        }

        public TileSchema TileSchema
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the resolution, scale of this tile matrix.
        /// </summary>
        public ZoomLevel ZoomLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the width of the tile in the matrix.
        /// </summary>
        public int TileWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the height of the tile in the matrix.
        /// </summary>
        public int TileHeight
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the width of the whole tile matrix, which covers the world boundingBox of the corresponding tile schema.
        /// </summary>
        public int Width
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the height of the whole tile matrix, which covers the world boundingBox of the corresponding tile schema.
        /// </summary>
        public int Height
        {
            get;
            set;
        }

        public TileRange GetTiles(BoundingBox boundingBox)
        {
            var tileWorldUnits = ZoomLevel.Resolution * TileWidth;
            if (TileSchema.IsYAxisReversed)
            {
                var firstCol = (int)Math.Floor((boundingBox.MinX - TileSchema.BoundingBox.MinX) / tileWorldUnits);
                var firstRow = (int)Math.Floor((-boundingBox.MaxY + TileSchema.BoundingBox.MaxY) / tileWorldUnits);
                var lastCol = (int)Math.Ceiling((boundingBox.MaxX - TileSchema.BoundingBox.MinX) / tileWorldUnits);
                var lastRow = (int)Math.Ceiling((-boundingBox.MinY + TileSchema.BoundingBox.MaxY) / tileWorldUnits);

                return new TileRange(firstCol, firstRow, lastCol - firstCol, lastRow - firstRow);
            }
            else
            {
                var firstCol = (int)Math.Floor((boundingBox.MinX - TileSchema.BoundingBox.MinX) / tileWorldUnits);
                var firstRow = (int)Math.Floor((boundingBox.MinY - TileSchema.BoundingBox.MaxY) / tileWorldUnits);
                var lastCol = (int)Math.Ceiling((boundingBox.MaxX - TileSchema.BoundingBox.MinX) / tileWorldUnits);
                var lastRow = (int)Math.Ceiling((boundingBox.MaxY - TileSchema.BoundingBox.MaxY) / tileWorldUnits);

                return new TileRange(firstCol, firstRow, lastCol - firstCol, lastRow - firstRow);
            }
        }

        public BoundingBox GetTilesBoundingBox(TileRange range)
        {
            var resolution = ZoomLevel.Resolution;
            if (TileSchema.IsYAxisReversed)
            {
                var tileWorldUnits = resolution * TileWidth;
                var minX = range.StartColumn * tileWorldUnits + TileSchema.BoundingBox.MinX;
                var minY = -(range.StartRow+ range.NumberOfRows) * tileWorldUnits + TileSchema.BoundingBox.MaxY;
                var maxX = (range.StartColumn + range.NumberOfColumns) * tileWorldUnits + TileSchema.BoundingBox.MinX;
                var maxY = -(range.StartRow) * tileWorldUnits + TileSchema.BoundingBox.MaxY;

                return new BoundingBox(minX, minY, maxX, maxY);
            }
            else
            {
                var tileWorldUnits = resolution * TileWidth;
                var minX = range.StartColumn * tileWorldUnits + TileSchema.BoundingBox.MinX;
                var minY = range.StartRow * tileWorldUnits + TileSchema.BoundingBox.MaxY;
                var maxX = (range.StartColumn + range.NumberOfColumns) * tileWorldUnits + TileSchema.BoundingBox.MinX;
                var maxY = (range.StartRow + range.NumberOfRows) * tileWorldUnits + TileSchema.BoundingBox.MaxY;

                return new BoundingBox(minX, minY, maxX, maxY);
            }
        }
    }
}
