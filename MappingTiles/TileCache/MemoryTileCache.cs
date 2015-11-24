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

        private readonly Dictionary<TileInfo, T> tileDatas;
        private readonly Dictionary<TileInfo, DateTime> queriedDatas;
        private readonly Func<TileInfo, bool> keepTileInMemory;
        private bool isDisposed;

        public MemoryTileCache()
            : this(50, 100, null)
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
            this.tileDatas = new Dictionary<TileInfo, T>();
            this.queriedDatas = new Dictionary<TileInfo, DateTime>();
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

        public void Add(TileInfo tileInfo, T data)
        {
            lock (syncLocker)
            {
                if (tileDatas.ContainsKey(tileInfo))
                {
                    tileDatas[tileInfo] = data;
                    queriedDatas[tileInfo] = DateTime.Now;
                }
                else
                {
                    queriedDatas.Add(tileInfo, DateTime.Now);
                    tileDatas.Add(tileInfo, data);
                    CleanUp();
                    OnNotifyPropertyChange("TileCount");
                }
            }
        }

        public void Remove(int column, int row, ZoomLevel zoomLevel)
        {
            Remove(column, row, zoomLevel.Id);
        }

        public void Remove(int column, int row, string zoomLevelId)
        {
            string id = string.Format(CultureInfo.InvariantCulture, "{0}_{1}_{2}", column, row, zoomLevelId);
            var queriedTile = queriedDatas.Keys.Where(key => key.Id.Equals(id, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (queriedTile != null)
            {
                Remove(queriedTile);
            }
        }

        public void Remove(TileInfo tileInfo)
        {
            lock (syncLocker)
            {
                if (!tileDatas.ContainsKey(tileInfo))
                {
                    return;
                }

                var disposable = (tileDatas[tileInfo] as IDisposable);
                if (disposable != null)
                {
                    disposable.Dispose();
                }

                queriedDatas.Remove(tileInfo);
                tileDatas.Remove(tileInfo);
                OnNotifyPropertyChange("TileCount");
            }
        }

        public T Get(int column, int row, ZoomLevel zoomLevel)
        {
            return Get(column, row, zoomLevel.Id);
        }

        public T Get(int column, int row, string zoomLevelId)
        {
            string id = string.Format(CultureInfo.InvariantCulture, "{0}_{1}_{2}", column, row, zoomLevelId);
            var queriedTile = queriedDatas.Keys.Where(key => key.Id.Equals(id, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (queriedTile != null)
            {
                return Get(queriedTile);
            }

            return default(T);
        }

        public T Get(TileInfo tileInfo)
        {
            lock (syncLocker)
            {
                if (!tileDatas.ContainsKey(tileInfo))
                {
                    return default(T);
                }
                queriedDatas[tileInfo] = DateTime.Now;

                return tileDatas[tileInfo];
            }
        }

        public void Clear()
        {
            lock (syncLocker)
            {
                DisposeTilesIfDisposable();
                queriedDatas.Clear();
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
            queriedDatas.Clear();
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
                var tilesToKeep = queriedDatas.Keys.Where(keepTileInMemory).ToList();
                foreach (var index in tilesToKeep)
                {
                    queriedDatas[index] = DateTime.Now; // touch tiles to keep
                }
                numberOfTilesToKeepInMemory = tilesToKeep.Count;
            }

            var numberOfTilesToRemove = tileDatas.Count - Math.Max(MinTiles, numberOfTilesToKeepInMemory);
            var oldItems = queriedDatas.OrderBy(p => p.Value).Take(numberOfTilesToRemove);
            foreach (var oldItem in oldItems)
            {
                Remove(oldItem.Key);
            }
        }
    }
}
