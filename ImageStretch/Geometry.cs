using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageStretch
{
    static class Geometry
    {
        static Point toVec(Point a, Point b)
        {
            return new Point(b.X - a.X, b.Y - a.Y);
        }
        static float dot(Point a, Point b)
        {
            return (a.X * b.X + a.Y * b.Y);
        }
        static float cross(Point a, Point b)
        {
            return a.X * b.Y - a.Y * b.X;
        }
        static float lengthSqr(Point v)
        {
            return v.X * v.X + v.Y * v.Y;
        }
        static bool ccw(Point p, Point q, Point r)
        {
            return cross(toVec(q, p), toVec(p, r)) > 0;
        }
        static float angle(Point a, Point o, Point b)
        {
            Point oa = toVec(o, a), ob = toVec(o, b);
            return (float)Math.Acos(dot(oa, ob) / Math.Sqrt(lengthSqr(oa) * lengthSqr(ob)));
        }

        //first and last points of polygon need to be equal
        public static bool inPolygon(Point pt, List<Point> polygon)
        {
            if (polygon.Count == 0)
                return false;
            float sum = 0;
            for (int i = 0; i < polygon.Count - 1; i++)
            {
                if (pt == polygon[i])
                    return true;
                if (ccw(pt, polygon[i], polygon[i + 1]))
                    sum += angle(polygon[i], pt, polygon[i + 1]);
                else
                    sum -= angle(polygon[i], pt, polygon[i + 1]);
            }
            return Math.Abs(sum) > Math.PI;
        }
    }
}
