
using System;
using System.Net;

namespace MappingTiles
{
    public abstract class TileDownloader
    {
        public event TileInfoEventHandler TileDownloadCompleted;

        private IWebProxy webProxy;
        private ICredentials credentials;
        private ITileCache<byte[]> tileCache;

        protected TileDownloader()
        {
            this.WebProxy = null;
            this.credentials = null;
        }

        public IWebProxy WebProxy
        {
            get
            {
                return webProxy;
            }
            set
            {
                webProxy = value;
            }
        }

        public ICredentials Credentials
        {
            get
            {
                return credentials;
            }
            set
            {
                credentials = value;
            }
        }

        internal ITileCache<byte[]> TileCache
        {
            get
            {
                return tileCache;
            }
            set
            {
                tileCache = value;
            }
        }

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
