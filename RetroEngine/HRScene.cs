using SharpDX;

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
            cam = new Camera(new Vector3(parser.StartPosition.X, 2F, parser.StartPosition.Y), 70, GameConstants.Context2D.PixelSize, 0, 50F);
            cam.Rotation = new Vector3(0, parser.StartRotation, 0);
            player = new Player(new Vector3(parser.StartPosition.X, 2f, parser.StartPosition.Y), cam);
            player.Rotation = cam.Rotation;
        }
    }
}