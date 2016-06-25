using System;
using SharpDX;
using SharpDX.Direct2D1;

namespace RetroEngine
{
    abstract class GameObject : IDisposable
    {
        private Vector3 pos;
        private Vector3 rotation;
        private Vector3 scale;
        private Color4 color;
        private Bitmap texture;
        private Size2 textureSize;

        public GameObject(Vector3 pos, Bitmap texture)
        {
            this.pos = pos;
            rotation = new Vector3(0, 0, 0);
            scale = new Vector3(1, 1, 1);
            color = new Color4(SharpDX.Color.Green.R / 255F, SharpDX.Color.Green.G / 255F, SharpDX.Color.Green.B / 255F, 1);
            this.texture = texture;
            if (texture != null)
                textureSize = texture.PixelSize;
        }

        public GameObject(Vector3 pos, Vector3 rotation, Vector3 scale, Bitmap texture)
        {
            this.pos = pos;
            this.rotation = rotation;
            this.scale = scale;
            color = new Color4(SharpDX.Color.Green.R / 255F, SharpDX.Color.Green.G / 255F, SharpDX.Color.Green.B / 255F, 1);
            this.texture = texture;
            if (texture != null)
                textureSize = texture.PixelSize;
        }
        abstract public void update();

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

        /// <summary>
        /// Gets or sets the texture of the wall.
        /// </summary>
        public Bitmap Texture
        {
            get { return texture; }
            set
            {
                textureSize = value.PixelSize;
                texture = value;
            }
        }

        /// <summary>
        /// Gets the texture size in pixels. (Read only)
        /// </summary>
        public Size2 TextureSize
        {
            get { return textureSize; }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    texture.Dispose();
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
