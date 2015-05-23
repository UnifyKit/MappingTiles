using System;

namespace MappingTiles
{
    public class ZoomLevel
    {
        private const double EquatorLength = 40075016.686;  // in meter.
        private const double ResolutionOfZoomLevel0 = 156543.034;   // 156543.034 is the resolution that the tile size at zoom level 0 in meter.

        private const int StandardDpi = 96;
        private const double InchPerMeter = 39.37;

        private double resolution;
        private double scale;
        private int screenDpi;

        public ZoomLevel()
            : this(ResolutionOfZoomLevel0)
        { }

        public ZoomLevel(double resolution)
        {
            this.screenDpi = StandardDpi;

            this.Resolution = resolution;
        }

        public double Resolution
        {
            get { return resolution; }
            set
            {
                resolution = value;
                ResolutionPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the scale on how 1 cm on a screen translates to 1cm of a map.
        /// </summary>
        public double Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                ScalePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the value on ho 1 cm on a screen translates to 1 cm of map.
        /// </summary>
        public int ScreenDpi
        {
            get { return screenDpi; }
            set
            {
                screenDpi = value;
                ScreenDpiPropertyChanged();
            }
        }

        public double GetConcreteResolution(double latitude, int zoomLevelIndex)
        {
            return ResolutionOfZoomLevel0 * Math.Cos(latitude) / (Math.Pow(2, zoomLevelIndex));
        }

        private void ResolutionPropertyChanged()
        {
            scale = screenDpi * InchPerMeter * resolution;
        }

        private void ScalePropertyChanged()
        {
            resolution = scale / (screenDpi * InchPerMeter);
        }

        private void ScreenDpiPropertyChanged()
        {
            scale = screenDpi * InchPerMeter * resolution;
        }
    }
}
