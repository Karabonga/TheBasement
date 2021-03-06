﻿using System;
using System.IO;
using SharpDX;
using System.Collections.Generic;

namespace RetroEngine
{
    class HRParser : Parser
    {
        public override void Parse()
        {
            try
            {
                sprites = new List<Sprite>();
                //Get the player position
                string[] playerCoords = data[0].Split(new char[] { ';' });
                startPos = new Vector2();
                if (!float.TryParse(playerCoords[0], out startPos.X))
                    throw new Exception("Couldn't parse the x coordinate of the player position in file '" + fileName + "'.");
                if (!float.TryParse(playerCoords[1], out startPos.Y))
                    throw new Exception("Couldn't parse the y coordinate of the player position in file '" + fileName + "'.");
                Debug.Log("Parser: player position " + startPos.ToString());

                //Get the player rotation
                if (!float.TryParse(data[1], out playerRotation))
                    throw new Exception("Couldn't parse the rotation of the player in file '" + fileName + "'.");
                Debug.Log("Parser: player rotation " + playerRotation.ToString() + "°");

                //Get the map data
                //Get the map dimension
                string[] mapDim = data[2].Split(new char[] { ';' });
                Size2 mapSize = new Size2();
                if (!int.TryParse(mapDim[0], out mapSize.Width))
                    throw new Exception("Couldn't parse the width of the map in file '" + fileName + "'.");
                if (!int.TryParse(mapDim[1], out mapSize.Height))
                    throw new Exception("Couldn't parse the height of the map in file '" + fileName + "'.");
                Debug.Log("Parser: map size " + mapSize.ToString());
                //Get the floor
                string[] floorData = data[3].Split(new char[] { ';' });
                float floorHeight = 0;
                if (!float.TryParse(floorData[0], out floorHeight))
                    throw new Exception("Couldn't parse the height of the floor in file '" + fileName + "'.");
                //Get the roof
                string[] roofData = data[4].Split(new char[] { ';' });
                float roofHeight = 0;
                if (!float.TryParse(roofData[0], out roofHeight))
                    throw new Exception("Couldn't parse the height of the roof in file '" + fileName + "'.");
                map = new HRMap(mapSize, floorHeight, new Color(52, 39, 28), roofHeight, new Color(50, 50, 50));
                //Get the map walls
                int wallsEnd = 0;
                for (int y = 5; data[y] != "-" && y < data.Length; y++)
                {
                    wallsEnd = y;
                    string[] tmp = data[y].Split(new char[] { ';' });
                    Vector2 start = new Vector2();
                    Vector2 end = new Vector2();
                    float bottom = 0;
                    float top = 0;
                    if (!float.TryParse(tmp[0], out start.X))
                        throw new Exception("Couldn't parse float at " + (y + 1) + ", 1 in file '" + fileName + "'.");
                    if (!float.TryParse(tmp[1], out start.Y))
                        throw new Exception("Couldn't parse float at " + (y + 1) + ", 2 in file '" + fileName + "'.");
                    if (!float.TryParse(tmp[2], out end.X))
                        throw new Exception("Couldn't parse float at " + (y + 1) + ", 3 in file '" + fileName + "'.");
                    if (!float.TryParse(tmp[3], out end.Y))
                        throw new Exception("Couldn't parse float at " + (y + 1) + ", 4 in file '" + fileName + "'.");
                    if (!float.TryParse(tmp[4], out bottom))
                        throw new Exception("Couldn't parse float at " + (y + 1) + ", 5 in file '" + fileName + "'.");
                    if (!float.TryParse(tmp[5], out top))
                        throw new Exception("Couldn't parse float at " + (y + 1) + ", 6 in file '" + fileName + "'.");
                    Wall wall = new Wall(start, end, top, bottom, GameConstants.TextureManager.Textures[tmp[6]]);
                    if (tmp[6] == "door.jpg")
                        wall.StretchFactor = new Size2F(1, 1);
                    ((HRMap)map).AddWall(wall);
                    Debug.Log("Parser: Added wall from " + wall.Start.ToString() + " to " + wall.End.ToString());
                }

                ////Get the map planes
                //for (int y = wallsEnd + 2; y < data.Length; y++)
                //{
                //    string[] tmp = data[y].Split(new char[] { ';' });
                //    Vector2 v1 = new Vector2();
                //    Vector2 v2 = new Vector2();
                //    Vector2 v3 = new Vector2();
                //    Vector2 v4 = new Vector2();
                //    float height = 0;
                //    if (!float.TryParse(tmp[0], out v1.X))
                //        throw new Exception("Couldn't parse float at " + (y + 1) + ", 1 in file '" + fileName + "'.");
                //    if (!float.TryParse(tmp[1], out v1.Y))
                //        throw new Exception("Couldn't parse float at " + (y + 1) + ", 2 in file '" + fileName + "'.");
                //    if (!float.TryParse(tmp[2], out v2.X))
                //        throw new Exception("Couldn't parse float at " + (y + 1) + ", 3 in file '" + fileName + "'.");
                //    if (!float.TryParse(tmp[3], out v2.Y))
                //        throw new Exception("Couldn't parse float at " + (y + 1) + ", 4 in file '" + fileName + "'.");
                //    if (!float.TryParse(tmp[4], out v3.X))
                //        throw new Exception("Couldn't parse float at " + (y + 1) + ", 5 in file '" + fileName + "'.");
                //    if (!float.TryParse(tmp[5], out v3.Y))
                //        throw new Exception("Couldn't parse float at " + (y + 1) + ", 6 in file '" + fileName + "'.");
                //    if (!float.TryParse(tmp[6], out v4.X))
                //        throw new Exception("Couldn't parse float at " + (y + 1) + ", 7 in file '" + fileName + "'.");
                //    if (!float.TryParse(tmp[7], out v4.Y))
                //        throw new Exception("Couldn't parse float at " + (y + 1) + ", 8 in file '" + fileName + "'.");
                //    if (!float.TryParse(tmp[8], out height))
                //        throw new Exception("Couldn't parse float at " + (y + 1) + ", 9 in file '" + fileName + "'.");
                //    Plane plane = new Plane(v1, v2, v3, v4, height);
                //    ((HRMap)map).AddPlane(plane);
                //    Debug.Log("Parser: Added plane on height " + plane.Height.ToString());
                //}

                //Get the sprites
                for (int y = wallsEnd + 2; y < data.Length; y++)
                {
                    string[] tmp = data[y].Split(new char[] { ';' });
                    Vector3 pos = new Vector3();
                    if (tmp[0] == "puppet")
                    {
                        if (!float.TryParse(tmp[1], out pos.X))
                            throw new Exception("Couldn't parse float at " + (y + 1) + ", 2 in file '" + fileName + "'.");
                        if (!float.TryParse(tmp[2], out pos.Z))
                            throw new Exception("Couldn't parse float at " + (y + 1) + ", 3 in file '" + fileName + "'.");
                        if (!float.TryParse(tmp[3], out pos.Y))
                            throw new Exception("Couldn't parse float at " + (y + 1) + ", 4 in file '" + fileName + "'.");
                        sprites.Add(new Puppet(pos));
                    }
                    else if (tmp[0] == "key")
                    {

                        if (!float.TryParse(tmp[1], out pos.X))
                            throw new Exception("Couldn't parse float at " + (y + 1) + ", 2 in file '" + fileName + "'.");
                        if (!float.TryParse(tmp[2], out pos.Z))
                            throw new Exception("Couldn't parse float at " + (y + 1) + ", 3 in file '" + fileName + "'.");
                        if (!float.TryParse(tmp[3], out pos.Y))
                            throw new Exception("Couldn't parse float at " + (y + 1) + ", 4 in file '" + fileName + "'.");
                        sprites.Add(new Key(pos));
                    }
                }

            }
            catch (Exception ex)
            {
                throw new SceneParserException("Couldn't parse the scene file '" + fileName + "'.", ex);
            }
        }

        public override void Load(string fileName)
        {
            //Loading the scene file
            data = File.ReadAllLines(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\scenes\\" + fileName);
            //Save the file name
            this.fileName = fileName;
        }
    }
}
