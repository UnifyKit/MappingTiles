using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MappingTiles
{
    public class TileLayer : Layer
    {
        private TileSource tileSource;
        private TileMatrix tileMatrix;

        protected TileLayer(TileSource tileSource, string id)
            : base(id)
        {
            this.tileSource = tileSource;
        }

        public TileSource TileSource
        {
            get
            {
                return tileSource;
            }
        }

        protected void SetTileSource(TileSource tileSource)
        {
            if (tileSource != null)
            { 
                
            }
        }

        public override void ClearCache()
        {
            if (TileSource != null)
            {
                TileSource.TileCache.Clear();
            }
        }

        public override void ViewChanged(UpdateMode updateMode, View view, Func<DrawingParameters, bool> drawArgs)
        {
            if (Visible && view.BoundingBox.Area > 0 && tileSource != null && MaxZoomLevel.Resolution > view.ZoomLevel.Resolution && MinZoomLevel.Resolution < view.ZoomLevel.Resolution)
            {
                tileMatrix = new TileMatrix(view.ZoomLevel.Resolution, tileSource.Schema);
                Collection<TileInfo> tilesInBbox = tileMatrix.GetTiles(view.BoundingBox);
                foreach (TileInfo tile in tilesInBbox)
                {
                    //tileSource.DownloadTile(tile, drawArgs); // Todo: we need to change the null value.
                }
            }
        }
    }
}
