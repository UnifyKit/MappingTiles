using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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
            TileRequest TileRequest = new TileRequest(uri, callback)
            {
                NetworkPriority = networkPriority
            };

            TileRequest tempTileRequest = TileRequest;
            lock (this.pendingRequests)
            {
                this.pendingRequests.Add(tempTileRequest);
            }
            this.thereMayBeWorkToDo.Set();

            return tempTileRequest;
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
            key.Callback(bitmapImage, error);
            ((WebClient)sender).Dispose();
        }

        private void DownloadThreadStart()
        {
            while (true)
            {
                WaitHandle[] waitHandleArray = new WaitHandle[] { this.thereMayBeWorkToDo };
                WaitHandle.WaitAll(waitHandleArray);

                TileRequest item = null;
                lock (this.executingRequests)
                {
                    lock (this.pendingRequests)
                    {
                        (from wr in this.pendingRequests
                         where wr.IsAborted
                         select wr).ToList<TileRequest>().ForEach((TileRequest wr) => this.pendingRequests.Remove(wr));

                        foreach (TileRequest pendingRequest in this.pendingRequests)
                        {
                            pendingRequest.NetworkPrioritySnapshot = pendingRequest.NetworkPriority;
                        }

                        this.pendingRequests.Sort((TileRequest left, TileRequest right) => Comparer<int>.Default.Compare(left.NetworkPrioritySnapshot, right.NetworkPrioritySnapshot));
                        if (this.executingRequests.Count >= TileRequestQueue.MaxSimultaneousRequests || this.pendingRequests.Count <= 0)
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
