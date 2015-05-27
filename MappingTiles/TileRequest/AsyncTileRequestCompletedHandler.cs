using System;
using System.Windows.Media.Imaging;

namespace MappingTiles
{
    public delegate void AsyncTileRequestCompletedHandler(BitmapImage result, Exception error);
}
