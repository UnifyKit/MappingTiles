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
        private View viewport;
        private Render render;

        private string crs;
        private BoundingBox boundingbox;
        private ZoomLevel zoomLevel;
        private Coordinate center;
        private bool viewInitialized;

        private ObservableCollection<Layer> layers;

        public MapCore()
        {
            viewInitialized = false;

            layers = new ObservableCollection<Layer>();
            layers.CollectionChanged += Layers_CollectionChanged;
        }

        public ObservableCollection<Layer> Layers
        {
            get
            {
                return layers;
            }
        }

        public View Viewport
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

        public Render Render
        {
            get
            {
                return render;
            }
            set
            {
                render = value;
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
            throw new NotImplementedException();
        }

        public void ClearCache()
        {
            foreach (Layer layer in layers)
            {
                layer.ClearCache();
            }
        }

        public void ViewChanged(UpdateMode updateMode)
        {
            foreach (var layer in layers.ToList())
            {
                layer.ViewChanged(updateMode, Viewport, (parameter) =>
                {
                    throw new NotImplementedException();
                });
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

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MapCore() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

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
