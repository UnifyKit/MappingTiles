using System;
using System.Windows.Media.Imaging;

namespace MappingTiles
{
    public delegate void TileRequestCompletedHandler(BitmapImage result, Exception error);
}
