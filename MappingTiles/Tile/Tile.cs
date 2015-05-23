using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MappingTiles
{
    public class Tile
    {
        public Tile()
        {
            TileWidth = 256;
            TileHeight = 256;
        }

        public int TileWidth
        {
            get;
            set;
        }

        public int TileHeight
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

        public TileSchema TileSchema
        {
            get;
            private set;
        }
    }
}
