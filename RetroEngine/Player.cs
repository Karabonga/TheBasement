using System;
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
        private Scene scene;
        public Player(Vector3 start, Camera cam, Scene scene)
            : base(start, null)
        {
            this.startposition = start;
            this.position = start;
            this.movespeed = 0;
            this.rotationspeed = 0;
            this.playercam = cam;
            this.scene = scene;
        }
        public void handleInput(Input input)
        {
            // acceleration
            if (input.GetKeyDown("MoveForward"))
                movespeed = 10;

            if (input.GetKeyDown("MoveBackward"))
                movespeed = -10;

            if (input.GetKeyDown("TurnLeft"))
                rotationspeed = -200;

            if (input.GetKeyDown("TurnRight"))
                rotationspeed = 200;

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
            Debug.Log(position.ToString());
        }
        private void collisionDetection()
        {
            HRMap map = (HRMap)scene.Map;

            foreach (Wall w in map.Walls)
            {
                Vector2 delta = new Vector2(position.X, position.Z) - w.Start;
                Vector2 delta2 = new Vector2(position.X, position.Z) - w.End;
                if (delta.Length() + delta2.Length() - w.Length < 0.3F)
                {
                    float angle = (float)Math.Acos(Mathf.Dot(delta, w.Direction) / (delta.Length() * w.Direction.Length()));
                    if (MathUtil.RadiansToDegrees(angle) > 90)
                        continue;
                    float distance = (float)Math.Abs(Math.Tan(angle) * delta.Length());
                    if (distance < 0.4F)
                    {
                        Vector3 normal = new Vector3(w.Normal.X, 0, w.Normal.Y);
                        normal.Normalize();
                        for (int i = 0; i < 10; i++)
                        {
                            delta = new Vector2(position.X, position.Z) - w.Start;
                            angle = (float)Math.Acos(Mathf.Dot(delta, w.Direction) / (delta.Length() * w.Direction.Length()));
                            distance = (float)Math.Abs(Math.Tan(angle) * delta.Length());
                            if (distance < 0.4F)
                            {
                                Vector3 dir1 = normal * (0.4F - distance) * (float)GameConstants.Time.DeltaTime * 60F;
                                Vector3 dir2 = -dir1;
                                Vector2 tmpdelta = new Vector2(position.X + dir1.X, position.Z + dir1.Z) - w.Start;
                                float tmpangle = (float)Math.Acos(Mathf.Dot(tmpdelta, w.Direction) / (tmpdelta.Length() * w.Direction.Length()));
                                float tmpdistance = (float)Math.Abs(Math.Tan(tmpangle) * delta.Length());
                                Vector2 tmpdelta2 = new Vector2(position.X + dir2.X, position.Z + dir2.Z) - w.Start;
                                float tmpangle2 = (float)Math.Acos(Mathf.Dot(tmpdelta2, w.Direction) / (tmpdelta2.Length() * w.Direction.Length()));
                                float tmpdistance2 = (float)Math.Abs(Math.Tan(tmpangle2) * delta.Length());
                                //Debug.Log(dir1.ToString() + " and " dir2.ToString());
                                if (tmpdistance > tmpdistance2)
                                    position += dir1;
                                else
                                    position += dir2;
                            }
                            else
                                break;
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
