using SharpDX;
using System;

namespace RetroEngine
{
    abstract class Map : IDisposable
    {
        protected Size2 size;

        public Map(Size2 size)
        {
            this.size = size;
        }

        /// <summary>
        /// Gets the map size. (Read only)
        /// </summary>
        public Size2 Size
        {
            get { return size; }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //Do nothing here yet
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
