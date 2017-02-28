using System;

namespace MappingTiles
{
    public class TileInfoEventArgs : EventArgs
    {
        private TileInfo tileInfo;

        public TileInfo TileInfo
        {
            get
            {
                return this.tileInfo;
            }
        }

        public TileInfoEventArgs(TileInfo tileInfo)
        {
            this.tileInfo = tileInfo;
        }
    }
}
