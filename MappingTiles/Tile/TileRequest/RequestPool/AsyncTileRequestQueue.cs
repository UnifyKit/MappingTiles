﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Threading;

namespace MappingTiles
{
    internal class AsyncTileRequestQueue : IDisposable
    {
        private static int MaxSimultaneousRequests;
        private static AsyncTileRequestQueue instance;
        private List<AsyncTileRequest> pendingRequests;
        private Dictionary<AsyncTileRequest, WebClient> executingRequests;
        private Thread downloadThread;
        private ManualResetEvent thereMayBeWorkToDo;

        private static ITileCache<byte[]> tileCache;

        static AsyncTileRequestQueue()
        {
            AsyncTileRequestQueue.MaxSimultaneousRequests = 6;
            WebProxy = null;
            TileCache = new MemoryTileCache<byte[]>();
        }

        private AsyncTileRequestQueue()
        {
            this.pendingRequests = new List<AsyncTileRequest>();
            this.executingRequests = new Dictionary<AsyncTileRequest, WebClient>();
            this.thereMayBeWorkToDo = new ManualResetEvent(true);
            this.downloadThread = new Thread(new ThreadStart(this.DownloadThreadStart))
            {
                IsBackground = true
            };
            this.downloadThread.Start();
        }

        public static AsyncTileRequestQueue Instance
        {
            get
            {
                if (AsyncTileRequestQueue.instance == null)
                {
                    AsyncTileRequestQueue.instance = new AsyncTileRequestQueue();
                }
                return AsyncTileRequestQueue.instance;
            }
        }

        public static IWebProxy WebProxy
        {
            get;
            set;
        }

        public static ICredentials Credentials
        {
            get;
            set;
        }

        public static ITileCache<byte[]> TileCache
        {
            get
            {
                return tileCache;
            }
            internal set
            {
                tileCache = value;
            }
        }

        public AsyncTileRequest CreateRequest(Uri uri, TileInfo tileInfo, NetworkPriority networkPriority, AsyncTileRequestCompletedHandler callback)
        {
            AsyncTileRequest tempTileRequest = new AsyncTileRequest(uri, tileInfo, callback)
            {
                NetworkPriority = networkPriority
            };
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

        private void DownloadThreadStart()
        {
            while (true)
            {
                WaitHandle[] waitHandleArray = new WaitHandle[] { this.thereMayBeWorkToDo };
                WaitHandle.WaitAll(waitHandleArray);

                AsyncTileRequest tempTileREQUEST = null;
                lock (this.executingRequests)
                {
                    lock (this.pendingRequests)
                    {
                        (from pendingRequest in this.pendingRequests
                         where pendingRequest.IsAborted
                         select pendingRequest).ToList<AsyncTileRequest>().ForEach((AsyncTileRequest tr) => this.pendingRequests.Remove(tr));

                        foreach (AsyncTileRequest pendingRequest in this.pendingRequests)
                        {
                            pendingRequest.NetworkPrioritySnapshot = pendingRequest.NetworkPriority;
                        }

                        this.pendingRequests.Sort((AsyncTileRequest left, AsyncTileRequest right) => Comparer<int>.Default.Compare((int)left.NetworkPrioritySnapshot, (int)right.NetworkPrioritySnapshot));
                        if (this.executingRequests.Count >= AsyncTileRequestQueue.MaxSimultaneousRequests || this.pendingRequests.Count <= 0)
                        {
                            this.thereMayBeWorkToDo.Reset();
                        }
                        else
                        {
                            tempTileREQUEST = this.pendingRequests[this.pendingRequests.Count - 1];
                            this.pendingRequests.RemoveAt(this.pendingRequests.Count - 1);
                        }
                    }
                    if (tempTileREQUEST != null)
                    {
                        WebClient webClient = new WebClient()
                        {
                            CachePolicy = new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable),
                            Proxy = WebProxy,
                            Credentials = Credentials
                        };
                        webClient.DownloadDataCompleted += new DownloadDataCompletedEventHandler(this.DownloadDataCompleted);

                        this.executingRequests.Add(tempTileREQUEST, webClient);
                        webClient.DownloadDataAsync(tempTileREQUEST.Uri, null);
                    }
                }
            }
        }

        private void DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            AsyncTileRequest key = null;
            lock (this.executingRequests)
            {
                KeyValuePair<AsyncTileRequest, WebClient> keyValuePair = this.executingRequests.First<KeyValuePair<AsyncTileRequest, WebClient>>((KeyValuePair<AsyncTileRequest, WebClient> item) => item.Value == sender);
                key = keyValuePair.Key;
                this.executingRequests.Remove(key);
                this.thereMayBeWorkToDo.Set();
            }
            byte[] requestedTileInBytes = null;
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
                        requestedTileInBytes = e.Result;
                    }
                }
                catch (Exception exception)
                {
                    error = exception;
                    requestedTileInBytes = null;
                }
            }

            // Save the requested tile into cache.
            if (requestedTileInBytes != null && key.TileInfo != null && TileCache != null)
            {
                TileCache.Save(key.TileInfo, requestedTileInBytes);
            }

            key.Callback(requestedTileInBytes, error);

            ((WebClient)sender).Dispose();
        }
    }
}
