using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MappingTiles
{
    public class TileLayer : Layer
    {
        private TileSource innerTileSource;
        private TileMatrix tileMatrix;

        protected TileLayer(TileSource tileSource, string id)
            : base(id)
        {
            SetTileSource(tileSource);
        }

        public TileSource TileSource
        {
            get
            {
                return innerTileSource;
            }
        }

        protected void SetTileSource(TileSource tileSource)
        {
            if (tileSource != null)
            {
                this.innerTileSource = tileSource;
            }
        }

        public override void ClearCache()
        {
            if (TileSource != null)
            {
                TileSource.TileCache.Clear();
            }
        }

        public override void ViewChanged(UpdateMode updateMode, DrawingParameters parameters)
        {
            if (TileSource == null)
            {
                return;
            }

            if (Visible && parameters.View.BoundingBox.Area > 0 && innerTileSource != null && MaxZoomLevel.Resolution > parameters.View.ZoomLevel.Resolution && MinZoomLevel.Resolution < parameters.View.ZoomLevel.Resolution)
            {
                tileMatrix = new TileMatrix(parameters.View.ZoomLevel.Resolution, innerTileSource.Schema);
                Collection<TileInfo> tilesInBbox = tileMatrix.GetTiles(parameters.View.BoundingBox);
                foreach (TileInfo tileInfo in tilesInBbox)
                {
                    this.TileSource.DownloadTile(tileInfo, new AsyncTileRequestCompletedHandler(DrawTile));
                }
            }
        }

        protected virtual TileDownloader CreateTileDownloader(TileSchema tileSchema)
        {
            TileDownloader tileDownloader = new ImageTileDownloader(tileSchema);

            return tileDownloader;
        }

        private void DrawTile(byte[] tilesInBytes, Exception error)
        { 
            
        }
    }
}
