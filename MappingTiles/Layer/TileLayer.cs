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

        public override void ViewChanged(UpdateMode updateMode, RenderContext renderContext)
        {
            if (TileSource == null)
            {
                return;
            }

            if (Visible && renderContext.View.BoundingBox.Area > 0 && innerTileSource != null && MaxZoomLevel.Resolution > renderContext.View.ZoomLevel.Resolution && MinZoomLevel.Resolution < renderContext.View.ZoomLevel.Resolution)
            {
                tileMatrix = new TileMatrix(renderContext.View.ZoomLevel.Resolution, innerTileSource.Schema);
                Collection<TileInfo> tilesInBbox = tileMatrix.GetTiles(renderContext.View.BoundingBox);
                foreach (TileInfo tileInfo in tilesInBbox)
                {
                    this.TileSource.DownloadTile(tileInfo, new AsyncTileRequestCompletedHandler((tileInBytes, error) =>
                    {
                        //parameters.Render.Draw()
                    }));
                }
            }
        }

        protected virtual TileDownloader CreateTileDownloader(TileSchema tileSchema)
        {
            TileDownloader tileDownloader = new ImageTileDownloader(tileSchema);

            return tileDownloader;
        }
    }
}
