using SharpDX;

namespace RetroEngine
{
    class Plane : GameObject
    {
        private Vector2[] corners;
        private float height;

        public Plane(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, float height)
            :base(new Vector3((v1.X + v2.X + v3.X + v4.X) / 4, height, (v1.Y + v2.Y + v3.Y + v4.Y) / 4), null)
        {
            corners = new Vector2[4];
            corners[0] = v1;
            corners[1] = v2;
            corners[2] = v3;
            corners[3] = v4;
            this.height = height;
        }

        /// <summary>
        /// Gets or sets the corners of the plane.
        /// </summary>
        public Vector2[] Corners
        {
            get { return corners; }
            set { corners = value; }
        }

        /// <summary>
        /// Gets or sets the height of the plane.
        /// </summary>
        public float Height
        {
            get { return height; }
            set { height = value; }
        }
    }
}
