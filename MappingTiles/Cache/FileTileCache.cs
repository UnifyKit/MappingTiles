using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

namespace MappingTiles
{
    public class FileTileCache<T> : ITileCache<T>
    {
        private readonly ReaderWriterLockSlim readerWriterLocker = new ReaderWriterLockSlim();
        private readonly string directory;
        private readonly string format;
        private readonly TimeSpan cacheExpireTime;

        /// <remarks>
        /// The constructor creates the storage _directory if it does not exist.
        /// </remarks>
        public FileTileCache(string directory, string format)
            : this(directory, format, TimeSpan.Zero)
        {
        }

        /// <remarks>
        ///   The constructor creates the storage _directory if it does not exist.
        /// </remarks>
        public FileTileCache(string directory, string format, TimeSpan cacheExpireTime)
        {
            this.directory = directory;
            this.format = format;
            this.cacheExpireTime = cacheExpireTime;

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public void Add(string id, T tile)
        {
            try
            {
                readerWriterLocker.EnterWriteLock();
                if (Exists(index))
                {
                    return; // ignore
                }
                string dir = GetDirectoryName(index);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                WriteToFile(image, index);
            }
            finally
            {
                readerWriterLocker.ExitWriteLock();
            }
        }

        public void Remove(string id)
        {
            try
            {
                readerWriterLocker.EnterWriteLock();
                if (Exists(index))
                {
                    File.Delete(GetFileName(index));
                }
            }
            finally
            {
                readerWriterLocker.ExitWriteLock();
            }
        }

        public T Get(string id)
        {
            try
            {
                readerWriterLocker.EnterReadLock();
                if (!Exists(index)) return null; // to indicate not found
                return File.ReadAllBytes(GetFileName(index));
            }
            finally
            {
                readerWriterLocker.ExitReadLock();
            }
        }

        public bool Exists(TileIndex index)
        {
            if (File.Exists(GetFileName(index)))
            {
                return cacheExpireTime == TimeSpan.Zero || (DateTime.Now - new FileInfo(GetFileName(index)).LastWriteTime) <= cacheExpireTime;
            }
            return false;
        }


        public string GetFileName(string id)
        {
            return Path.Combine(GetDirectoryName(id), string.Format(CultureInfo.InvariantCulture, "{0}.{1}", id.Row, format));
        }

        private string GetDirectoryName(string index)
        {
            var level = index.Level.ToString(CultureInfo.InvariantCulture);
            level = level.Replace(':', '_');
            return Path.Combine(directory,
                level,
                index.Col.ToString(CultureInfo.InvariantCulture));
        }

        private void WriteToFile(byte[] image, string id)
        {
            using (FileStream fileStream = File.Open(GetFileName(id), FileMode.Create))
            {
                fileStream.Write(image, 0, image.Length);
                fileStream.Flush();
                fileStream.Close();
            }
        }
    }
}
