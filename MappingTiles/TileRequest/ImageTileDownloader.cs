using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace MappingTiles
{
    public class ImageTileDownloader : TileDownloader
    {
        private Dictionary<TileInfo, AsyncTileRequest> tileRequests;
        private Dispatcher uiDispatcher;
        private TileSchema tileSchema;

        public ImageTileDownloader(Dispatcher uiDispatcher)
            : this(new Wgs84TileSchema(), uiDispatcher)
        {
        }

        public ImageTileDownloader(TileSchema tileSchema, Dispatcher uiDispatcher)
        {
            this.tileSchema = tileSchema;
            this.uiDispatcher = uiDispatcher;

            tileRequests = new Dictionary<TileInfo, AsyncTileRequest>();
        }

        public TileSchema TileSchema
        {
            get { return tileSchema; }
        }

        public override void CancelTileDownload(TileInfo tileInfo)
        {
            AsyncTileRequest tileRequest;
            if (!this.tileRequests.TryGetValue(tileInfo, out tileRequest))
            {
                throw new InvalidOperationException(ApplicationMessages.TileInProgressCancel);
            }
            tileRequest.IsAborted = true;
            tileRequest.AbortIfInQueue();
            tileRequests.Remove(tileInfo);
        }

        public override void DownloadTile(TileInfo tileInfo)
        {
            AsyncTileRequest tileRequest = null;
            if (this.tileRequests.TryGetValue(tileInfo, out tileRequest))
            {
                throw new InvalidOperationException("Multiple concurrent downloads of the same tile is not supported.");
            }
            else
            { 
            }

            //AsyncTileRequestQueue.Instance.CreateRequest()
        }   

        public override void UpdateTileDownloadPriority(TileInfo tileInfo, int priority)
        {
            throw new NotImplementedException();
        }
    }
}
