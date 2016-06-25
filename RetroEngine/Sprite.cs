using SharpDX;
using SharpDX.Direct2D1;

namespace RetroEngine
{
    abstract class Sprite : GameObject
    {
        private Animation animation;

        public Sprite(Animation animation, Vector3 pos)
            :base(pos, animation.Textures[0])
        {
            this.animation = animation;
        }

        public Sprite(Bitmap bmp, Vector3 pos)
            :base(pos, bmp)
        {
            animation = new Animation(new Bitmap[] { bmp }, 1);
        }

        public void Draw(Vector2 position, Size2F size)
        {
            GameConstants.Context2D.DrawBitmap(animation.GetFrame(GameConstants.Time.GameTime), new RectangleF(position.X, position.Y, size.Width, size.Height), 1.0F, BitmapInterpolationMode.NearestNeighbor);
        }
    }
}
