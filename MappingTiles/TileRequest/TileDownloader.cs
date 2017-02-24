
using System;

namespace MappingTiles
{
    public abstract class TileDownloader
    {
        public event TileInfoEventHandler TileDownloadCompleted;

        protected TileDownloader()
        { }

        public abstract void StartDownload(Uri uri, TileInfo tileInfo);

        public abstract void CancelDownload(TileInfo tileInfo);

        protected virtual void OnTileDownloadComplete(TileInfoEventArgs e)
        {
            if (TileDownloadCompleted != null)
            {
                TileDownloadCompleted(this, e);
            }
        }
    }
}
