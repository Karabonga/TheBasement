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
        private static Bitmap map;
        public Player(Vector3 start, Camera cam)
            : base(start, null)
        {
            this.startposition = start;
            this.position = start;
            this.movespeed = 0;
            this.rotationspeed = 0;
            this.playercam = cam;
        }
        private void handleInput(Input input)
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
        private bool isWalkable(Vector3 pos, Bitmap map)
        {
            return !(pos.X < 0 || pos.X >= map.Width || pos.Z < 0 || pos.Z >= map.Height) && map.GetPixel((int)pos.X, (int)pos.Z).B > 0;
        }
        private void move(Input input)
        {
            map = PathFinder.fromHRMap((HRMap)scene.Map, (int)position.Y);
            Vector3 forward = Mathf.RotateAroundY(new Vector3(0, 0, 1), rotation.Y);
            Vector3 oldPos = position;
            handleInput(input);
            position += forward * (float)(movespeed * time.DeltaTime);
                if(!isWalkable(position, map))
                    position = oldPos;
            movespeed = 0;
            rotation = new Vector3(rotation.X, (float)(rotation.Y + rotationspeed * time.DeltaTime), rotation.Z);
            rotationspeed = 0;
            playercam.Position = position;
            playercam.Rotation = rotation;
        }
        public void update(Input input)
        {
            move(input);

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
