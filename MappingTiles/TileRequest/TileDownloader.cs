
namespace MappingTiles
{
    public abstract class TileDownloader
    {
        protected TileDownloader()
        { }

        public abstract void Cancel(TileSource tileSource);

        public void Download(TileInfo tileInfo, TileSource tileSource, AsyncTileRequestCompletedHandler callback)
        {
            Download(tileInfo, tileSource, callback, NetworkPriority.Normal);
        }

        public abstract void Download(TileInfo tileInfo, TileSource tileSource, AsyncTileRequestCompletedHandler callback, NetworkPriority networkPriority);

        public abstract void UpdateDownloadPriority(TileSource tileSource, int priority);
    }
}
