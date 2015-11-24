using System;

namespace MappingTiles
{
    internal class AsyncTileRequest
    {
        private Uri uri;
        private TileInfo tileInfo;
        private bool isAborted;
        private NetworkPriority networkPriority;
        private AsyncTileRequestCompletedHandler callback;

        private AsyncTileRequest()
        { }

        public AsyncTileRequest(Uri uri)
            : this(uri, null, null)
        { }

        public AsyncTileRequest(Uri uri, TileInfo tileInfo, AsyncTileRequestCompletedHandler callback)
        {
            this.uri = uri;
            this.callback = callback;
        }

        public Uri Uri
        {
            get
            {
                return this.uri;
            }
        }

        public bool IsAborted
        {
            get
            {
                return isAborted;
            }
            internal set
            {
                IsAborted = value;
            }
        }

        public AsyncTileRequestCompletedHandler Callback
        {
            get
            {
                return this.callback;
            }
        }

        public NetworkPriority NetworkPriority
        {
            get
            {
                return this.networkPriority;
            }
            set
            {
                this.networkPriority = value;
            }
        }

        internal NetworkPriority NetworkPrioritySnapshot
        {
            get;
            set;
        }

        public TileInfo TileInfo
        {
            get
            {
                return tileInfo;
            }

            set
            {
                tileInfo = value;
            }
        }

        public void AbortIfInQueue()
        {
            this.isAborted = true;
        }
    }
}
