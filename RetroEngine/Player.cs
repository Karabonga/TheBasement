using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

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
        public Player(Vector3 start, Camera cam, Time time)
            :base(start,null)
        {
            this.startposition = start;
            this.position = start;
            this.movespeed = 10f;
            this.rotationspeed = 10f;
            this.playercam = cam;
            this.time = time;


        }
        void handleInput(Input input)
        {
            if (input.GetKeyDown("MoveBackward"))
                this.movespeed = -movespeed;
            if (input.GetKeyDown("TurnLeft"))
                this.rotationspeed = -rotationspeed;
        }
        void move(Input input)
        {
            handleInput(input);
            this.position += this.Forward * (float)(movespeed * time.DeltaTime);
            this.rotation = new Vector3(this.Rotation.X, (float)(this.Rotation.Y + rotationspeed * time.DeltaTime), this.Rotation.Z);
            playercam.Position = position;
            playercam.Rotation = rotation;
        }
        void update(double gameTime)
        {

        }
        public Vector3 StartPos
        {
            get { return startposition; }
            set { startposition = value; }
        }
        public float Speed
        {
            get { return movespeed; }
            set{ movespeed = value; }
        }
    }
}
