using System;

namespace MappingTiles
{
    public delegate void AsyncTileRequestCompletedHandler(byte[] result, Exception error);
}
