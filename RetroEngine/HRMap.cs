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
        private Brush floorBrush;

        public HRMap(Size2 mapSize)
            : base(mapSize)
        {
            walls = new List<Wall>();
            planes = new List<Plane>();
            floorBrush = new SolidColorBrush(GameConstants.Context2D, new Color4(Color.DeepPink.R / 255F, Color.DeepPink.G / 255F, Color.DeepPink.B / 255F, 1));
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
        /// Gets or sets the floor brush.
        /// </summary>
        public Brush FloorBrush
        {
            get { return floorBrush; }
            set { floorBrush = value; }
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
                }
                disposedValue = true;
            }
        }
        #endregion
    }
}