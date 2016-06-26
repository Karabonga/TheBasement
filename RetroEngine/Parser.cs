using SharpDX;
using System.Collections.Generic;

namespace RetroEngine
{
    abstract class Parser
    {
        protected string[] data;
        protected Vector2 startPos;
        protected float playerRotation;
        protected Map map;
        protected string fileName;
        protected List<Sprite> sprites;

        /// <summary>
        /// Parses the file data writes the information into a map object.
        /// </summary>
        public abstract void Parse();
        /// <summary>
        /// Loads a scene file.
        /// </summary>
        /// <param name="fileName">The scene file.</param>
        public abstract void Load(string fileName);

        /// <summary>
        /// Gets the start position of the camera. (Read only)
        /// </summary>
        public Vector2 StartPosition
        {
            get { return startPos; }
        }

        /// <summary>
        /// Gets the start rotation of the camera. (Read only)
        /// </summary>
        public float StartRotation
        {
            get { return playerRotation; }
        }

        /// <summary>
        /// Gets the map of the scene. (Read only)
        /// </summary>
        public Map Map
        {
            get { return map; }
        }

        public List<Sprite> Sprites
        {
            get { return sprites; }
        }
    }
}
