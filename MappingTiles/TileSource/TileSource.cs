using System;
using System.Collections.ObjectModel;

namespace MappingTiles
{
    public abstract class TileSource : Source
    {
        private readonly Random random = new Random();

        private TileDownloader tileDownloader;
        private TileFormat tileFormat;
        private TileSchema tileSchema;

        protected TileSource()
            : this(new Wgs84TileSchema())
        { }

        protected TileSource(TileSchema tileSchema)
            : this(tileSchema, Utility.CreateUniqueId())
        { }

        protected TileSource(TileSchema tileSchema, string id)
            : base(id)
        {
            this.tileSchema = tileSchema;
            this.tileDownloader = this.tileDownloader ?? new ImageTileDownloader();
            this.tileDownloader.TileCache = new MemoryTileCache<byte[]>();
            this.tileFormat = TileFormat.Png;
        }

        public TileSchema Schema
        {
            get
            {
                return tileSchema;
            }
            set
            {
                tileSchema = value;
            }
        }

        public TileFormat Format
        {
            get
            {
                return tileFormat;
            }
            set
            {
                tileFormat = value;
            }
        }

        public ITileCache<byte[]> TileCache
        {
            get
            {
                return this.tileDownloader.TileCache;
            }
            set
            {
                this.tileDownloader.TileCache = value;
            }
        }

        public TileDownloader TileDownloader
        {
            get
            {
                return tileDownloader;
            }
            set
            {
                tileDownloader = value;
            }
        }

        public virtual void DownloadTile(TileInfo tileInfo, AsyncTileRequestCompletedHandler callback)
        {
            Uri tileUri = GetTileUri(tileInfo);

            // Attach the events after tile request downloaded.
            tileDownloader.TileDownloadCompleted += (sender, e) =>
            {
                callback(e.TileInfo.Content, null);
            };
            // Start download the image and attach the content.
            tileDownloader.StartDownload(tileUri, tileInfo);
        }

        public Uri GetTileUri(TileInfo tileInfo)
        {
            return GetUriCore(tileInfo);
        }

        protected abstract Uri GetUriCore(TileInfo tileInfo);

        protected string GetNextServerDomain(Collection<string> serverDomains)
        {
            return serverDomains[this.random.Next(0, serverDomains.Count)];
        }
    }
}
