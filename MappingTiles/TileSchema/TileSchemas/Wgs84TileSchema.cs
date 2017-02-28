namespace MappingTiles
{
    public class Wgs84TileSchema : TileSchema
    {
        private const double MaxResolution = 1.40625;

        public Wgs84TileSchema()
            : base()
        {
            Crs = "EPSG:4326";
            MaxExtent = new BoundingBox(-180, -90, 180, 90);
            MinZoomLevel = new ZoomLevel(0);
            MaxZoomLevel = new ZoomLevel(MaxResolution);
        }
    }
}
