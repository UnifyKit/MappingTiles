using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MappingTiles;

namespace MappingTiles.Winforms
{
    public class Map : Control, IDisposable
    {
        //private Bitmap mapBuffer;

        private View viewport;
        private string crs;
        private BoundingBox boundingbox;
        private ZoomLevel zoomLevel;
        private Coordinate center;
        private ObservableCollection<Layer> layers;

        public Map()
        {
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

        public BoundingBox Boundingbox
        {
            get
            {
                return boundingbox;
            }
            set
            {
                boundingbox = value;
            }
        }

        public ZoomLevel ZoomLevel
        {
            get
            {
                return zoomLevel;
            }
            set
            {
                zoomLevel = value;
            }
        }

        public Coordinate Center
        {
            get
            {
                return center;
            }
            set
            {
                center = value;
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

        protected void ViewChanged(UpdateMode updateMode)
        {
            foreach (var layer in layers.ToList())
            {
                layer.ViewChanged(updateMode, Viewport);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);

            base.OnPaint(e);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
        }

        protected void InitializeView()
        {
            if (double.IsNaN(Width) || Width == 0) return;
            //if (_map == null || _map.Envelope == null || double.IsNaN(_map.Envelope.Width) || _map.Envelope.Width <= 0) return;
            //if (_map.Envelope.GetCentroid() == null) return;

            //Map.Viewport.Center = _map.Envelope.GetCentroid();
            //Map.Viewport.Resolution = _map.Envelope.Width / Width;
            //_viewInitialized = true;
            //ViewChanged(true);
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
