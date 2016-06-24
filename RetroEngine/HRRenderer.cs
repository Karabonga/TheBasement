using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;

namespace RetroEngine
{
    /// <summary>
    /// The renderer class for horizontal raytracing.
    /// </summary>
    class HRRenderer : Renderer
    {
        public HRRenderer(DXInterface dxInterface)
            : base(dxInterface)
        {
            //Do some renderer type specific initialization
        }

        public override void RenderScene(Scene scene)
        {
            //Clear the window with the background color of the scene
            DXInterface.Context2D.Clear(scene.BackgroundColor);

            //Get the map
            HRMap map = (HRMap)scene.Map;

            //Render the floor
            DrawFloor();

            //Render the scene
            for (int x = 0; x < scene.Camera.Resolution.Width; x++)
            {
                //Calculate the ray
                Ray ray = ConstructRayThroughPixel(x, scene.Camera);

                //Get the color for the pixel
                List<float> upperYs;
                List<float> lowerYs;
                List<float> distances;
                List<GameObject> drawableObjects = GetWalls(ray, scene, map, x, out upperYs, out lowerYs, out distances);
                //Add the planes in the right order
                SortPlanes(drawableObjects, map, scene);                
                DrawWalls(drawableObjects, upperYs, lowerYs, distances, x, scene);
            }
        }

        private void DrawFloor(float height)
        {

        }

        private void DrawWalls(List<GameObject> walls, List<float> upperYs, List<float> lowerYs, List<float> distances, int x, Scene scene)
        {
            for (int i = 0; i < walls.Count; i++)
            {
                //Apply a little dimming by distance
                float dimmingFactor = distances[i] / scene.Camera.FarPlane;
                Brush dimmedBrush = new SolidColorBrush(DXInterface.Context2D, new Color4(walls[i].Color.Red - dimmingFactor, walls[i].Color.Green - dimmingFactor, walls[i].Color.Blue - dimmingFactor, 1));
                if (upperYs[i] < lowerYs[i])
                    DXInterface.Context2D.FillRectangle(new RectangleF(x, upperYs[i], 1, lowerYs[i] - upperYs[i]), dimmedBrush);
                else
                {
                    //Draw to rectangles and leave a whole in the middle
                    DXInterface.Context2D.FillRectangle(new RectangleF(x, 0, 1, lowerYs[i]), dimmedBrush);
                    DXInterface.Context2D.FillRectangle(new RectangleF(x, upperYs[i], 1, scene.Camera.Resolution.Height), dimmedBrush);
                }
            }
        }

        private Ray ConstructRayThroughPixel(int pixelX, Camera cam)
        {
            //Get the distance from the middle of the screen to the left
            float leftX = cam.ScreenSize.Width / 2F;

            //Gets the direction of the ray through the given pixel
            Vector3 direction = cam.Forward + (-leftX + ((leftX * 2) / (cam.Resolution.Width - 1)) * pixelX) * Mathf.Left(cam.Forward);
            direction.Normalize();

            //Creates a new ray
            return new Ray(cam.Position, direction);
        }

        private bool IntersectsWall(Ray ray, Wall wall, Camera cam, int currentX, out Vector3 collision, out float upperY, out float lowerY)
        {
            //Simplify the collision to 2D
            //Create a plane for the wall
            SharpDX.Plane plane = new SharpDX.Plane(wall.Position, new Vector3(wall.Normal.X, 0, wall.Normal.Y));
            if (ray.Intersects(ref plane, out collision))
            {
                //Get the parameter t from the ray equation
                float t = (collision.X - ray.Position.X) / ray.Direction.X;
                //Check if collision is in front of the camera
                if (t > 0)
                {
                    //Check if collision is on the wall (x and z-axis check)
                    float distanceToStart = (new Vector3(wall.Start.X, collision.Y, wall.Start.Y) - collision).Length();
                    float distanceToEnd = (new Vector3(wall.End.X, collision.Y, wall.End.Y) - collision).Length();
                    if (distanceToStart <= wall.Length && distanceToEnd <= wall.Length)
                    {
                        //Get the limits of the wall on the y-axis
                        float middleY = cam.Resolution.Height / 2F;
                        float distance = (ray.Position - collision).Length();
                        //Calculate the distance from the origin of the camera to the current pixel x-coordinate in the screen plane
                        float distToPlane = (float)Math.Sqrt(Math.Pow(-cam.ScreenSize.Width / 2 + cam.ScreenSize.Width / (cam.Resolution.Width - 1) * currentX, 2) + 1);
                        upperY = middleY - ((wall.Top - cam.Position.Y) / distance) * distToPlane * cam.Resolution.Height / cam.ScreenSize.Height;
                        lowerY = middleY + ((cam.Position.Y - wall.Bottom) / distance) * distToPlane * cam.Resolution.Height / cam.ScreenSize.Height;
                        return true;
                    }
                }
            }
            upperY = 0;
            lowerY = 0;
            return false;
        }

        private List<GameObject> GetWalls(Ray ray, Scene scene, HRMap map, int currentX, out List<float> upperYs, out List<float> lowerYs, out List<float> distances)
        {
            //Check for collision with each wall
            //Store the walls in the lists sorted by the distance to make rendering easier
            distances = new List<float>();
            List<GameObject> closestWalls = new List<GameObject>();
            upperYs = new List<float>();
            lowerYs = new List<float>();
            for (int j = 0; j < map.Walls.Count; j++)
            {
                //Intersect the ray with this wall
                Vector3 collision = new Vector3();
                float tmpUpperY = 0;
                float tmpLowerY = 0;
                if (IntersectsWall(ray, map.Walls[j], scene.Camera, currentX, out collision, out tmpUpperY, out tmpLowerY))
                {
                    float d = (collision - ray.Position).Length();
                    bool inserted = false;
                    //Find the right spot to insert the wall
                    for (int i = 0; i < distances.Count; i++)
                    {
                        if (d > distances[i])
                        {
                            //Found the right place so insert here
                            closestWalls.Insert(i, map.Walls[j]);
                            upperYs.Insert(i, tmpUpperY);
                            lowerYs.Insert(i, tmpLowerY);
                            distances.Insert(i, d);
                            inserted = true;
                            break;
                        }
                    }
                    if (!inserted)
                    {
                        //Not inserted within the loop so it has to be the closest one
                        //Just append wall
                        closestWalls.Add(map.Walls[j]);
                        upperYs.Add(tmpUpperY);
                        lowerYs.Add(tmpLowerY);
                        distances.Add(d);
                    }
                }
            }
            return closestWalls;
        }

        private void SortPlanes(List<GameObject> objects, HRMap map, Scene scene)
        {
            for (int i = 0; i < map.Planes.Count; i++)
            {
                //For each vertex:
                for (int j = 0; j < 4; j++)
                {
                    //Create a ray
                    Ray ray = new Ray(scene.Camera.Position, new Vector3(map.Planes[i].Corners[j].X, map.Planes[i].Height, map.Planes[i].Corners[j].Y) - scene.Camera.Position);

                    //Intersect with every other object already in object buffer
                    for (int k = 0; k < objects.Count; k++)
                    {
                        //if (Intersects(ray, objects))
                        //{

                        //}
                    }
                    //Get the object first in list from the collided objects
                    //Insert plane in front of that object
                }
            }
        }
    }
}