﻿using SharpDX;
using System;

namespace RetroEngine
{
    /// <summary>
    /// The class containing all information necessary for specific rendering.
    /// </summary>
    class Camera : GameObject
    {
        private float fov;
        private Size2 resolution;
        private float nearPlane;
        private float farPlane;
        private Size2F screenSize;

        public Camera(Vector3 pos, float fov, Size2 resolution, float nearPlane, float farPlane)
            : base(pos, null)
        {
            this.fov = fov;
            this.resolution = resolution;
            this.nearPlane = nearPlane;
            this.farPlane = farPlane;
            float WW = (float)Math.Tan(MathUtil.DegreesToRadians(fov / 2F)) * 2F;
            screenSize = new Size2F(WW, WW * resolution.Height / resolution.Width);
        }

        /// <summary>
        /// Gets or sets the horizontal opening angle for the view frustrum.
        /// </summary>
        public float FOV
        {
            get { return fov; }
            set
            {
                float WW = (float)Math.Tan(MathUtil.DegreesToRadians(fov / 2F)) * 2F;
                screenSize = new Size2F(WW, WW * resolution.Height / resolution.Width);
                fov = value;
            }
        }

        /// <summary>
        /// Gets or sets the rendering resolution.
        /// </summary>
        public Size2 Resolution
        {
            get { return resolution; }
            set
            {
                float WW = (float)Math.Tan(MathUtil.DegreesToRadians(fov / 2F)) * 2F;
                screenSize = new Size2F(WW, WW * resolution.Height / resolution.Width);
                resolution = value;
            }
        }

        /// <summary>
        /// Gets or sets the near rendering distance.
        /// </summary>
        public float NearPlane
        {
            get { return nearPlane; }
            set { nearPlane = value; }
        }

        /// <summary>
        /// Gets or sets the far rendering distance.
        /// </summary>
        public float FarPlane
        {
            get { return farPlane; }
            set { farPlane = value; }
        }

        /// <summary>
        /// Gets the screen size in world coordinates. (Read only)
        /// </summary>
        public Size2F ScreenSize
        {
            get { return screenSize; }
        }

        public override void update()
        {
            throw new NotImplementedException();
        }
    }
}
