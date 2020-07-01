using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShapeFill
{
    class Shape
    {
        public Vector[] points;
        public Vector center;
        public Vector tVel;
        public double rVel;
        public double mass;

        public Shape()
        { }
        public Shape(Vector[] points)
        {
            this.points = new Vector[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                this.points[i] = points[i];
                center += points[i];
            }
            center.X /= points.Length;
            center.Y /= points.Length;
        }

        public void FindCenter()
        {
            center = new Vector();
            for (int i = 0; i < points.Length; i++)
                center += points[i];
            center /= points.Length;
        }

        public void Translate(double x, double y)
        {
            for (int p = 0; p < points.Length; p++)
            {
                points[p].X += x;
                points[p].Y += y;
            }
            FindCenter();
        }
        public void Translate(Vector t)
        {
            for (int p = 0; p < points.Length; p++)
                points[p] += t;
            FindCenter();
        }

        public void Rotate(double radians)
        {
            for (int p = 0; p < points.Length; p++)
            {
                points[p] -= center;
                points[p] = new Vector((points[p].X * Math.Cos(radians) - points[p].Y * Math.Sin(radians)),
                    (points[p].X * Math.Sin(radians) + points[p].Y * Math.Cos(radians))) + center;
            }
        }

        public Vector FindVelocity(int p)
        {
            FindCenter();
            return new Vector(
                (points[p].X - center.X) * Math.Cos(rVel) -
                (points[p].Y - center.Y) * Math.Sin(rVel) - (points[p].X - center.X),
                (points[p].X - center.X) * Math.Sin(rVel) +
                (points[p].Y - center.Y) * Math.Cos(rVel) - (points[p].Y - center.Y)) +
                tVel;
        }
    }
    class Pointe
    {
        public Vector pos;
        public Vector vel;
        public (double, double) pRelation;    //relationship to neighbor points (distance to prev, angle between)

        public override string ToString()
        {
            return pos.ToString();
        }
    }

    class Pointv1
    {
        public Vector pos;
        public Vector vel;
        public override string ToString()
        {
            return pos.ToString();
        }
    }
    class Pointv0
    {
        public Vector pos;
        public Vector vel;
        public Vector pRelation;    //target distance to previous point

        public override string ToString()
        {
            return pos.ToString();
        }
    }
}
