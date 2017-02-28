using System;
using System.Collections.Generic;
using System.Net;

namespace MappingTiles
{
    public class ImageTileDownloader : TileDownloader
    {
        protected object webClientsPoolLockObject = new object();

        protected Dictionary<string, WebClient> webClientsPool;
        protected Dictionary<string, Uri> webRequestCache;

        public ImageTileDownloader()
            : base()
        {
            this.webClientsPool = new Dictionary<string, WebClient>();
            this.webRequestCache = new Dictionary<string, Uri>();
        }

        public override void StartDownload(Uri tileUri, TileInfo tileInfo)
        {
            if (TileCache != null)
            {
                byte[] bytes = TileCache.Read(tileInfo);
                if (bytes != null)
                {
                    tileInfo.Content = bytes;

                    // raise the event
                    this.OnTileDownloadComplete(new TileInfoEventArgs(tileInfo));

                    return;
                }
            }

            lock (this.webClientsPoolLockObject)
            {
                if (!this.webClientsPool.ContainsKey(tileInfo.Id))
                {
                    WebClient webClient = new WebClient();
                    if (Credentials != null)
                    {
                        webClient.Credentials = Credentials;
                    }
                    if (WebProxy != null)
                    {
                        webClient.Proxy = WebProxy;
                    }

                    this.webClientsPool.Add(tileInfo.Id, webClient);
                    this.webRequestCache.Add(tileInfo.Id, tileUri);

                    ImageTileDownloader imageTileDownloader = this;
                    webClient.DownloadDataCompleted += new DownloadDataCompletedEventHandler(imageTileDownloader.DownloadTileDataCompleted);
                    webClient.DownloadDataAsync(tileUri, tileInfo);
                }
            }
        }

        public override void CancelDownload(TileInfo tileInfo)
        {
            lock (this.webClientsPoolLockObject)
            {
                if (this.webClientsPool.ContainsKey(tileInfo.Id))
                {
                    this.webClientsPool[tileInfo.Id].CancelAsync();

                    ImageTileDownloader imageTileDownloader = this;
                    this.webClientsPool[tileInfo.Id].DownloadDataCompleted -= new DownloadDataCompletedEventHandler(imageTileDownloader.DownloadTileDataCompleted);
                    this.webClientsPool.Remove(tileInfo.Id);
                    this.webRequestCache.Remove(tileInfo.Id);
                }
            }
        }

        protected virtual bool ShouldRetryDownload(Exception error)
        {
            WebException webException = error as WebException;
            if (webException == null || webException.Status == WebExceptionStatus.RequestCanceled)
            {
                return false;
            }
            HttpWebResponse response = webException.Response as HttpWebResponse;
            if (response != null && response.StatusCode == HttpStatusCode.NotFound)
            {
                return true;
            }
            return false;
        }

        protected virtual void DownloadTileDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            TileInfo userState = (TileInfo)e.UserState;
            if (e.Error == null)
            {
                userState.Content = e.Result;
                // if tileCache is applied, cache the request tile
                if (TileCache != null)
                {
                    TileCache.Save(userState, userState.Content);
                }

                this.OnTileDownloadComplete(new TileInfoEventArgs(userState));
                lock (this.webClientsPoolLockObject)
                {
                    ImageTileDownloader imageTileDownloader = this;
                    this.webClientsPool[userState.Id].DownloadDataCompleted -= new DownloadDataCompletedEventHandler(imageTileDownloader.DownloadTileDataCompleted);
                    this.webClientsPool.Remove(userState.Id);
                    this.webRequestCache.Remove(userState.Id);
                }
            }
            else if (this.ShouldRetryDownload(e.Error))
            {
                Uri item = this.webRequestCache[userState.Id];
                lock (this.webClientsPoolLockObject)
                {
                    item = new Uri(this.webClientsPool[userState.Id].BaseAddress);

                    ImageTileDownloader imageTileDownloader = this;
                    this.webClientsPool[userState.Id].DownloadDataCompleted -= new DownloadDataCompletedEventHandler(imageTileDownloader.DownloadTileDataCompleted);
                    this.webClientsPool.Remove(userState.Id);
                    this.webRequestCache.Remove(userState.Id);
                }
                this.StartDownload(item, userState);
            }
        }
    }
}
