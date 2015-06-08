using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MappingTiles
{
    public class SphericalMercatorTileSchema : TileSchema
    {
        private const double ScaleFactor = 78271.51696401953125;
        private const int defaultZoomLevelNumbers = 20;
        
        private int numberOfZoomLevels = 20;

        public SphericalMercatorTileSchema() :
            this(GetZoomLevels(defaultZoomLevelNumbers), TileFormat.Png)
        {
        }

        public SphericalMercatorTileSchema(TileFormat format) :
            this(GetZoomLevels(defaultZoomLevelNumbers), TileFormat.Png)
        {
        }

        internal SphericalMercatorTileSchema(IEnumerable<ZoomLevel> zoomLevels, TileFormat format)
        {
            TileFormat = format;
            Crs = "EPSG:3857";
            IsYAxisReversed = true;
            foreach (var zoomLevel in zoomLevels)
            {
                ZoomLevels.Add(zoomLevel);
            }
            BoundingBox = new BoundingBox(-20037508.342789, -20037508.342789, 20037508.342789, 20037508.342789);
        }

        public int NumberOfZoomLevels
        {
            get { return numberOfZoomLevels; }
            set
            {
                if (numberOfZoomLevels != value)
                {
                    InitializeZoomLevels();
                }

                numberOfZoomLevels = value;
            }
        }

        private void InitializeZoomLevels()
        {
            ZoomLevels.Clear();

            Collection<ZoomLevel> zoomlevels = GetZoomLevels(NumberOfZoomLevels);
            foreach (var zoomLevel in zoomlevels)
            {
                ZoomLevels.Add(zoomLevel);
            }
        }

        private static Collection<ZoomLevel> GetZoomLevels(int numberOfZoomLevels)
        {
            Collection<ZoomLevel> zoomlevels = new Collection<ZoomLevel>();

            for (int i = 0; i < numberOfZoomLevels; i++)
            {
                var resolution = 2 * ScaleFactor / (1 << i);
                var zoomLevel = new ZoomLevel(resolution, (i + 1).ToString(CultureInfo.InvariantCulture));
                zoomlevels.Add(zoomLevel);
            }

            return zoomlevels;
        }
    }
}
