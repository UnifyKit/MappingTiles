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
        private ZoomLevel minZoomLevel;
        private ZoomLevel maxZoomLevel;
        private double opacity;

        protected Layer()
            : this(Utility.CreateUniqueId())
        { }

        protected Layer(string id)
        {
            this.minZoomLevel = new ZoomLevel(0);
            this.MaxZoomLevel = new ZoomLevel(); // take the max resolution
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

        /// <summary>
        /// Gets or sets the minimum zoom level (inclusive) at which this layer will be visible.
        /// </summary>
        public ZoomLevel MinZoomLevel
        {
            get
            {
                return minZoomLevel;
            }

            set
            {
                minZoomLevel = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum  zoom level (inclusive) at which this layer will be visible.
        /// </summary>
        public ZoomLevel MaxZoomLevel
        {
            get
            {
                return maxZoomLevel;
            }

            set
            {
                maxZoomLevel = value;
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

        /// <summary>
        /// Calculates the boundingbox for layer rendering. The layer will not be rendered outside of this extent.
        /// </summary>
        /// <returns>Return the extent of the layer or default value "new BoundingBox(0, 0, 0, 0)" if it will be visible regardless of extent.</returns>
        public virtual BoundingBox GetBoundingBox()
        {
            return new BoundingBox(0, 0, 0, 0);
        }

        public abstract void ClearCache();

        public abstract void ViewChanged(UpdateMode updateMode, View view, Func<DrawingParameters, bool> func);
    }
}
