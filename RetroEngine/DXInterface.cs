using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using System;

namespace RetroEngine
{
    /// <summary>
    /// Provides an easy to use interface for the SharpDX library.
    /// </summary>
    class DXInterface : IDisposable
    {
        private RenderForm renderForm;
        private SharpDX.Direct3D11.Device device3d;
        private SharpDX.Direct2D1.Device device2d;
        private SwapChain swapChain;
        private SharpDX.Direct2D1.DeviceContext context2d;
        private Surface backBuffer;
        private Bitmap1 target2d;

        /// <summary>
        /// Create a new DirectX context and an empty window.
        /// </summary>
        /// <param name="title">The caption of the window.</param>
        public DXInterface(string title)
        {
            Debug.Log("Initializing DirectX...", MessageState.Info);

            //Create a new window to draw onto
            renderForm = new RenderForm(title);

            //320 x 200 is the original doom resolution
            renderForm.ClientSize = new System.Drawing.Size(320, 200);

            //Create a swap chain and a Direct3D device
            CreateD3DDependencies();

            //Create a Direct2D device and a Direct2D context
            CreateD2DDependencies();

            //Let the swap chain know what window to draw into
            AssociateWithWindow();

            //Create the surface bitmap to draw onto
            CreateTarget();
        }

        private void CreateD3DDependencies()
        {
            //Setting the properties for a new swap chain
            SwapChainDescription desc = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription = new ModeDescription(renderForm.ClientSize.Width, renderForm.ClientSize.Height, new Rational(60, 1), Format.B8G8R8A8_UNorm),
                IsWindowed = true,
                OutputHandle = renderForm.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            //Create the swap chain and the DirectX device from the description and save the references
            //Set the flag Bgra support to be compatible with Direct2D
            SharpDX.Direct3D11.Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, desc, out device3d, out swapChain);
        }

        private void CreateD2DDependencies()
        {
            //Get the dxgi device from the d3d device
            SharpDX.DXGI.Device deviceDxgi = device3d.QueryInterface<SharpDX.DXGI.Device>();

            //Create the d2d device from a dxgi device
            device2d = new SharpDX.Direct2D1.Device(deviceDxgi);

            //Dispose this device since it fulfilled its purpose
            deviceDxgi.Dispose();

            //Create the 2d context
            context2d = new SharpDX.Direct2D1.DeviceContext(device2d, DeviceContextOptions.None);
        }

        private void AssociateWithWindow()
        {
            //Give the swap chain the window it will draw onto
            SharpDX.DXGI.Factory factory = swapChain.GetParent<SharpDX.DXGI.Factory>();
            factory.MakeWindowAssociation(renderForm.Handle, WindowAssociationFlags.IgnoreAll);
            factory.Dispose();
        }

        private void CreateTarget()
        {
            //Get the reference of the back buffer from the swap chain
            backBuffer = Surface.FromSwapChain(swapChain, 0);

            //Create the render target
            BitmapProperties1 properties = new BitmapProperties1(new PixelFormat(Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied), 96, 96, BitmapOptions.Target | BitmapOptions.CannotDraw);
            target2d = new Bitmap1(context2d, backBuffer, properties);

            //Set the target for the 2d context
            context2d.Target = target2d;
        }

        /// <summary>
        /// Gets the rendering window. (Read only)
        /// </summary>
        public RenderForm RenderForm
        {
            get { return renderForm; }
        }

        /// <summary>
        /// Gets the Direct2D context drawing into the back buffer. (Read only)
        /// </summary>
        public SharpDX.Direct2D1.DeviceContext Context2D
        {
            get { return context2d; }
        }

        /// <summary>
        /// Gets the swap chain responsible for flipping the current render target and the back buffer. (Read only)
        /// </summary>
        public SwapChain SwapChain
        {
            get { return swapChain; }
        }

        /// <summary>
        /// Toggles fullscreen and windowed mode.
        /// </summary>
        public void ToggleFullscreen()
        {
            if (swapChain.IsFullScreen)
                swapChain.SetFullscreenState(false, null);
            else
                swapChain.SetFullscreenState(true, null);
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //Free the memory
                    backBuffer.Dispose();
                    device3d.Dispose();
                    device2d.Dispose();
                    context2d.Dispose();
                    swapChain.Dispose();
                    target2d.Dispose();
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
