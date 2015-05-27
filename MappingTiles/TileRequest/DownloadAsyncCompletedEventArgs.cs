using System;

namespace MappingTiles
{
    public class DownloadAsyncCompletedEventArgs : AsyncCompletedEventArgs
    {
        private DownloadAsyncCompletedEventArgs()
        { }

        public DownloadAsyncCompletedEventArgs(byte[] result,Exception error, bool cancelled, object userState)
            : base(error, cancelled, userState)
        { }

        public byte[] Result
        {
            get;
        }
    }
}
