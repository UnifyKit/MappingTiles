
namespace MappingTiles
{
    public abstract class TileDownloader
    {
        protected TileDownloader()
        {}

		public abstract void CancelTileDownload(TileInfo tileInfo);

		public abstract void DownloadTile(TileInfo tileInfo);

		public abstract void UpdateTileDownloadPriority(TileInfo tileInfo, int priority);
    }
}
