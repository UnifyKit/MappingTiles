using System;

namespace MappingTiles
{
    public class TileRequest
    {
		private Uri uri;
		private bool isAborted;
		private NetworkPriority networkPriority;
		private TileRequestCompletedHandler callback;

        private TileRequest()
        { }

        public TileRequest(Uri uri)
            : this(uri, null)
        { }

        public TileRequest(Uri uri, TileRequestCompletedHandler callback)
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
		}

		public TileRequestCompletedHandler Callback
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

		public void AbortIfInQueue()
		{
			this.isAborted = true;
		}
    }
}
