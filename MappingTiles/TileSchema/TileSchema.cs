using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace MappingTiles
{
    public abstract class TileSchema
    {
        private ZoomLevel minZoomLevel;
        private ZoomLevel maxZoomLevel;
        private Collection<ZoomLevel> zoomLevels;

        private int numberOfZoomLevels = 20;

        protected TileSchema()
        {
            this.IsYAxisReversed = false;
            this.zoomLevels = new Collection<ZoomLevel>();
            this.minZoomLevel = new ZoomLevel(0);
            this.maxZoomLevel = new ZoomLevel(); // take the max resolution
        }

        public BoundingBox MaxExtent
        {
            get;
            set;
        }

        public ZoomLevel MinZoomLevel
        {
            get
            {
                return minZoomLevel;
            }
            set
            {
                if (minZoomLevel != value)
                {
                    minZoomLevel = value;
                    GetZoomLevelsCore();
                }
            }
        }

        public ZoomLevel MaxZoomLevel
        {
            get
            {
                return maxZoomLevel;
            }
            set
            {
                if (maxZoomLevel != value)
                {
                    maxZoomLevel = value;
                    GetZoomLevelsCore();
                }
            }
        }

        public int NumberOfZoomLevels
        {
            get { return numberOfZoomLevels; }
            set
            {
                if (numberOfZoomLevels != value)
                {
                    numberOfZoomLevels = value;
                    GetZoomLevelsCore();
                }
            }
        }

        public bool IsYAxisReversed
        {
            get;
            set;
        }

        public string Crs
        {
            get;
            set;
        }

        public Collection<ZoomLevel> GetZoomLevels()
        {
            if (zoomLevels.Count <= 0)
            {
                return GetZoomLevelsCore();
            }

            return zoomLevels;
        }

        protected virtual Collection<ZoomLevel> GetZoomLevelsCore()
        {
            zoomLevels.Clear();

            double resolution = MaxZoomLevel.Resolution;
            for (int i = 0; i < numberOfZoomLevels; i++)
            {
                var zoomLevel = new ZoomLevel(resolution, (i + 1).ToString(CultureInfo.InvariantCulture));
                zoomLevels.Add(zoomLevel);

                resolution /= 2;
            }

            return zoomLevels;
        }

        public ZoomLevel GetNearestZoomLevel(double resolution)
        {
            InternalChecker.CheckArrayIsEmptyOrNull(zoomLevels, "ZoomLevels");

            var orderedZoomLevels = zoomLevels.OrderByDescending(z => z.Resolution);

            // smaller than smallest
            if (orderedZoomLevels.Last().Resolution > resolution)
            {
                return orderedZoomLevels.Last();
            }

            // bigger than biggest
            if (orderedZoomLevels.First().Resolution < resolution)
            {
                return orderedZoomLevels.First();
            }

            ZoomLevel result = null;
            double resultDistance = double.MaxValue;
            foreach (var current in orderedZoomLevels)
            {
                double distance = Math.Abs(current.Resolution - resolution);
                if (distance < resultDistance)
                {
                    result = current;
                    resultDistance = distance;
                }
            }

            return result;
        }
    }
}
