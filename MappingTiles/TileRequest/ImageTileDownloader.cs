using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MappingTiles
{
    public class ImageTileDownloader : TileDownloader
    {
        public override void CancelTileDownload(TileInfo tileInfo)
        {
            throw new NotImplementedException();
        }

        public override void DownloadTile(TileInfo tileInfo)
        {
            throw new NotImplementedException();
        }

        public override void UpdateTileDownloadPriority(TileInfo tileInfo, int priority)
        {
            throw new NotImplementedException();
        }
    }
}
