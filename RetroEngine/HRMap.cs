using System.Collections.Generic;
using SharpDX;
using System;
using SharpDX.Direct2D1;

namespace RetroEngine
{
    class HRMap : Map
    {
        private List<Wall> walls;
        private List<Plane> planes;
        private Bitmap floorTexture;
        private float floorHeight;
        private Bitmap roofTexture;
        private float roofHeight;

        public HRMap(Size2 mapSize, float floorHeight, Bitmap floorTexture, float roofHeight, Bitmap roofTexture)
            : base(mapSize)
        {
            walls = new List<Wall>();
            planes = new List<Plane>();
            this.floorHeight = floorHeight;
            this.floorTexture = floorTexture;
            this.roofHeight = roofHeight;
            this.roofTexture = roofTexture;
        }

        /// <summary>
        /// Adds a new wall to the map.
        /// </summary>
        /// <param name="wall">The wall being added to the map.</param>
        public void AddWall(Wall wall)
        {
            walls.Add(wall);
        }

        /// <summary>
        /// Adds a new plane to the map.
        /// </summary>
        /// <param name="plane">The plane being added to the map.</param>
        public void AddPlane(Plane plane)
        {
            planes.Add(plane);
        }

        /// <summary>
        /// Gets or sets the walls in this map.
        /// </summary>
        public List<Wall> Walls
        {
            get { return walls; }
            set { walls = value; }
        }

        /// <summary>
        /// Gets or sets the planes in this map.
        /// </summary>
        public List<Plane> Planes
        {
            get { return planes; }
            set { planes = value; }
        }

        /// <summary>
        /// Gets or sets the height of the floor.
        /// </summary>
        public float FloorHeight
        {
            get { return floorHeight; }
            set { floorHeight = value; }
        }

        /// <summary>
        /// Gets or sets the floor texture.
        /// </summary>
        public Bitmap FloorTexture
        {
            get { return floorTexture; }
            set { floorTexture = value; }
        }

        /// <summary>
        /// Gets or sets the height of the roof.
        /// </summary>
        public float RoofHeight
        {
            get { return roofHeight; }
            set { roofHeight = value; }
        }

        /// <summary>
        /// Gets or sets the roof texture.
        /// </summary>
        public Bitmap RoofTexture
        {
            get { return roofTexture; }
            set { roofTexture = value; }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    for (int i = 0; i < walls.Count; i++)
                        walls[i].Dispose();
                    floorTexture.Dispose();
                    roofTexture.Dispose();
                }
                disposedValue = true;
            }
        }
        #endregion
    }
}