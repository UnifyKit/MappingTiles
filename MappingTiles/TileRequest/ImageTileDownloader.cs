using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Threading;

namespace MappingTiles
{
    public class ImageTileDownloader : TileDownloader
    {
        private Dictionary<TileSource, AsyncTileRequest> tileRequests;
        private TileSchema tileSchema;
        private readonly ITileCache<byte[]> tileCache;

        public ImageTileDownloader()
            : this(new Wgs84TileSchema())
        {
        }

        public ImageTileDownloader(TileSchema tileSchema)
        {
            this.tileSchema = tileSchema;
            this.tileRequests = new Dictionary<TileSource, AsyncTileRequest>();
        }

        public IWebProxy WebProxy
        {
            get
            {
                return AsyncTileRequestQueue.WebProxy;
            }
            set
            {
                AsyncTileRequestQueue.WebProxy = value;
            }
        }

        public ICredentials Credentials
        {
            get
            {
                return AsyncTileRequestQueue.Credentials;
            }
            set
            {
                AsyncTileRequestQueue.Credentials = value;
            }
        }

        public ITileCache<byte[]> TileCache
        {
            get
            {
                return AsyncTileRequestQueue.TileCache;
            }
        }

        public TileSchema TileSchema
        {
            get { return tileSchema; }
        }

        public override void Download(TileInfo tileInfo, TileSource tileSource, AsyncTileRequestCompletedHandler callback, NetworkPriority networkPriority)
        {
            AsyncTileRequest tileRequest;
            if (this.tileRequests.TryGetValue(tileSource, out tileRequest))
            {
                throw new InvalidOperationException("Multiple concurrent downloads of the same tile is not supported.");
            }
            AsyncTileRequestQueue.Instance.CreateRequest(tileSource.GetUri(tileInfo), tileInfo, networkPriority, callback);
        }

        public override void Cancel(TileSource tileSource)
        {
            AsyncTileRequest tileRequest;
            if (!this.tileRequests.TryGetValue(tileSource, out tileRequest))
            {
                throw new InvalidOperationException(ApplicationMessages.TileInProgressCancel);
            }
            tileRequest.IsAborted = true;
            tileRequest.AbortIfInQueue();
            tileRequests.Remove(tileSource);
        }

        public override void UpdateDownloadPriority(TileSource tileSource, int priority)
        {
            AsyncTileRequest tileRequest;
            if (this.tileRequests.TryGetValue(tileSource, out tileRequest))
            {
                tileRequest.NetworkPriority = (NetworkPriority)priority;
            }
        }
    }
}
