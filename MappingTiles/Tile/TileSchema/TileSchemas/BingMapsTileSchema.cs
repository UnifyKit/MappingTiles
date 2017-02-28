using System.Collections.ObjectModel;

namespace MappingTiles
{
    public class BingMapsTileSchema : SphericalMercatorTileSchema
    {
        public BingMapsTileSchema()
            : base()
        {
        }

        protected override Collection<ZoomLevel> GetZoomLevelsCore()
        {
            Collection<ZoomLevel> zoomLevels = base.GetZoomLevelsCore();
            zoomLevels.RemoveAt(0);

            return zoomLevels;
        }
    }
}
