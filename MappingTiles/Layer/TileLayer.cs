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

        public override void ViewChanged(UpdateMode updateMode, View view)
        {
            if (Visible && view.BoundingBox.Area > 0 && tileSource != null && MaxZoomLevel.Resolution > view.ZoomLevel.Resolution && MinZoomLevel.Resolution < view.ZoomLevel.Resolution)
            {
                //_tileFetcher.ViewChanged(extent, resolution);
            }
        }
    }
}
