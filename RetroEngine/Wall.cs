using SharpDX;

namespace RetroEngine
{
    class Wall : GameObject
    {
        private Vector2 start;
        private Vector2 end;
        private float bottom;
        private float top;

        public Wall(Vector2 start, Vector2 end)
            : base(new Vector3(start.X + 0.5F * (end.X - start.X), 0.5F, start.Y + 0.5F * (end.Y - start.Y)))
        {
            bottom = 0;
            top = 1;
            this.start = start;
            this.end = end;
        }

        /// <summary>
        /// Gets or sets the start position of the wall.
        /// </summary>
        public Vector2 Start
        {
            get { return start; }
            set { start = value; }
        }

        /// <summary>
        /// Gets or sets the end position of the wall.
        /// </summary>
        public Vector2 End
        {
            get { return end; }
            set { end = value; }
        }

        /// <summary>
        /// Gets or sets the bottom of the wall.
        /// </summary>
        public float Bottom
        {
            get { return bottom; }
            set { bottom = value; }
        }

        /// <summary>
        /// Gets or sets the top of the wall.
        /// </summary>
        public float Top
        {
            get { return top; }
            set { top = value; }
        }

        /// <summary>
        /// Gets the normal of the wall. (Read only)
        /// </summary>
        public Vector2 Normal
        {
            get
            {
                Vector2 dir = Direction;
                return new Vector2(-dir.Y, dir.X);
            }
        }

        /// <summary>
        /// Gets the direction of the wall from start to end. (Read only)
        /// </summary>
        public Vector2 Direction
        {
            get { return end - start; }
        }

        /// <summary>
        /// Gets the length of the wall. (Read only)
        /// </summary>
        public float Length
        {
            get { return Direction.Length(); }
        }
    }
}
