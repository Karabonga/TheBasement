using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using System.Drawing;

namespace RetroEngine
{
    class Player : GameObject
    {
        private Vector3 startposition;
        private Vector3 rotation;
        private Vector3 position;
        private float movespeed;
        private float rotationspeed;
        private Camera playercam;
        private Time time;
        private Scene scene;
        private float collisionradius;
        public Player(Vector3 start, Camera cam)
            : base(start, null)
        {
            this.startposition = start;
            this.position = start;
            this.movespeed = 0;
            this.rotationspeed = 0;
            this.playercam = cam;
        }
        public void handleInput(Input input)
        {
            // acceleration
            if (input.GetKeyDown("MoveForward"))
                movespeed = 10;

            if (input.GetKeyDown("MoveBackward"))
                movespeed = -10;

            if (input.GetKeyDown("TurnLeft"))
                rotationspeed = -100;

            if (input.GetKeyDown("TurnRight"))
                rotationspeed = 100;

        }

        private void move()
        {
            collisionDetection();
            Vector3 forward = Mathf.RotateAroundY(new Vector3(0, 0, 1), rotation.Y);
            Vector3 oldPos = position;
            position += forward * (float)(movespeed * time.DeltaTime);
            movespeed = 0;
            rotation = new Vector3(rotation.X, (float)(rotation.Y + rotationspeed * time.DeltaTime), rotation.Z);
            rotationspeed = 0;
            playercam.Position = position;
            playercam.Rotation = rotation;
        }
        private void collisionDetection()
        {
            HRMap map = (HRMap)scene.Map;
            
            foreach(Wall w in map.Walls)
            {
                float distance = (new Vector2(position.X, position.Z) - w.Start).Length() + (new Vector2(position.X, position.Z) - w.End).Length();
                float delta = Math.Abs(distance - w.Length);
                if(delta < 0.2f)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        distance = (new Vector2(position.X, position.Z) - w.Start).Length() + (new Vector2(position.X, position.Z) - w.End).Length();
                        delta = Math.Abs(distance - w.Length);
                        if (delta < 0.15f)
                        {
                            Vector3 normal = new Vector3(w.Normal.X, 0, w.Normal.Y);
                            normal.Normalize();
                            position += normal * (0.15f - delta);
                        }
                    }
                }
            }
        }

        public override void update()
        {
            move();

        }
        public Vector3 StartPos
        {
            get { return startposition; }
            set { startposition = value; }
        }
        public float MoveSpeed
        {
            get { return movespeed; }
            set { movespeed = value; }
        }
        public float RotationSpeed
        {
            get { return rotationspeed; }
            set { rotationspeed = value; }
        }
        public Camera PlayerCam
        {
            get { return playercam; }
            set { playercam = value; }
        }
        public Time Time
        {
            get { return time; }
            set { time = value; }
        }
        public Scene Scene
        {
            get { return scene; }
            set { scene = value; }
        }
    }
}
