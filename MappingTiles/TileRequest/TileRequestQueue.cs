using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MappingTiles
{
    public class TileRequestQueue : IDisposable
    {
        private static int MaxSimultaneousRequests;
        private static TileRequestQueue instance;

        private List<TileRequest> pendingRequests;
        private Dictionary<TileRequest, WebClient> executingRequests;
        private Thread downloadThread;
        private ManualResetEvent thereMayBeWorkToDo;

        static TileRequestQueue()
		{
            TileRequestQueue.MaxSimultaneousRequests = 6;
		}

        private TileRequestQueue()
		{
            this.pendingRequests = new List<TileRequest>();
            this.executingRequests = new Dictionary<TileRequest, WebClient>();
			this.thereMayBeWorkToDo = new ManualResetEvent(true);
			Thread thread = new Thread(new ThreadStart(this.DownloadThreadStart))
			{
				IsBackground = true
			};
			this.downloadThread = thread;
			this.downloadThread.Start();
		}

        public static TileRequestQueue Instance
        {
            get
            {
                if (TileRequestQueue.instance == null)
                {
                    TileRequestQueue.instance = new TileRequestQueue();
                }
                return TileRequestQueue.instance;
            }
        }

        public TileRequest CreateRequest(Uri uri, NetworkPriority networkPriority, TileRequestCompletedHandler callback)
        {
            TileRequest tileRequest = new TileRequest(uri, callback)
            {
                NetworkPriority = networkPriority
            };
            TileRequest bitmapImageRequest1 = tileRequest;
            lock (this.pendingRequests)
            {
                this.pendingRequests.Add(bitmapImageRequest1);
            }
            this.thereMayBeWorkToDo.Set();
            return bitmapImageRequest1;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.thereMayBeWorkToDo.Dispose();
            }
        }

        private void DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            TileRequest key = null;
            lock (this.executingRequests)
            {
                KeyValuePair<TileRequest, WebClient> keyValuePair = this.executingRequests.First<KeyValuePair<TileRequest, WebClient>>((KeyValuePair<TileRequest, WebClient> item) => item.Value == sender);
                key = keyValuePair.Key;
                this.executingRequests.Remove(key);
                this.thereMayBeWorkToDo.Set();
            }
            BitmapImage bitmapImage = null;
            Exception error = e.Error;
            if (error == null)
            {
                try
                {
                    if ((int)e.Result.Length <= 0)
                    {
                        error = new Exception("empty result");
                    }
                    else
                    {
                        bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = new MemoryStream(e.Result);
                        bitmapImage.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable);
                        bitmapImage.CacheOption = BitmapCacheOption.None;
                        bitmapImage.EndInit();
                        bitmapImage.Freeze();
                    }
                }
                catch (Exception exception)
                {
                    error = exception;
                    bitmapImage = null;
                }
            }
            key.Callback(key.UserToken, bitmapImage, error);
            ((WebClient)sender).Dispose();
        }

        private void DownloadThreadStart()
        {
            while (true)
            {
                WaitHandle[] waitHandleArray = new WaitHandle[] { this.thereMayBeWorkToDo };
                WaitHandle.WaitAll(waitHandleArray);
                BitmapImageRequest item = null;
                lock (this.executingRequests)
                {
                    lock (this.pendingRequests)
                    {
                        (
                            from wr in this.pendingRequests
                            where wr.Aborted
                            select wr).ToList<BitmapImageRequest>().ForEach((BitmapImageRequest wr) => this.pendingRequests.Remove(wr));
                        foreach (BitmapImageRequest pendingRequest in this.pendingRequests)
                        {
                            pendingRequest.NetworkPrioritySnapshot = pendingRequest.NetworkPriority;
                        }
                        this.pendingRequests.Sort((BitmapImageRequest left, BitmapImageRequest right) => Comparer<int>.Default.Compare(left.NetworkPrioritySnapshot, right.NetworkPrioritySnapshot));
                        if (this.executingRequests.Count >= BitmapImageRequestQueue.MaxSimultaneousRequests || this.pendingRequests.Count <= 0)
                        {
                            this.thereMayBeWorkToDo.Reset();
                        }
                        else
                        {
                            item = this.pendingRequests[this.pendingRequests.Count - 1];
                            this.pendingRequests.RemoveAt(this.pendingRequests.Count - 1);
                        }
                    }
                    if (item != null)
                    {
                        WebClient webClient = new WebClient()
                        {
                            CachePolicy = new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable)
                        };
                        webClient.DownloadDataCompleted += new DownloadDataCompletedEventHandler(this.DownloadDataCompleted);
                        this.executingRequests.Add(item, webClient);
                        webClient.DownloadDataAsync(item.Uri, null);
                    }
                }
            }
        }
    }
}
