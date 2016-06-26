using SharpDX;
using System.Collections.Generic;
using SharpDX.Direct2D1;

namespace RetroEngine
{
    class HRScene : Scene
    {
        public override void Load()
        {
            HRParser parser = new HRParser();
            parser.Load(fileName);
            parser.Parse();
            map = parser.Map;
            cam = new Camera(new Vector3(parser.StartPosition.X, 0.1F, parser.StartPosition.Y), 70, GameConstants.Context2D.PixelSize, 0, 15F);
            cam.Rotation = new Vector3(0, parser.StartRotation, 0);
            player = new Player(new Vector3(parser.StartPosition.X, 2f, parser.StartPosition.Y), cam, this);
            player.Time = GameConstants.Time;
            player.Rotation = cam.Rotation;
            sprites = new List<Sprite>();
            sprites.Add(new NPC(new Vector3(10, 1, 10), new Animation(new Bitmap[] { GameConstants.TextureManager.Textures["errorimage.jpg"]}, 1), (HRMap)map, 1, new List<Vector2>()));
        }
    }
}