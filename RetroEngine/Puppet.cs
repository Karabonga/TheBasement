using System;
using SharpDX;
using SharpDX.Direct2D1;

namespace RetroEngine
{
    class Puppet : Sprite
    {
        public Puppet(Vector3 pos)
            :base(new Animation(new Bitmap[] { GameConstants.TextureManager.Textures["PuppetSlave.png"],
            GameConstants.TextureManager.Textures["PuppetSlave.png"],
            GameConstants.TextureManager.Textures["PuppetSlave.png"],
            GameConstants.TextureManager.Textures["PuppetSlave.png"],
            GameConstants.TextureManager.Textures["PuppetSlave.png"],
            GameConstants.TextureManager.Textures["PuppetSlave.png"],
            GameConstants.TextureManager.Textures["PuppetSlave.png"],
            GameConstants.TextureManager.Textures["PuppetSlave.png"],
            GameConstants.TextureManager.Textures["PuppetSlave.png"],
            GameConstants.TextureManager.Textures["PuppetSlave.png"],
            GameConstants.TextureManager.Textures["PuppetSlave.png"],
            GameConstants.TextureManager.Textures["PuppetSlave.png"],
            GameConstants.TextureManager.Textures["PuppetSlave.png"],
            GameConstants.TextureManager.Textures["PuppetSlave.png"],
            GameConstants.TextureManager.Textures["PuppetSlave.png"],
            GameConstants.TextureManager.Textures["PuppetSlave.png"],
            GameConstants.TextureManager.Textures["PuppetSlave.png"],
            GameConstants.TextureManager.Textures["PuppetSlave.png"],
            GameConstants.TextureManager.Textures["PuppetSlave.png"],
            GameConstants.TextureManager.Textures["PuppetSlave2.png"]}, 10), pos)
        {
            Scale = new Vector3(2, 2, 0);
        }

        public override void update()
        {
            
        }
    }
}
