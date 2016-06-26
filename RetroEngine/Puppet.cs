using System;
using SharpDX;

namespace RetroEngine
{
    class Puppet : Sprite
    {
        public Puppet(Vector3 pos)
            :base(GameConstants.TextureManager.Textures["PuppetSlave.png"], pos)
        {
            Scale = new Vector3(2, 2, 0);
        }

        public override void update()
        {
            
        }
    }
}
