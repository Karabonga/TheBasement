using SharpDX.DXGI;
using SharpDX.Windows;
using System;
using SharpDX;
using System.Windows.Forms;

namespace RetroEngine
{
    /// <summary>
    /// A class containing all game information and logic.
    /// </summary>
    class Game : IDisposable
    {
        Time time;
        Scene scene;
        Renderer renderer;
        Input input;
        TextureManager textureMan;
        Player player;

        public Game(Renderer renderer)
        {
            this.renderer = renderer;
            //Create texture manager
            textureMan = new TextureManager();
            //Fill in the missing game constants
            GameConstants.Initialize(renderer.DXInterface.Context2D, textureMan, time);
            //Load textures
            textureMan.LoadFolder("textures");
            //Initialize new time measurement
            time = new Time();
            //Add some input options
            renderer.DXInterface.RenderForm.KeyDown += KeyDown;
            renderer.DXInterface.RenderForm.KeyUp += KeyUp;
            //Create a new input wrapper
            input = new Input();
            input.RegisterInput("TurnLeft", Keys.A);
            input.RegisterInput("TurnRight", Keys.D);
            input.RegisterInput("MoveForward", Keys.W);
            input.RegisterInput("MoveBackward", Keys.S);
        }

        private void KeyUp(object sender, KeyEventArgs e)
        {
            //Sets the value of the key to false.
            input.SetKey(e.KeyCode, false);
        }

        private void KeyDown(object sender, KeyEventArgs e)
        {
            //Sets the value of the key to true.
            input.SetKey(e.KeyCode, true);
        }

        /// <summary>
        /// Loads a given <see cref="Scene"/>.
        /// </summary>
        /// <param name="scene">The <see cref="Scene"/> to be loaded.</param>
        public void LoadScene(Scene scene)
        {
            //Let the programmer know what is happening
            Debug.Log("Loading scene...", MessageState.Info);
            try
            {
                //Check if scene is initialized by an object of the game class
                if (IsValidScene(scene))
                {
                    //The scene is valid
                    //Continue loading the scene and its assets
                    this.scene = scene;
                    this.scene.Load();
                    player = scene.Player;
                    player.Scene = scene;
                }
                else
                    //The scene is not valid
                    throw new InvalidSceneException("InvalidSceneException: The used scene object is invalid! Try initializing with 'Game.CreateScene()'.");
            }
            catch (Exception ex)
            {
                //Log the error that occured while loading the scene
                string errorMessage = "Couldn't load current scene!";
                //Log the inner errors iteratively
                while(ex != null)
                {
                    errorMessage += " " + ex.Message;
                    ex = ex.InnerException;
                }
                Debug.Log(errorMessage, MessageState.Error);
            }
        }

        private bool IsValidScene(Scene scene)
        {
            return scene.BackgroundColor.Alpha == 1;
        }

        /// <summary>
        /// Starts the game and its render loop.
        /// </summary>
        public void Run()
        {
            //Start new time measurement
            time.Start();
            player.Time = time;
            Debug.Log("Running the game...", MessageState.Info);
            //Run the render loop
            RenderLoop.Run(renderer.DXInterface.RenderForm, GameLoop);

        }

        private void GameLoop()
        {
            //Get inputs
            Input();

            //Do the game logic
            Update(time.GameTime);

            //Do the rendering
            Render();

            //Benchmark the time for this frame
            time.EndFrame();
        }

        private void Input()
        {
            //if (input.GetKeyDown("MoveForward"))
            //    scene.Camera.Position += scene.Camera.Forward * (float)(10 * time.DeltaTime);
            //if (input.GetKeyDown("MoveBackward"))
            //    scene.Camera.Position -= scene.Camera.Forward * (float)(10 * time.DeltaTime);
            //if (input.GetKeyDown("TurnLeft"))
            //    scene.Camera.Rotation = new Vector3(scene.Camera.Rotation.X, (float)(scene.Camera.Rotation.Y - 50 * time.DeltaTime), scene.Camera.Rotation.Z);
            //if (input.GetKeyDown("TurnRight"))
            //    scene.Camera.Rotation = new Vector3(scene.Camera.Rotation.X, (float)(scene.Camera.Rotation.Y + 50 * time.DeltaTime), scene.Camera.Rotation.Z);
            player.handleInput(input);
        }

        private void Update(double gameTime)
        {
            //Refresh the debug console
            Debug.Refresh(time.DeltaTime, gameTime);
            //Finally update the input
            player.update();
            input.FinalUpdate();
        }

        private void Render()
        {
            renderer.DXInterface.Context2D.BeginDraw();

            //Render the scene
            renderer.RenderScene(scene);

            renderer.DXInterface.Context2D.EndDraw();

            //Swap back- and frontbuffer
            renderer.DXInterface.SwapChain.Present(1, PresentFlags.None);
        }

        /// <summary>
        /// Signals the game to exit the render loop after the next frame.
        /// </summary>
        public void Close()
        {
            //Dispose everything
            Dispose();
            //Close the render form to exit the render loop
            renderer.DXInterface.RenderForm.Close();
        }

        /// <summary>
        /// Creates a new scene.
        /// </summary>
        /// <param name="backgroundColor">The background color of the scene.</param>
        /// <param name="fileName">The name of the file containing the scene data.</param>
        /// <returns>Returns a new scene.</returns>
        public Scene CreateScene(Color backgroundColor, string fileName)
        {
            //Construct a new scene
            Scene scene;
            if (renderer.GetType() == typeof(HRRenderer))
                scene = new HRScene();
            else
                scene = null;
            scene.BackgroundColor = backgroundColor;
            scene.FileName = fileName;
            return scene;
        }

        #region IDisposable Support
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    renderer.Dispose();
                    scene.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
