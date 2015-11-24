using System;
using System.ComponentModel;

namespace MappingTiles
{
    public abstract class Layer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string id;
        private string name;
        private bool enabled;
        private ZoomLevel minVisibleZoomLevel;
        private ZoomLevel maxVisibleZoomLevel;

        protected Layer()
            : this(Utility.CreateUniqueId())
        { }

        protected Layer(string id)
        {
            this.minVisibleZoomLevel = new ZoomLevel(0);
            this.MaxVisibleZoomLevel = new ZoomLevel(); // take the max resolution
            this.id = id;
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

        public bool Enabled
        {
            get
            {
                return enabled;
            }

            set
            {
                enabled = value;
            }
        }

        public ZoomLevel MinVisibleZoomLevel
        {
            get
            {
                return minVisibleZoomLevel;
            }

            set
            {
                minVisibleZoomLevel = value;
            }
        }

        public ZoomLevel MaxVisibleZoomLevel
        {
            get
            {
                return maxVisibleZoomLevel;
            }

            set
            {
                maxVisibleZoomLevel = value;
            }
        }

        public virtual BoundingBox GetBoundingBox()
        {
            throw new NotImplementedException();
        }
    }
}
