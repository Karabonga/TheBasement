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
        private SolidColorBrush floorBrush;
        private float floorHeight;
        private SolidColorBrush roofBrush;
        private float roofHeight;

        public HRMap(Size2 mapSize, float floorHeight, Color floorBrush, float roofHeight, Color roofBrush)
            : base(mapSize)
        {
            walls = new List<Wall>();
            planes = new List<Plane>();
            this.floorHeight = floorHeight;
            this.floorBrush = new SolidColorBrush(GameConstants.Context2D, floorBrush);
            this.roofHeight = roofHeight;
            this.roofBrush = new SolidColorBrush(GameConstants.Context2D, roofBrush);
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
        public SolidColorBrush FloorBrush
        {
            get { return floorBrush; }
            set { floorBrush = value; }
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
        public SolidColorBrush RoofBrush
        {
            get { return roofBrush; }
            set { roofBrush = value; }
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
                    floorBrush.Dispose();
                    roofBrush.Dispose();
                }
                disposedValue = true;
            }
        }
        #endregion
    }
}