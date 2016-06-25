using SharpDX;
using System;
using System.Collections.Generic;

namespace RetroEngine
{
    abstract class Scene : IDisposable
    {
        protected Color4 backgroundColor;
        protected string fileName;
        protected Map map;
        protected Camera cam;
        protected List<NPC> npcs;
        protected Player player;

        public abstract void Load();

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    map.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        /// <summary>
        /// Gets or sets the color that paints the background of the scene.
        /// </summary>
        public Color4 BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        /// <summary>
        /// Gets the map of this scene. (Read only)
        /// </summary>
        public Map Map
        {
            get { return map; }
        }

        /// <summary>
        /// Gets or sets the name of the file containing the scene data.
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// Gets or sets the camera of the scene.
        /// </summary>
        public Camera Camera
        {
            get { return cam; }
            set { cam = value; }
        }
        public Player Player
        {
            get { return player; }
        }

        public List<NPC> NPCs
        {
            get { return npcs; }
            set { npcs = value; }
        }
    }
}
