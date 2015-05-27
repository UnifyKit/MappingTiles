using System;

namespace MappingTiles
{
    public class AsyncCompletedEventArgs : EventArgs
    {
        private AsyncCompletedEventArgs()
        { }

        public AsyncCompletedEventArgs(Exception error, bool cancelled, object userState)
        {
            Error = error;
            Cancelled = cancelled;
            UserState = userState;
        }

        ///<summary>
        /// Gets a value indicating whether an asynchronous operation has been canceled.
        /// </summary>
        public bool Cancelled
        {
            get;
            private set;
        }

        ///<summary>
        /// Gets a value indicating which error occurred during an asynchronous operation.
        /// </summary>
        public Exception Error
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the unique identifier for the asynchronous task.
        /// </summary>
        public object UserState
        {
            get;
            private set;
        }
    }
}
