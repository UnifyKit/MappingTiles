using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MappingTiles
{
    public class BingMapsTileSource : TileSource
    {
        private readonly Collection<string> OptionDomains = new Collection<string>() { "0", "1", "2", "3", "4", "5", "6", "7" };
        private readonly string uriTemplate = "http://t{s}.tiles.virtualearth.net/tiles/a{quadkey}.jpeg?g=517&token={k}";

        public BingMapsTileSource()
            : this(string.Empty, BingMapsType.Roads)
        { }

        public BingMapsTileSource(string accessKey)
            : this(accessKey, BingMapsType.Roads)
        { }

        public BingMapsTileSource(string accessKey, BingMapsType mapType)
        {
            MapType = mapType;
            AccessKey = accessKey;
            Schema = new SphericalMercatorTileSchema();
        }

        // https://msdn.microsoft.com/en-us/library/ff428642.aspx
        public string AccessKey
        {
            get;
            set;
        }

        public BingMapsType MapType
        {
            get;
            set;
        }

        protected override Uri GetUriCore(TileInfo tileInfo)
        {
            string requestUri = uriTemplate;

            requestUri.Replace("{s}", GetNextServerDomain(OptionDomains));
            requestUri.Replace("{quadkey}", tileInfo.ZoomLevel.Id);
            requestUri.Replace("{k}", tileInfo.TileX.ToString(CultureInfo.InvariantCulture));

            return new Uri(requestUri);
        }
    }
}
