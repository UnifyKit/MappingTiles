using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
