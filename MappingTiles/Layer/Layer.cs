using System;
using System.ComponentModel;

namespace MappingTiles
{
    public abstract class Layer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string id;
        private string name;
        private bool visible;
        private double opacity;

        private ZoomLevel minZoomLevel;
        private ZoomLevel maxZoomLevel;
        private Source datasource;

        protected Layer()
            : this(null, Utility.CreateUniqueId())
        { }
        protected Layer(string id)
            : this(null, id)
        { }

        protected Layer(Source datasource)
            : this(datasource, Utility.CreateUniqueId())
        {
        }

        protected Layer(Source datasource, string id)
        {
            this.datasource = datasource;
            this.id = id;
            this.Opacity = 1;
        }

        public string Id
        {
            get
            {
                return id;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
            }
        }

        public double Opacity
        {
            get
            {
                return opacity;
            }
            set
            {
                opacity = value;
            }
        }

        public Source Datasource
        {
            get
            {
                return datasource;
            }
            set
            {
                datasource = value;
            }
        }

        public ZoomLevel MaxZoomLevel
        {
            get { return maxZoomLevel; }
            set { maxZoomLevel = value; }
        }

        public ZoomLevel MinZoomLevel
        {
            get { return minZoomLevel; }
            set { minZoomLevel = value; }
        }

        /// <summary>
        /// Calculates the boundingbox for layer rendering. The layer will not be rendered outside of this extent.
        /// </summary>
        /// <returns>Return the extent of the layer or default value "new BoundingBox(0, 0, 0, 0)" if it will be visible regardless of extent.</returns>
        public virtual BoundingBox GetBoundingBox()
        {
            return new BoundingBox(0, 0, 0, 0);
        }

        public abstract void ClearCache();

        public abstract void Draw(RenderContext renderContext, UpdateMode updateMode);
    }
}
