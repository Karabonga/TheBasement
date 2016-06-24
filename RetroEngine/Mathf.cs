using SharpDX;
using System;

namespace RetroEngine
{
    static class Mathf
    {
        /// <summary>
        /// Rotates a vector around the y axis by the given degrees.
        /// </summary>
        /// <param name="v">The vector to be rotated.</param>
        /// <param name="degrees">The degrees the vector is being rotated.</param>
        /// <returns>Returns the rotated vector.</returns>
        public static Vector3 RotateAroundY(Vector3 v, float degrees)
        {
            degrees = MathUtil.DegreesToRadians(degrees);
            float x = (float)(Math.Cos(degrees) * v.X + Math.Sin(degrees) * v.Z);
            float z = (float)(-Math.Sin(degrees) * v.X + Math.Cos(degrees) * v.Z);
            return new Vector3(x, v.Y, z);
        }

        /// <summary>
        /// Gets the left vector to the given vector assuming the up vector is (0, 1, 0).
        /// </summary>
        /// <param name="v">The vector to calculate the left vector of.</param>
        /// <returns>Return the left vector of a given vector.</returns>
        public static Vector3 Left(Vector3 v)
        {
            Vector3 res = new Vector3(v.Z, 0, -v.X);
            res.Normalize();
            return res;
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>Returns the dot product.</returns>
        public static float Dot(Vector3 v1, Vector3 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }
    }
}
