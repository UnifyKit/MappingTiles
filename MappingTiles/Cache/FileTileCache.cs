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
        private TimeSpan expireTime;

        public FileTileCache()
            : this(Path.Combine(Path.GetTempPath(), Utilities.CreateUniqueId()), TileFormat.Png, TimeSpan.Zero)
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

        public FileTileCache(string directory, TileFormat format, TimeSpan expireTime)
        {
            this.directory = directory;
            this.format = format;
            this.ExpireTime = expireTime;

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public TimeSpan ExpireTime
        {
            get { return expireTime; }
            set { expireTime = value; }
        }

        public void Add(TileInfo tileInfo, byte[] data)
        {
            try
            {
                readerWriterLocker.EnterWriteLock();
                if (Exists(tileInfo))
                {
                    return;
                }
                string directory = GetDirectoryName(tileInfo);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                WriteToFile(data, tileInfo);
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

        public byte[] Get(TileInfo tileInfo)
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
                return expireTime == TimeSpan.Zero || (DateTime.Now - new FileInfo(GetCachedTileFilePathName(tileInfo)).LastWriteTime) <= expireTime;
            }

            return false;
        }


        public string GetCachedTileFilePathName(TileInfo tileInfo)
        {
            return Path.Combine(GetDirectoryName(tileInfo), string.Format(CultureInfo.InvariantCulture, "{0}.{1}", tileInfo.Row, format.ToString()));
        }

        private string GetDirectoryName(TileInfo tileInfo)
        {
            string level = tileInfo.ZoomLevel.Id.ToString(CultureInfo.InvariantCulture);
            return Path.Combine(directory, level, tileInfo.Column.ToString(CultureInfo.InvariantCulture));
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
