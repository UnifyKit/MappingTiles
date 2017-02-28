using System.Collections.ObjectModel;
using System.Globalization;

namespace MappingTiles
{
    public class SphericalMercatorTileSchema : TileSchema
    {
        private const double ScaleFactor = 78271.51696401953125;
        private const int defaultZoomLevelNumbers = 20;

        public SphericalMercatorTileSchema()
        {
            Crs = "EPSG:3857";
            IsYAxisReversed = true;
            MaxExtent = new BoundingBox(-20037508.342789, -20037508.342789, 20037508.342789, 20037508.342789);
            MinZoomLevel = new ZoomLevel(0);
            MaxZoomLevel = new ZoomLevel(); // take the max resolution
        }

        protected override Collection<ZoomLevel> GetZoomLevelsCore()
        {
            Collection<ZoomLevel> zoomlevels = new Collection<ZoomLevel>();

            for (int i = 0; i < NumberOfZoomLevels; i++)
            {
                var resolution = 2 * ScaleFactor / (1 << i);
                var zoomLevel = new ZoomLevel(resolution, (i + 1).ToString(CultureInfo.InvariantCulture));
                zoomlevels.Add(zoomLevel);
            }

            return zoomlevels;
        }
    }
}
