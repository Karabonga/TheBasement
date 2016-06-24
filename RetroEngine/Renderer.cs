using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine
{
    /// <summary>
    /// A class containing basic rendering information.
    /// </summary>
    abstract class Renderer : IDisposable
    {
        private DXInterface dxInterface;

        public Renderer(DXInterface dxInterface)
        {
            this.dxInterface = dxInterface;
        }

        /// <summary>
        /// The DirectX interface for to render with.
        /// </summary>
        public DXInterface DXInterface
        {
            get { return dxInterface; }
        }

        /// <summary>
        /// Renders the given scene.
        /// </summary>
        /// <param name="scene">The scene to be rendered.</param>
        public abstract void RenderScene(Scene scene);

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    dxInterface.Dispose();
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
