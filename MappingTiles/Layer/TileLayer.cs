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

            if (Visible && renderContext.View.BoundingBox.Area > 0
                && innerSource.Schema.MaxZoomLevel.Resolution > renderContext.View.ZoomLevel.Resolution
                && innerSource.Schema.MinZoomLevel.Resolution < renderContext.View.ZoomLevel.Resolution)
            {
                tileMatrix = new TileMatrix(renderContext.View.ZoomLevel.Resolution, innerSource.Schema);
                Collection<TileInfo> tilesInBbox = tileMatrix.GetTiles(renderContext.View.BoundingBox);
                foreach (TileInfo tileInfo in tilesInBbox)
                {
                    this.innerSource.DownloadTile(tileInfo, new AsyncTileRequestCompletedHandler((tileInBytes, error) =>
                    {
                        renderContext.RenderObject = tileInBytes;
                        renderContext.RenderPosition = tileInfo.GetDrawingPosition((int)renderContext.View.Width, (int)renderContext.View.Height);

                        renderContext.Render.Draw(renderContext);
                    }));
                }
            }
        }
    }
}
