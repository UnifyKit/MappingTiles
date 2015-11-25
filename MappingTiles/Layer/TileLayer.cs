using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MappingTiles
{
    public class TileLayer : Layer
    {
        private TileSource tileSource;

        protected TileLayer(TileSource tileSource, string id)
            : base(id)
        {
        }

        public TileSource TileSource
        {
            get
            {
                return tileSource;
            }
        }

        public override void ClearCache()
        {
            if (TileSource != null)
            {
                TileSource.TileCache.Clear();
            }
        }
    }
}
