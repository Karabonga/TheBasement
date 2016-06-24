using System.Diagnostics;

namespace RetroEngine
{
    /// <summary>
    /// A class for game related accurate time measurement.
    /// </summary>
    class Time
    {
        private Stopwatch timer;
        private double deltaTime;
        private double lastTime;

        public Time()
        {
            timer = new Stopwatch();
        }

        /// <summary>
        /// The overall time elapsed within the game.
        /// </summary>
        public double GameTime
        {
            get { return (double)timer.ElapsedTicks / Stopwatch.Frequency; }
        }

        /// <summary>
        /// The time elapsed within the last frame.
        /// </summary>
        public double DeltaTime
        {
            get { return deltaTime; }
        }

        /// <summary>
        /// Start or resumes the time measurements.
        /// </summary>
        public void Start()
        {
            timer.Start();
        }

        /// <summary>
        /// Stops the time measurement and resets the elapsed time to zero.
        /// </summary>
        public void Reset()
        {
            timer.Reset();
        }

        /// <summary>
        /// Stops the time measurement.
        /// </summary>
        public void Stop()
        {
            timer.Stop();
        }

        /// <summary>
        /// Tells the timer to measure the time elapsed for the last frame.
        /// </summary>
        public void EndFrame()
        {
            //Calculate the time elapsed for the last frame
            deltaTime = GameTime - lastTime;
            //Start the time measure from the next frame at this time
            lastTime = GameTime;
        }
    }
}
