using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MappingTiles
{
    public abstract class TileSchema
    {
        public TileSchema()
        { }

        public BoundingBox BoundingBox
        {
            get;
            set;
        }

        public TileFormat TileFormat
        {
            get;
            set;
        }

        public Collection<ZoomLevel> ZoomLevels
        {
            get;
            private set;
        }

        public bool IsYAxisReversed
        {
            get;
            set;
        }
    }
}
