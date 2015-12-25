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

        private MapCore mapCore;
        private bool viewInitialized;

        public Map()
        {
            this.viewInitialized = false;
            this.mapCore = GetMapCore();
        }

        public ObservableCollection<Layer> Layers
        {
            get
            {
                return mapCore.Layers;
            }
        }

        public View Viewport
        {
            get
            {
                return mapCore.Viewport;
            }
            set
            {
                mapCore.Viewport = value;
            }
        }

        public string Crs
        {
            get
            {
                return mapCore.Crs;
            }
            set
            {
                mapCore.Crs = value;
            }
        }

        public void ClearCache()
        {
            this.mapCore.ClearCache();
        }

        protected void ViewChanged(UpdateMode updateMode)
        {
            this.mapCore.ViewChanged(updateMode);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!viewInitialized)
            {
                InitializeView();
            }

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
            if (Viewport == null || Viewport.BoundingBox == null) return;
            if (Viewport.Center == null) return;

            viewInitialized = true;
            ViewChanged(UpdateMode.All);
        }

        protected virtual MapCore GetMapCore()
        {
            MapCore map = new MapCore();

            return map;
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
