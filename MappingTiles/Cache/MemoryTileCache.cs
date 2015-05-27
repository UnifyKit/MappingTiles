using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MappingTiles
{
    public class MemoryTileCache<T> : ITileCache<T>, INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly Dictionary<string, T> tileDatas = new Dictionary<string, T>();
        private readonly Dictionary<string, DateTime> queriedDatas = new Dictionary<string, DateTime>();
        private readonly object syncLocker = new object();
        private readonly Func<string, bool> keepTileInMemory;
        private bool isDisposed;

        public MemoryTileCache(int minTiles = 50, int maxTiles = 100, Func<string, bool> keepTileInMemory = null)
        {
            if (minTiles >= maxTiles) throw new ArgumentException("minTiles should be smaller than maxTiles");
            if (minTiles < 0) throw new ArgumentException("minTiles should be larger than zero");
            if (maxTiles < 0) throw new ArgumentException("maxTiles should be larger than zero");

            MinTiles = minTiles;
            MaxTiles = maxTiles;
            this.keepTileInMemory = keepTileInMemory;
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

        public void Add(string id, T item)
        {
            lock (syncLocker)
            {
                if (tileDatas.ContainsKey(id))
                {
                    tileDatas[id] = item;
                    queriedDatas[id] = DateTime.Now;
                }
                else
                {
                    queriedDatas.Add(id, DateTime.Now);
                    tileDatas.Add(id, item);
                    CleanUp();
                    OnNotifyPropertyChange("TileCount");
                }
            }
        }

        public void Remove(string id)
        {
            lock (syncLocker)
            {
                if (!tileDatas.ContainsKey(id))
                {
                    return;
                }

                var disposable = (tileDatas[id] as IDisposable);
                if (disposable != null)
                {
                    disposable.Dispose();
                }

                queriedDatas.Remove(id);
                tileDatas.Remove(id);
                OnNotifyPropertyChange("TileCount");
            }
        }

        public T Get(string id)
        {
            lock (syncLocker)
            {
                if (!tileDatas.ContainsKey(id))
                {
                    return default(T);
                }
                queriedDatas[id] = DateTime.Now;

                return tileDatas[id];
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

        virtual protected void CleanUp()
        {
            if (tileDatas.Count <= MaxTiles)
            {
                return;
            }

            var numberOfTilesToKeepInMemory = 0;
            if (keepTileInMemory != null)
            {
                var tilesToKeep = queriedDatas.Keys.Where(keepTileInMemory).ToList();
                foreach (var index in tilesToKeep) queriedDatas[index] = DateTime.Now; // touch tiles to keep
                numberOfTilesToKeepInMemory = tilesToKeep.Count;
            }

            var numberOfTilesToRemove = tileDatas.Count - Math.Max(MinTiles, numberOfTilesToKeepInMemory);
            var oldItems = queriedDatas.OrderBy(p => p.Value).Take(numberOfTilesToRemove);
            foreach (var oldItem in oldItems)
            {
                Remove(oldItem.Key);
            }
        }

        protected virtual void OnNotifyPropertyChange(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
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

        private void DisposeTilesIfDisposable()
        {
            foreach (var index in tileDatas.Keys)
            {
                var bitmap = (tileDatas[index] as IDisposable);
                if (bitmap != null) bitmap.Dispose();
            }
        }
    }
}
