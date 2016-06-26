using System;
using SharpDX;

namespace RetroEngine
{
    class Key : Sprite
    {
        public Key(Vector3 pos)
            :base(GameConstants.TextureManager.Textures["key2.png"], pos)
        {

        }

        public override void update()
        {

        }
    }
}
