using System;
using SharpDX;

namespace RetroEngine
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            //Create the SharpDX interface
            DXInterface dxInterface = new DXInterface("RetroEngine - Demo");
            //Create a new horizontal raytracing renderer from the SharpDX interface
            Renderer renderer = new HRRenderer(dxInterface);
            //Create a new game
            Game game = new Game(renderer);
            //Create a new scene
            Scene scene1 = game.CreateScene(Color.CornflowerBlue, "DemoScene.hrs");
            //Load the scene into the game
            game.LoadScene(scene1);
            //Start the game
            game.Run();
        }
    }
}
