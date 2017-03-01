using System;
using System.Collections;
using System.Collections.Generic;

namespace MappingTiles
{
    public class LayerCollection : IEnumerable<Layer>, IEnumerable
    {
        private double minScale = double.MaxValue;
        private double maxScale = double.MinValue;

        private List<Layer> layers;
        private MapCore owner;

        public LayerCollection(MapCore owner)
        {
            this.layers = new List<Layer>();
            this.owner = owner;
        }

        public int Count
        {
            get
            {
                return this.layers.Count;
            }
        }

        public double MaxScale
        {
            get
            {
                return this.maxScale;
            }
        }

        public double MinScale
        {
            get
            {
                return this.minScale;
            }
        }

        public Layer this[string id]
        {
            get
            {
                Layer layer;
                List<Layer>.Enumerator enumerator = this.layers.GetEnumerator();

                try
                {
                    while (enumerator.MoveNext())
                    {
                        Layer current = enumerator.Current;
                        if (current.Id != id)
                        {
                            continue;
                        }
                        layer = current;

                        return layer;
                    }

                    return null;
                }
                finally
                {
                    ((IDisposable)enumerator).Dispose();
                }
            }
        }

        public Layer this[int index]
        {
            get
            {
                return this.layers[index];
            }
        }

        public virtual void Add(Layer layer)
        {
            this.layers.Add(layer);
            this.UpdateZoomLevels();
        }

        public virtual void Remove(Layer layer)
        {
            this.layers.Remove(layer);
            this.UpdateZoomLevels();
        }

        public IEnumerator<Layer> GetEnumerator()
        {
            return this.layers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.layers).GetEnumerator();
        }

        protected virtual void UpdateZoomLevels()
        {
            if (this.layers == null || this.layers.Count == 0)
            {
                this.minScale = double.MaxValue;
                this.maxScale = double.MinValue;
                return;
            }

            foreach (Layer layer in this.layers)
            {
                if (this.minScale > layer.MinZoomLevel.Scale)
                {
                    this.minScale = layer.MinZoomLevel.Scale;
                }
                if (this.maxScale >= layer.MaxZoomLevel.Scale)
                {
                    continue;
                }
                this.maxScale = layer.MaxZoomLevel.Scale;
            }
        }
    }
}
