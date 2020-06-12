using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShapeFill
{
    public static class ExtensionMethods
    {
        public static Vector ToVector(this System.Drawing.Point p)
        {
            return new Vector(p.X, p.Y);
        }
        public static PointF ToPointF(this Vector v)
        {
            return new PointF((float)v.X, (float)v.Y);
        }
    }

    static class ExtensionMethods2
    {
        public static PointF[] ToPointFArray(this Pointe[] points)
        {
            PointF[] ar = new PointF[points.Length];
            for (int i = 0; i < points.Length; i++)
                ar[i] = points[i].pos.ToPointF();
            return ar;
        }
    }
}
