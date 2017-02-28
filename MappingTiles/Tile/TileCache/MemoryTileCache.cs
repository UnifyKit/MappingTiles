using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace MappingTiles
{
    public class MemoryTileCache<T> : ITileCache<T>, INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly object syncLocker = new object();

        private readonly Dictionary<string, T> tileDatas;
        private readonly Dictionary<TileInfo, DateTime> queriedDate;
        private readonly Func<TileInfo, bool> keepTileInMemory;
        private bool isDisposed;

        public MemoryTileCache()
            : this(50, 2000, null)
        {
        }

        public MemoryTileCache(int maxTiles)
            : this(Math.Min(50, maxTiles), Math.Max(50, maxTiles), null)
        { }

        public MemoryTileCache(int minTiles, int maxTiles, Func<TileInfo, bool> keepTileInMemory)
        {
            if (minTiles >= maxTiles)
            {
                throw new ArgumentException("minTiles should be smaller than maxTiles");
            }
            if (minTiles < 0)
            {
                throw new ArgumentException("minTiles should be larger than zero");
            }
            if (maxTiles < 0)
            {
                throw new ArgumentException("maxTiles should be larger than zero");
            }

            this.MinTiles = minTiles;
            this.MaxTiles = maxTiles;

            this.keepTileInMemory = keepTileInMemory;
            this.tileDatas = new Dictionary<string, T>();
            this.queriedDate = new Dictionary<TileInfo, DateTime>();
        }

        public int TileCount
        {
            get
            {
                return tileDatas.Count;
            }
        }

        public int MinTiles
        {
            get;
            set;
        }

        public int MaxTiles
        {
            get;
            set;
        }

        public void Save(TileInfo tileInfo, T tile)
        {
            lock (syncLocker)
            {
                if (tileDatas.ContainsKey(tileInfo.Id))
                {
                    queriedDate[tileInfo] = DateTime.Now;
                }
                else
                {
                    queriedDate.Add(tileInfo, DateTime.Now);
                    tileDatas.Add(tileInfo.Id, tile);
                    CleanUp();
                    OnNotifyPropertyChange("TileCount");
                }
            }
        }

        public void Remove(int tileX, int tileY, ZoomLevel zoomLevel)
        {
            Remove(tileX, tileY, zoomLevel.Id);
        }

        public void Remove(int tileX, int tileY, string zoomLevelId)
        {
            string id = string.Format(CultureInfo.InvariantCulture, "{0}_{1}_{2}", tileX, tileY, zoomLevelId);
            var queriedTile = queriedDate.Keys.Where(key => key.Id.Equals(id, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (queriedTile != null)
            {
                Remove(queriedTile);
            }
        }

        public void Remove(TileInfo tileInfo)
        {
            lock (syncLocker)
            {
                if (!tileDatas.ContainsKey(tileInfo.Id))
                {
                    return;
                }

                var disposable = (tileDatas[tileInfo.Id] as IDisposable);
                if (disposable != null)
                {
                    disposable.Dispose();
                }

                queriedDate.Remove(tileInfo);
                tileDatas.Remove(tileInfo.Id);
                OnNotifyPropertyChange("TileCount");
            }
        }

        public T Read(int tileX, int tileY, ZoomLevel zoomLevel)
        {
            return Read(tileX, tileY, zoomLevel.Id);
        }

        public T Read(int tileX, int tileY, string zoomLevelId)
        {
            string id = string.Format(CultureInfo.InvariantCulture, "{0}_{1}_{2}", tileX, tileY, zoomLevelId);
            var queriedTile = queriedDate.Keys.Where(key => key.Id.Equals(id, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (queriedTile != null)
            {
                return Read(queriedTile);
            }

            return default(T);
        }

        public T Read(TileInfo tileInfo)
        {
            lock (syncLocker)
            {
                if (!tileDatas.ContainsKey(tileInfo.Id))
                {
                    return default(T);
                }
                queriedDate[tileInfo] = DateTime.Now;

                return tileDatas[tileInfo.Id];
            }
        }

        public void Clear()
        {
            lock (syncLocker)
            {
                DisposeTilesIfDisposable();
                queriedDate.Clear();
                tileDatas.Clear();
                OnNotifyPropertyChange("TileCount");
            }
        }

        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            DisposeTilesIfDisposable();
            queriedDate.Clear();
            tileDatas.Clear();
            isDisposed = true;
        }

        protected virtual void OnNotifyPropertyChange(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void DisposeTilesIfDisposable()
        {
            foreach (var index in tileDatas.Keys)
            {
                var bitmap = (tileDatas[index] as IDisposable);
                if (bitmap != null)
                {
                    bitmap.Dispose();
                }
            }
        }

        private void CleanUp()
        {
            if (tileDatas.Count <= MaxTiles)
            {
                return;
            }

            var numberOfTilesToKeepInMemory = 0;
            if (keepTileInMemory != null)
            {
                var tilesToKeep = queriedDate.Keys.Where(keepTileInMemory).ToList();
                foreach (var index in tilesToKeep)
                {
                    queriedDate[index] = DateTime.Now; // touch tiles to keep
                }
                numberOfTilesToKeepInMemory = tilesToKeep.Count;
            }

            var numberOfTilesToRemove = tileDatas.Count - Math.Max(MinTiles, numberOfTilesToKeepInMemory);
            var oldItems = queriedDate.OrderBy(p => p.Value).Take(numberOfTilesToRemove);
            foreach (var oldItem in oldItems)
            {
                Remove(oldItem.Key);
            }
        }
    }
}
