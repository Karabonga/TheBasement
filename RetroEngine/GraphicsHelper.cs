using SharpDX;
using System;

namespace RetroEngine
{
    static class GraphicsHelper
    {
        public static Matrix ProjectionMatrix(Vector3 p, float angle)
        {
            angle = MathUtil.DegreesToRadians(angle);
            Matrix mat = new Matrix();
            mat.M11 = (float)(Math.Pow(Math.Cos(angle), 2) + p.X * Math.Sin(-angle));
            mat.M12 = 0;
            mat.M13 = (float)(Math.Sin(-angle) * Math.Cos(angle) - p.X * Math.Cos(angle));
            mat.M14 = (float)(p.X * (Math.Pow(Math.Cos(angle), 2) + p.X * Math.Sin(-angle)) + p.Z * (Math.Sin(-angle) * Math.Cos(angle) - p.X * Math.Cos(angle)) - p.X);
            mat.M21 = (float)(p.Y * Math.Sin(-angle));
            mat.M22 = 1;
            mat.M23 = (float)(-p.Y * Math.Cos(angle));
            mat.M24 = (float)(p.X * p.Y * Math.Sin(-angle) - p.Z * p.Y * Math.Cos(angle));
            mat.M31 = (float)(-Math.Sin(angle) * Math.Cos(-angle) + p.Z * Math.Sin(-angle));
            mat.M32 = 0;
            mat.M33 = (float)(Math.Pow(Math.Sin(angle), 2) - p.Z * Math.Cos(angle));
            mat.M34 = (float)(p.X * (-Math.Sin(angle) * Math.Cos(-angle) + p.Z * Math.Sin(-angle)) + p.Z * (Math.Pow(Math.Sin(angle), 2) - p.Z * Math.Cos(angle)) - p.Z);
            mat.M41 = (float)(-Math.Sin(-angle));
            mat.M42 = 0;
            mat.M43 = (float)(Math.Cos(angle));
            mat.M44 = (float)(p.X * (-Math.Sin(-angle)) + p.Z * Math.Cos(angle) + 1);
            return mat;
        }
    }
}
