using System.Collections.Generic;
using SharpDX.Direct2D1;
using System.IO;
using SharpDX;
using System;

namespace RetroEngine
{
    /// <summary>
    /// Loads and manages all textures.
    /// </summary>
    class TextureManager
    {
        Dictionary<string, Bitmap> textures;

        public TextureManager()
        {
            textures = new Dictionary<string, Bitmap>();
        }

        /// <summary>
        /// Loads a texture from the picture file.
        /// </summary>
        /// <param name="file">The name of the file.</param>
        public void Load(string file)
        {
            System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(file);
            string[] fileName = file.Split(new char[] { '\\' });
            textures.Add(fileName[fileName.Length - 1], DrawingToSharpDX(bmp));
            Debug.Log("Loaded texture '" + fileName[fileName.Length - 1] + "'!");
        }

        public void LoadFolder(string folder)
        {
            string pathTo = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string[] files = Directory.GetFiles(pathTo +  "\\" + folder);
            for (int i = 0; i < files.Length; i++)
                Load(files[i]);
        }

        private unsafe Bitmap DrawingToSharpDX(System.Drawing.Bitmap bitmap)
        {
            int stride = bitmap.Width * sizeof(int);
            using (var tempStream = new DataStream(bitmap.Height * stride, true, true))
            {
                var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var bitmapData = bitmap.LockBits(sourceArea, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                uint* p = (uint*)bitmapData.Scan0;
                for (int i = 0; i < bitmap.Width * bitmap.Height; i++)
                {
                    byte* channels = (byte*)p;
                    tempStream.Write(channels[2]);
                    tempStream.Write(channels[1]);
                    tempStream.Write(channels[0]);
                    tempStream.Write(channels[3]);
                    p++;
                }

                bitmap.UnlockBits(bitmapData);
                tempStream.Position = 0;

                var bitmapProperties = new BitmapProperties(new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.R8G8B8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied));
                return new Bitmap(GameConstants.Context2D, new Size2(bitmap.Width, bitmap.Height), tempStream, stride, bitmapProperties);
            }
        }

        /// <summary>
        /// Gets the textures stored by the manager. (Read only)
        /// </summary>
        public Dictionary<string, Bitmap> Textures
        {
            get { return textures; }
        }
    }
}
