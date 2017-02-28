using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MappingTiles
{
    public class MapCore : IDisposable
    {
        private Viewport viewport;
        private Renderer renderer;

        private string crs;
        private bool viewInitialized;

        private ObservableCollection<Layer> layers;

        public MapCore()
        {
            viewInitialized = false;

            layers = new ObservableCollection<Layer>();
            layers.CollectionChanged += Layers_CollectionChanged;
            viewport = new Viewport(0, 0, double.NaN);
        }

        public ObservableCollection<Layer> Layers
        {
            get
            {
                return layers;
            }
        }

        public Viewport Viewport
        {
            get
            {
                return viewport;
            }
            set
            {
                viewport = value;
            }
        }

        public Renderer Renderer
        {
            get
            {
                return renderer;
            }
            set
            {
                renderer = value;
            }
        }

        public string Crs
        {
            get
            {
                return crs;
            }
            set
            {
                crs = value;
            }
        }

        private void Layers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void ClearCache()
        {
            foreach (Layer layer in layers)
            {
                layer.ClearCache();
            }
        }

        public void ViewChanged(RenderContext renderContext, UpdateMode updateMode)
        {
            foreach (var layer in layers.ToList())
            {
                layer.Draw(renderContext, updateMode);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
