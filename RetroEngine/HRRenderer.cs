using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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
            DrawFloor(0, scene.Camera, map);

            //Render the roof
            DrawRoof(4, scene.Camera, map);

            //Render the scene
            for (int x = 0; x < scene.Camera.Resolution.Width; x++)
            {
                //Calculate the ray

                Ray ray = ConstructRayThroughPixel(x, scene.Camera);

                //Get the color for the pixel
                List<float> upperYs;
                List<float> lowerYs;
                List<float> distances;
                List<Vector3> collisions;
                List<Wall> walls = GetWalls(ray, scene, map, x, out upperYs, out lowerYs, out distances, out collisions);
                DrawWalls(walls, upperYs, lowerYs, distances, collisions, x, scene);
            }
        }

        private void DrawFloor(float height, Camera cam, HRMap map)
        {
            RectangleF rect = new RectangleF(0, 0, 50, 50);

            for (int y = cam.Resolution.Height - 1; y >= cam.Resolution.Height / 2; y--)
            {
                //Calculate the distance to the bottom
                float sHeight = cam.ScreenSize.Height;
                Vector3 dir = cam.Forward + (-sHeight / 2 + (sHeight / (cam.Resolution.Height - 1)) * y) * new Vector3(0, -1, 0);
                dir.Normalize();
                float distance = Math.Abs((height - cam.Position.Y) / dir.Y);
                float dimmingFactor = distance / cam.FarPlane;
                Brush dimmedBrush = new SolidColorBrush(DXInterface.Context2D, new Color4(0, 0, 0, dimmingFactor));
                //Get the distance to the pivot point of the textures
                //float worldSpaceWidth = -(float)Math.Tan(MathUtil.DegreesToRadians(cam.FOV / 2F)) * 2 *(new Vector2(dir.X, dir.Z) * distance).Length();
                //Vector3 collision = cam.Position + dir * distance;
                //Vector3 leftBorder = collision + worldSpaceWidth / 2 * Mathf.Left(cam.Forward);
                //Vector3 rightBorder = collision + worldSpaceWidth / 2 * (-Mathf.Left(cam.Forward));
                //Vector2 leftUV = new Vector2(leftBorder.X / rect.Width * map.FloorTexture.PixelSize.Width, leftBorder.Z / rect.Height * map.FloorTexture.PixelSize.Height);
                //Vector2 rightUV = new Vector2(rightBorder.X / rect.Width * map.FloorTexture.PixelSize.Width, rightBorder.Z / rect.Height * map.FloorTexture.PixelSize.Height);
                //float textureWidth = worldSpaceWidth * map.FloorTexture.PixelSize.Width;
                ////Get angle
                //float angle = MathUtil.DegreesToRadians(-(float)Math.Acos(Mathf.Dot(rightUV - leftUV, new Vector2(1, 0)) / ((rightUV - leftUV).Length())));
                //DXInterface.Context2D.DrawBitmap(map.FloorTexture, new RectangleF(0, y, cam.Resolution.Width, 1), 1.0F, BitmapInterpolationMode.NearestNeighbor, new RectangleF(leftUV.X, leftUV.Y, (rightUV - leftUV).Length(), 1));
                DXInterface.Context2D.FillRectangle(new RectangleF(0, y, cam.Resolution.Width, 1), map.FloorBrush);
                DXInterface.Context2D.FillRectangle(new RectangleF(0, y, cam.Resolution.Width, 1), dimmedBrush);
                dimmedBrush.Dispose();
            }
        }

        private void DrawRoof(float height, Camera cam, HRMap map)
        {
            for (int y = 0; y <= cam.Resolution.Height / 2; y++)
            {
                //Calculate the distance to the bottom
                float sHeight = cam.ScreenSize.Height;
                Vector3 dir = cam.Forward + (-sHeight / 2 + (sHeight / (cam.Resolution.Height - 1)) * y) * new Vector3(0, 1, 0);
                dir.Normalize();
                float distance = Math.Abs((height - cam.Position.Y) / dir.Y);
                float dimmingFactor = distance / cam.FarPlane;
                Brush dimmedBrush = new SolidColorBrush(DXInterface.Context2D, new Color4(0, 0, 0, dimmingFactor));
                DXInterface.Context2D.FillRectangle(new RectangleF(0, y, cam.Resolution.Width, 1), map.RoofBrush);
                DXInterface.Context2D.FillRectangle(new RectangleF(0, y, cam.Resolution.Width, 1), dimmedBrush);
                dimmedBrush.Dispose();
            }
        }

        private void DrawWalls(List<Wall> walls, List<float> upperYs, List<float> lowerYs, List<float> distances, List<Vector3> collisions, int x, Scene scene)
        {
            for (int i = 0; i < walls.Count; i++)
            {
                //Dim the texture by distance
                float dimmingFactor = distances[i] / scene.Camera.FarPlane;
                Brush distanceBrush = new SolidColorBrush(DXInterface.Context2D, new Color4(0, 0, 0, dimmingFactor));
                float distanceToStart = (collisions[i] - new Vector3(walls[i].Start.X, scene.Camera.Position.Y, walls[i].Start.Y)).Length();
                //Stretch the texture
                float pixelX = ((distanceToStart / (walls[i].Direction.Length() / walls[i].StretchFactor.Width)) * walls[i].TextureSize.Width) % walls[i].TextureSize.Width;
                if (upperYs[i] < lowerYs[i])
                {
                    DXInterface.Context2D.DrawBitmap(walls[i].Texture, new RectangleF(x, upperYs[i], 1, lowerYs[i] - upperYs[i]), 1.0F, BitmapInterpolationMode.NearestNeighbor, new RectangleF(pixelX, 0, 1, walls[i].TextureSize.Height));
                    DXInterface.Context2D.FillRectangle(new RectangleF(x, upperYs[i], 1, lowerYs[i] - upperYs[i]), distanceBrush);
                }
                else
                {
                    //Draw to rectangles and leave a whole in the middle
                    DXInterface.Context2D.FillRectangle(new RectangleF(x, 0, 1, lowerYs[i]), distanceBrush);
                    DXInterface.Context2D.FillRectangle(new RectangleF(x, upperYs[i], 1, scene.Camera.Resolution.Height), distanceBrush);
                }
                distanceBrush.Dispose();
            }
        }

        private Ray ConstructRayThroughPixel(int pixelX, Camera cam)
        {
            //Get the distance from the middle of the screen to the left
            float leftX = cam.ScreenSize.Width / 2F;

            //Gets the direction of the ray through the given pixel
            Vector3 forward = cam.Forward;
            Vector3 direction = forward + (-leftX + ((leftX * 2) / (cam.Resolution.Width - 1)) * pixelX) * Mathf.Left(forward);
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

        private List<Wall> GetWalls(Ray ray, Scene scene, HRMap map, int currentX, out List<float> upperYs, out List<float> lowerYs, out List<float> distances, out List<Vector3> collisions)
        {
            //Check for collision with each wall
            //Store the walls in the lists sorted by the distance to make rendering easier
            distances = new List<float>();
            List<Wall> closestWalls = new List<Wall>();
            upperYs = new List<float>();
            lowerYs = new List<float>();
            collisions = new List<Vector3>();
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
                            collisions.Insert(i, collision);
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
                        collisions.Add(collision);
                    }
                }
            }
            return closestWalls;
        }
    }
}