using System;

namespace RetroEngine
{
    //The states of a debug log
    public enum MessageState
    {
        Debug,
        Info,
        Warning,
        Error
    }

    /// <summary>
    /// A static class providing all methods necessary for debugging.
    /// </summary>
    static class Debug
    {
        private static double lastRefresh = 0;
        private static double refreshRate = 0.2;
        private static int framesPast = 0;
        private static double averageDeltaTime = 0;

        /// <summary>
        /// Prints a debug message in the console.
        /// </summary>
        /// <param name="text">The message to be printed.</param>
        public static void Log(string text)
        {
            if (Console.CursorTop == 0)
                Console.CursorTop = 1;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("DEBUG >> " + text);
        }

        /// <summary>
        /// Prints a message in the console with the given <see cref="MessageState"/>.
        /// </summary>
        /// <param name="text">The message to be printed.</param>
        /// <param name="messageState">The <see cref="MessageState"/> of the message.</param>
        public static void Log(string text, MessageState messageState)
        {
            if (Console.CursorTop == 0)
                Console.CursorTop = 1;
            if (messageState == MessageState.Debug)
                Log(text);
            else if (messageState == MessageState.Info)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("INFO");
                LogStateless(text);
            }
            else if (messageState == MessageState.Warning)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("WARNING");
                LogStateless(text);
            }
            else if (messageState == MessageState.Error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("ERROR");
                LogStateless(text);
            }
        }

        private static void LogStateless(string text)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" >> " + text);
        }

        /// <summary>
        /// Refreshes the benchmark information in the console.
        /// </summary>
        /// <param name="deltaTime">The time elapsed during the last frame.</param>
        /// <param name="gameTime">The overall time elapsed.</param>
        public static void Refresh(double deltaTime, double gameTime)
        {
            try
            {
                //Count frame
                framesPast++;
                //If it's time, refresh the framerate
                if (gameTime - lastRefresh >= refreshRate)
                {
                    averageDeltaTime = (gameTime - lastRefresh) / framesPast;
                    framesPast = 0;
                    lastRefresh = gameTime;
                }

                //Get current cursor position
                int left = Console.CursorLeft;
                int top = Console.CursorTop;

                //Set the current cursor position to upper left corner
                Console.CursorLeft = 0;
                Console.CursorTop = Math.Max(top - Console.WindowHeight + 1, 0);

                //Set the background color to blue
                Console.BackgroundColor = ConsoleColor.Blue;

                //Write the framerate and the elapsed time
                double framerate = 1 / averageDeltaTime;
                Console.Write("Framerate: " + framerate.ToString("N2") + " FPS (" + (averageDeltaTime * 1000).ToString("N2") + " ms) Time: " + gameTime.ToString("N2") + " s");

                //Reset the background color
                Console.BackgroundColor = ConsoleColor.Black;

                //Set the cursor back to its last position
                Console.CursorLeft = left;
                Console.CursorTop = top;
            }
            catch (Exception ex)
            {
                //Send a message to prevent the programmer from messing up the console buffer
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("STOP MESSING AROUND WITH ME!\nYou just caused a " + ex.GetType().ToString() + "!\n");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}
