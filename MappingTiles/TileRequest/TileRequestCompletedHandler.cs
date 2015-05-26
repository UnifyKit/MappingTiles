using System;

namespace MappingTiles
{
    public delegate void TileRequestCompletedHandler(object userToken, TileImage result, Exception error);
}
