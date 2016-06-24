using SharpDX.Direct2D1;

namespace RetroEngine
{
    /// <summary>
    /// Contains all game constants as well as all objects that have to be accessible by every class.
    /// </summary>
    public static class GameConstants
    {
        private static DeviceContext context2d;

        /// <summary>
        /// Initializes the constants that are specific for each game.
        /// </summary>
        /// <param name="context2d">The Direct2D context which executes all drawing operations.</param>
        public static void Initialize(DeviceContext context2d)
        {
            GameConstants.context2d = context2d;
        }

        /// <summary>
        /// Gets the Direct2D context which executes all drawing operations. (Read only)
        /// </summary>
        public static DeviceContext Context2D
        {
            get { return context2d; }
        }
    }
}
