using System;
using SharpDX;
using SharpDX.Direct2D1;

namespace RetroEngine
{
    class GameObject : IDisposable
    {
        private Vector3 pos;
        private Vector3 rotation;
        private Vector3 scale;
        private Color4 color;

        public GameObject(Vector3 pos)
        {
            this.pos = pos;
            rotation = new Vector3(0, 0, 0);
            scale = new Vector3(1, 1, 1);
            color = new Color4(SharpDX.Color.Green.R / 255F, SharpDX.Color.Green.G / 255F, SharpDX.Color.Green.B / 255F, 1);
        }

        public GameObject(Vector3 pos, Vector3 rotation, Vector3 scale)
        {
            this.pos = pos;
            this.rotation = rotation;
            this.scale = scale;
            color = new Color4(SharpDX.Color.Green.R / 255F, SharpDX.Color.Green.G / 255F, SharpDX.Color.Green.B / 255F, 1);
        }

        /// <summary>
        /// Gets or sets the position of the game object.
        /// </summary>
        public Vector3 Position
        {
            get { return pos; }
            set { pos = value; }
        }

        /// <summary>
        /// Gets or sets the rotation of the game object.
        /// </summary>
        public Vector3 Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        /// <summary>
        /// Gets or sets the scale of the game object.
        /// </summary>
        public Vector3 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        /// <summary>
        /// The brush of this game object.
        /// </summary>
        public Color4 Color
        {
            get { return color; }
            set { color = value; }
        }

        /// <summary>
        /// Gets the local forward vector of the object.
        /// </summary>
        public Vector3 Forward
        {
            get { return Mathf.RotateAroundY(new Vector3(0, 0, 1), rotation.Y); }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

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
