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

        public Bitmap GetTexture()
        {
            return animation.GetFrame(GameConstants.Time.GameTime);
        }

        public void UpdateRotation(Player player)
        {
            //Rotation = new Vector3(0, MathUtil.RadiansToDegrees(Mathf.IntersectionAngle(new Vector2(player.Forward.X, player.Forward.Z), new Vector2(Forward.X, Forward.Z))), 0);
        }
    }
}
