using System.Globalization;

namespace MappingTiles
{
    public class Wgs84TileScehma : TileSchema
    {
        private const double MaxResolution = 1.40625;
        private int numberOfZoomLevels = 20;

        public Wgs84TileScehma()
            : base()
        {
            Crs = "EPSG:4326";
            BoundingBox = new BoundingBox(-180, -90, 180, 90);

            InitializeZoomLevels();
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

            double resolution = MaxResolution;
            for (int i = 0; i < numberOfZoomLevels; i++)
			{
                var ZoomLevel = new ZoomLevel(resolution, (i + 1).ToString(CultureInfo.InvariantCulture));
                ZoomLevels.Add(ZoomLevel);

                resolution /= 2;
			}
        }
    }
}
