using System;
using System.Globalization;
using System.IO;
using System.Threading;

namespace MappingTiles
{
    public class FileTileCache : ITileCache<byte[]>
    {
        private readonly ReaderWriterLockSlim readerWriterLocker = new ReaderWriterLockSlim();

        private readonly string directory;
        private readonly TileFormat format;
        private TimeSpan expirationTime;

        public FileTileCache()
            : this(Path.Combine(Path.GetTempPath(), Utility.CreateUniqueId()), TileFormat.Png, TimeSpan.Zero)
        {
        }

        public FileTileCache(string directory)
            : this(directory, TileFormat.Png, TimeSpan.Zero)
        {
        }

        public FileTileCache(string directory, TileFormat format)
            : this(directory, format, TimeSpan.Zero)
        {
        }

        public FileTileCache(string directory, TileFormat format, TimeSpan expirationTime)
        {
            this.directory = directory;
            this.format = format;
            this.expirationTime = expirationTime;

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public TimeSpan ExpirationTime
        {
            get { return expirationTime; }
            set { expirationTime = value; }
        }


        public void Save(TileInfo tileInfo, byte[] tile)
        {
            try
            {
                string directory = GetDirectoryName(tileInfo);

                readerWriterLocker.EnterWriteLock();
                if (Exists(tileInfo))
                {
                    return;
                }
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                WriteToFile(tile, tileInfo);
            }
            finally
            {
                readerWriterLocker.ExitWriteLock();
            }
        }

        public void Remove(TileInfo tileInfo)
        {
            try
            {
                readerWriterLocker.EnterWriteLock();
                if (Exists(tileInfo))
                {
                    File.Delete(GetCachedTileFilePathName(tileInfo));
                }
            }
            finally
            {
                readerWriterLocker.ExitWriteLock();
            }
        }

        public byte[] Read(TileInfo tileInfo)
        {
            try
            {
                readerWriterLocker.EnterReadLock();
                if (!Exists(tileInfo))
                {
                    return null;
                }

                return File.ReadAllBytes(GetCachedTileFilePathName(tileInfo));
            }
            finally
            {
                readerWriterLocker.ExitReadLock();
            }
        }

        public bool Exists(TileInfo tileInfo)
        {
            if (File.Exists(GetCachedTileFilePathName(tileInfo)))
            {
                return expirationTime == TimeSpan.Zero || (DateTime.Now - new FileInfo(GetCachedTileFilePathName(tileInfo)).LastWriteTime) <= expirationTime;
            }

            return false;
        }

        public void Clear()
        {
            try
            {
                readerWriterLocker.EnterWriteLock();
                if (!Directory.Exists(directory))
                {
                    Directory.Delete(directory);
                }
            }
            finally
            {
                readerWriterLocker.ExitWriteLock();
            }
        }

        public string GetCachedTileFilePathName(TileInfo tileInfo)
        {
            return Path.Combine(GetDirectoryName(tileInfo), string.Format(CultureInfo.InvariantCulture, "{0}.{1}", tileInfo.TileY, format.ToString()));
        }

        private string GetDirectoryName(TileInfo tileInfo)
        {
            string level = tileInfo.ZoomLevel.Id.ToString(CultureInfo.InvariantCulture);
            return Path.Combine(directory, level, tileInfo.TileX.ToString(CultureInfo.InvariantCulture));
        }

        private void WriteToFile(byte[] data, TileInfo tileInfo)
        {
            using (FileStream fileStream = File.Open(GetCachedTileFilePathName(tileInfo), FileMode.Create))
            {
                fileStream.Write(data, 0, data.Length);
                fileStream.Flush();
                fileStream.Close();
            }
        }
    }
}
