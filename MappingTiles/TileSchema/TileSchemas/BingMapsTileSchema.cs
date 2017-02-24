
namespace MappingTiles
{
    public class BingMapsTileSchema : SphericalMercatorTileSchema
    {
        public BingMapsTileSchema()
        {
            ZoomLevels.RemoveAt(0);     // Bing Maps does't have the signle tile showing the whole world.
        }
    }
}
