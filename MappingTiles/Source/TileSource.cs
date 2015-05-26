using System;
using System.Collections.ObjectModel;

namespace MappingTiles
{
    public abstract class TileSource : Source
    {
        private readonly object counterLocker = new object();

        private int counter;

        protected TileSource()
        { }

        protected TileSource(string id)
            : this(new Wgs84TileSchema(), id)
        { }

        protected TileSource(TileSchema tileSchema, string id)
            : base(id)
        {
            this.Schema = tileSchema;
        }

        public TileSchema Schema
        {
            get;
            protected set;
        }

        public TileFormat Format
        {
            get
            {
                return Schema.TileFormat;
            }
            set
            {
                Schema.TileFormat = value;
            }
        }

        // Todo: consider remove it or add a callback fun for async-request.
        public byte[] GetTile(TileInfo tileInfo)
        {
            return GetTileCore(tileInfo);
        }

        protected abstract byte[] GetTileCore(TileInfo tileInfo);

        public Uri GetUri(TileInfo tileInfo)
        {
            return GetUriCore(tileInfo);
        }

        protected abstract Uri GetUriCore(TileInfo tileInfo);

        internal string GetNextServerDomain(Collection<string> serverDomains)
        {
            lock (counterLocker)
            {
                var selectedDomain = serverDomains[counter++];
                if (counter >= serverDomains.Count)
                {
                    counter = 0;
                }

                return selectedDomain;
            }
        }
    }
}
