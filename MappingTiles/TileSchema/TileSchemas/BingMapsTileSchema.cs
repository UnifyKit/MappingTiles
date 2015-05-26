
namespace MappingTiles
{
    public class BingMapsTileSchema: SphericalMercatorTileSchema
    {
        public BingMapsTileSchema()
            : base(TileFormat.Jpeg)
        {
            ZoomLevels.RemoveAt(0);     // Bing Maps does't have the signle tile showing the whole world.
        }
    }
}
