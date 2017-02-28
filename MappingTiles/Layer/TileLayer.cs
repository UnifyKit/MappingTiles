using System.Collections.ObjectModel;

namespace MappingTiles
{
    public class TileLayer : Layer
    {
        private TileSource innerSource;
        private TileMatrix tileMatrix;

        public TileLayer(TileSource tileSource)
            : this(tileSource, Utility.CreateUniqueId())
        { }

        public TileLayer(TileSource tileSource, string id)
            : base(tileSource, id)
        {
            innerSource = tileSource;
        }

        public override void ClearCache()
        {
            if (innerSource != null)
            {
                innerSource.TileCache.Clear();
            }
        }

        public override void Draw(RenderContext renderContext, UpdateMode updateMode)
        {
            if (innerSource == null || innerSource.Schema == null)
            {
                return;
            }

            if (Visible && renderContext.Viewport.BoundingBox.Area > 0
                && innerSource.Schema.MaxZoomLevel.Resolution > renderContext.Viewport.ZoomLevel.Resolution
                && innerSource.Schema.MinZoomLevel.Resolution < renderContext.Viewport.ZoomLevel.Resolution)
            {
                tileMatrix = new TileMatrix(renderContext.Viewport.ZoomLevel.Resolution, innerSource.Schema);
                Collection<TileInfo> tilesInBbox = tileMatrix.GetTiles(renderContext.Viewport.BoundingBox);
                foreach (TileInfo tileInfo in tilesInBbox)
                {
                    this.innerSource.DownloadTile(tileInfo, new AsyncTileRequestCompletedHandler((tileInBytes, error) =>
                    {
                        renderContext.DrawnObject = tileInBytes;
                        renderContext.DrawnPosition = tileInfo.GetDrawingPosition((int)renderContext.Viewport.Width, (int)renderContext.Viewport.Height);

                        renderContext.Renderer.Draw(renderContext);
                    }));
                }
            }
        }
    }
}
