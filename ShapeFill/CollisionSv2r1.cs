using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace ShapeFill
{
    public partial class CollisionSv2r1 : Form
    {
        Bitmap bmpMain = new Bitmap(400, 400);  //main layer
        Graphics gMain;

        Bitmap bmpDraw = new Bitmap(400, 400);  //drawing layer
        Graphics gDraw;
        List<Vector> drawShape = new List<Vector>();
        Vector curPos;
        Pen drawPen = Pens.Black;
        Brush drawBrush = Brushes.Black;

        List<Pointe[]> shapes = new List<Pointe[]>();

        const int pointDist = 10;

        public CollisionSv2r1()
        {
            InitializeComponent();

            gMain = Graphics.FromImage(bmpMain);
            gMain.Clear(Color.White);
            gDraw = Graphics.FromImage(bmpDraw);
            Render();
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            drawShape.Clear();
            shapes.Clear();
            gMain.Clear(Color.White);
            gDraw.Clear(Color.Transparent);
            Render();
        }

        private void ButtonConfirm_Click(object sender, EventArgs e)
        {
            if (drawShape.Count >= 3)
                AddShape(drawShape.ToArray());

            //clear shape
            drawShape.Clear();
            gDraw.Clear(Color.Transparent);
            Render();
        }

        private void PictureDisplay_Click(object sender, EventArgs e)
        {
            //add point to shape
            Vector pos = pictureDisplay.PointToClient(Cursor.Position).ToVector();

            drawShape.Add(pos);
            Render();
        }

        private void PictureDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            //move line
            if (drawShape.Count > 0)
            {
                //erase old line
                DrawLine(Color.Transparent, drawShape[drawShape.Count - 1], curPos);
                //draw new line
                curPos = pictureDisplay.PointToClient(Cursor.Position).ToVector();
                DrawLine(drawPen.Color, drawShape[drawShape.Count - 1], curPos);
                Render();
            }
        }

        void DrawLine(Color col, Vector p1, Vector p2)
        {
            //put a line on bmpDraw (overwrite mode)
            int dMax = (int)Math.Max(Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y));
            double x = p1.X, y = p1.Y,
                dx = (p2.X - p1.X) / dMax, dy = (p2.Y - p1.Y) / dMax;
            for (int i = 0; i < dMax; i++)
            {
                x += dx;
                y += dy;
                if (x >= 0 && x < bmpDraw.Width - 0.5 && y >= 0 && y < bmpDraw.Height - 0.5)
                    bmpDraw.SetPixel((int)Math.Round(x), (int)Math.Round(y), col);
            }
        }

        void Render()
        {
            gMain.Clear(Color.White);

            //drawing shape
            for (int i = 0; i < drawShape.Count - 1; i++)
            {
                gMain.DrawLine(drawPen, drawShape[i].ToPointF(), drawShape[i + 1].ToPointF());
            }

            //physics shapes
            for (int i = 0; i < shapes.Count; i++)
                gMain.FillPolygon(drawBrush, shapes[i].ToPointFArray());

            //transfer to display
            pictureDisplay.BackgroundImage = bmpMain;
            pictureDisplay.Image = bmpDraw;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            const double tFac = 1;

            for (int s = 0; s < shapes.Count; s++)
            {
                //shapes[s][1].pos.Y = 10;

                //distance
                Vector uVec = shapes[s][0].pos - shapes[s][1].pos;
                Vector vVec = shapes[s][2].pos - shapes[s][1].pos;
                //find offset from target distance

                Vector velVec = uVec * (uVec.Length - shapes[s][0].pRelation.Item1) / uVec.Length * tFac;
                Vector point1Vel = /*shapes[s][1].vel +*/ velVec;
                Vector point0Vel = /*shapes[s][0].vel*/ -velVec;
                double ang = Vector.AngleBetween(vVec, uVec) * Math.PI / 180;  //angle between points p0 and p2 from p
                ang += shapes[s][1].pRelation.Item2;  //how much more it needs to rotate
                //keep in a circle
                if (ang >= Math.PI)
                    ang -= Math.PI * 2;
                else if (ang <= -Math.PI)
                    ang += Math.PI * 2;
#pragma warning disable CS0642 // Possible mistaken empty statement
                if (Math.Abs(ang) > 1) ;
#pragma warning restore CS0642 // Possible mistaken empty statement
                ang /= 2;

                uVec.Normalize();
                uVec *= shapes[s][0].pRelation.Item1;
                vVec.Normalize();
                vVec *= shapes[s][1].pRelation.Item1;
                point0Vel = /*shapes[s][0].vel*/ -(new Vector(uVec.X * Math.Cos(ang) - uVec.Y * Math.Sin(ang),
                      uVec.X * Math.Sin(ang) + uVec.Y * Math.Cos(ang)) - uVec) * 0.2 * tFac;
                Vector point2Vel = /*shapes[s][2].vel*/ -(new Vector(vVec.X * Math.Cos(ang) + vVec.Y * Math.Sin(ang),
                       vVec.X * -Math.Sin(ang) + vVec.Y * Math.Cos(ang)) - vVec) * 0.2 * tFac;

                for (int p = 0; p < shapes[s].Length; p++)
                {
                    //gravity
                    shapes[s][p].vel.Y += 0.05f;

                    //collision detection
                    for (int s2 = 0; s2 < shapes.Count; s2++)
                        if (s != s2)
                            for (int s2p = 0; s2p < shapes[s2].Length; s2p++)
                            {
                                Vector dist = shapes[s][p].pos - shapes[s2][s2p].pos;
                                if (dist.LengthSquared < pointDist * pointDist)
                                {
                                    dist *= (pointDist - dist.Length) / dist.Length;
                                    shapes[s][p].vel += dist / 2;
                                    shapes[s2][s2p].vel -= dist / 2;
                                }
                            }

                    //boundary
                    if (shapes[s][p].pos.X < 0)
                    {
                        shapes[s][p].vel.X *= 0.5;
                        shapes[s][p].pos.X = 0;
                    }
                    else if (shapes[s][p].pos.X >= bmpMain.Width)
                    {
                        shapes[s][p].vel.X *= 0.5;
                        shapes[s][p].pos.X = bmpMain.Width;
                    }
                    if (shapes[s][p].pos.Y < 0)
                    {
                        shapes[s][p].vel.Y *= 0.5;
                        shapes[s][p].pos.Y = 0;
                    }
                    else if (shapes[s][p].pos.Y >= bmpMain.Height)
                    {
                        shapes[s][p].vel.Y *= 0.5;
                        shapes[s][p].pos.Y = bmpMain.Height;
                    }

                    //target pos
                    int p0 = (p - 1 + shapes[s].Length) % shapes[s].Length;  //point before
                    int p2 = (p + 1) % shapes[s].Length;  //point after

                    //distance
                    Vector u = shapes[s][p0].pos - shapes[s][p].pos;
                    Vector v = shapes[s][p2].pos - shapes[s][p].pos;
                    //find offset from target distance
                    Vector vel = u * (u.Length - shapes[s][p].pRelation.Item1) / u.Length * tFac;
                    shapes[s][p].vel += vel;
                    shapes[s][p0].vel -= vel;

                    //angle
                    double a = Vector.AngleBetween(v, u) * Math.PI / 180;  //angle between points p0 and p2 from p
                    a += shapes[s][p].pRelation.Item2;  //how much more it needs to rotate
                    //keep in a circle
                    if (a >= Math.PI)
                        a -= Math.PI * 2;
                    else if (a <= -Math.PI)
                        a += Math.PI * 2;
#pragma warning disable CS0642 // Possible mistaken empty statement
                    if (Math.Abs(a) > 1) ;
#pragma warning restore CS0642 // Possible mistaken empty statement
                    a /= 2;

                    u.Normalize();
                    u *= shapes[s][p0].pRelation.Item1;
                    v.Normalize();
                    v *= shapes[s][p].pRelation.Item1;
                    shapes[s][p0].vel -= (new Vector(u.X * Math.Cos(a) - u.Y * Math.Sin(a),
                        u.X * Math.Sin(a) + u.Y * Math.Cos(a)) - u) * 0.3 * tFac;
                    shapes[s][p2].vel -= (new Vector(v.X * Math.Cos(a) + v.Y * Math.Sin(a),
                        v.X * -Math.Sin(a) + v.Y * Math.Cos(a)) - v) * 0.3 * tFac;
                }
                for (int p = 0; p < shapes[s].Length; p++)
                {
                    //drag
                    shapes[s][p].vel *= 0.9f;

                    //translate
                    shapes[s][p].pos += shapes[s][p].vel;
                }
            }
            Render();
        }

        int CompareLength((Vector, int) s1, (Vector, int) s2)
        {
            if (s1.Item1.LengthSquared < s2.Item1.LengthSquared)
                return -1;
            else if (s1.Item1.LengthSquared == s2.Item1.LengthSquared)
                return 0;
            else
                return 1;
        }

        Vector FindCenter(Pointe[] points)
        {
            Vector center = new Vector();
            for (int i = 0; i < points.Length; i++)
                center += points[i].pos;
            return center / points.Length;
        }

        private void ButtonTetro_Click(object sender, EventArgs e)
        {
            switch (new Random().Next(7))
            {
                case 0:   //L
                    AddShape(new Vector[] { new Vector(0, 0), new Vector(0, 150), new Vector(100, 150), new Vector(100, 100), new Vector(50, 100), new Vector(50, 0) });
                    break;
                case 1:   //J
                    AddShape(new Vector[] { new Vector(100, 0), new Vector(100, 150), new Vector(0, 150), new Vector(0, 100), new Vector(50, 100), new Vector(50, 0) });
                    break;
                case 2:   //S
                    AddShape(new Vector[] { new Vector(150, 0), new Vector(50, 0), new Vector(50, 50), new Vector(0, 50), new Vector(0, 100), new Vector(100, 100), new Vector(100, 50), new Vector(150, 50) });
                    break;
                case 3:   //Z
                    AddShape(new Vector[] { new Vector(0, 0), new Vector(100, 0), new Vector(100, 50), new Vector(150, 50), new Vector(150, 100), new Vector(50, 100), new Vector(50, 50), new Vector(0, 50) });
                    break;
                case 4:   //T
                    AddShape(new Vector[] { new Vector(0, 0), new Vector(150, 0), new Vector(150, 50), new Vector(100, 50), new Vector(100, 100), new Vector(50, 100), new Vector(50, 50), new Vector(0, 50) });
                    break;
                case 5:   //O
                    AddShape(new Vector[] { new Vector(0, 0), new Vector(100, 0), new Vector(100, 100), new Vector(0, 100) });
                    break;
                case 6:   //I
                    AddShape(new Vector[] { new Vector(0, 0), new Vector(50, 0), new Vector(50, 200), new Vector(0, 200) });
                    break;
                default:
                    break;
            }

            Render();
        }

        private void AddShape(Vector[] points)
        {
            List<Pointe> pts = new List<Pointe>();

            //cut into smaller segments
            for (int i = 0; i < points.Length; i++)
            {
                Vector dir = points[i] - points[(i + 1) % points.Length];
                double segments = Math.Ceiling(dir.Length / pointDist);
                Vector s = dir / segments;
                Vector p = points[i];
                for (int i2 = 0; i2 < segments; i2++, p -= s)
                    pts.Add(new Pointe() { pos = p });
            }
            //assign relationship
            for (int p = 0; p < pts.Count; p++)
            {
                Vector u = pts[(p - 1 + pts.Count) % pts.Count].pos - pts[p].pos;
                Vector v = pts[(p + 1) % pts.Count].pos - pts[p].pos;
                pts[p].pRelation = (u.Length, Vector.AngleBetween(u, v) * Math.PI / 180);
            }
            shapes.Add(pts.ToArray());
        }
    }
}
