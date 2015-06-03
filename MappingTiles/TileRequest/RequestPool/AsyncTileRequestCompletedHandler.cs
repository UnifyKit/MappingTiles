using System;

namespace MappingTiles
{
    internal delegate void AsyncTileRequestCompletedHandler(byte[] result, Exception error);
}
