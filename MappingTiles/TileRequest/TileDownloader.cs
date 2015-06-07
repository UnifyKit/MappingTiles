
namespace MappingTiles
{
    public abstract class TileDownloader
    {
        protected TileDownloader()
        { }

        public abstract void CancelTileDownload(TileSource tileSource);

        public void DownloadTile(TileInfo tileInfo, TileSource tileSource, AsyncTileRequestCompletedHandler callback)
        {
            DownloadTile(tileInfo, tileSource, callback, NetworkPriority.Normal);
        }

        public abstract void DownloadTile(TileInfo tileInfo, TileSource tileSource, AsyncTileRequestCompletedHandler callback, NetworkPriority networkPriority);

        public abstract void UpdateTileDownloadPriority(TileInfo tileInfo, int priority);
    }
}
