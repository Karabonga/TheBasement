using SharpDX;
using SharpDX.Direct2D1;

namespace RetroEngine
{
    class Animation
    {
        Bitmap[] textures;
        float cycleTime;

        public Animation(Bitmap[] textures, float cycleTime)
        {
            this.textures = textures;
            this.cycleTime = cycleTime;
        }

        public Bitmap GetFrame(double gameTime)
        {
            return textures[(int)(((gameTime % cycleTime) / cycleTime) * textures.Length)];
        }

        public Bitmap[] Textures
        {
            get { return textures; }
            set { textures = value; }
        }
    }
}
