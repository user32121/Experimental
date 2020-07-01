﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace ShapeFill
{
    public partial class CollisionHv2 : Form
    {
        Bitmap bmpMain = new Bitmap(400, 400);  //main layer
        Graphics gMain;

        Bitmap bmpDraw = new Bitmap(400, 400);  //drawing layer
        Graphics gDraw;
        List<Vector> drawShape = new List<Vector>();
        Vector curPos;
        Pen drawPen = Pens.Black;

        List<Shape> shapes = new List<Shape>();
        int selectShape = -1;

        public CollisionHv2()
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
            selectShape = -1;
            gMain.Clear(Color.White);
            gDraw.Clear(Color.Transparent);
            Render();
        }

        private void ButtonConfirm_Click(object sender, EventArgs e)
        {
            if (drawShape.Count > 0)
            {
                shapes.Add(new Shape(drawShape.ToArray()));
                shapes[shapes.Count - 1].FindCenter();
            }

            //clear shape
            drawShape.Clear();
            gDraw.Clear(Color.Transparent);
            Render();
        }

        private void PictureDisplay_Click(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Middle)
            {
                selectShape = -1;
                return;
            }

            //add point to shape
            Vector pos = pictureDisplay.PointToClient(Cursor.Position).ToVector();

            drawShape.Add(pos);
            Render();
        }

        private void PictureDisplay_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                Vector mousePos = new Vector(e.X, e.Y);
                double distSqr = double.MaxValue;
                selectShape = -1;
                for (int s = 0; s < shapes.Count; s++)
                    if ((shapes[s].center - mousePos).LengthSquared < distSqr)
                    {
                        selectShape = s;
                        distSqr = (shapes[s].center - mousePos).LengthSquared;
                    }
            }
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
            {
                switch (shapes[i].points.Length)
                {
                    case 0:
                        break;
                    case 1:
                        //single point
                        if (shapes[i].points[0].X >= 0 && shapes[i].points[0].X < bmpMain.Width &&
                            shapes[i].points[0].Y >= 0 && shapes[i].points[0].Y < bmpMain.Height)
                            bmpMain.SetPixel((int)shapes[i].points[0].X, (int)shapes[i].points[0].Y, drawPen.Color);
                        break;
                    case 2:
                        //line
                        gMain.DrawLine(drawPen, shapes[i].points[0].ToPointF(), shapes[i].points[1].ToPointF());
                        break;
                    default:
                        //fill shape (scan line method)
                        double[,] lines = new double[shapes[i].points.Length, 4];
                        int min = (int)shapes[i].points[0].X, max = (int)shapes[i].points[0].X;
                        for (int i1 = 0; i1 < shapes[i].points.Length; i1++)
                        {
                            int i2 = (i1 + 1) % shapes[i].points.Length;
                            lines[i1, 0] = (shapes[i].points[i1].Y - shapes[i].points[i2].Y) / (shapes[i].points[i1].X - shapes[i].points[i2].X);  //slope
                            lines[i1, 1] = shapes[i].points[i1].Y - lines[i1, 0] * shapes[i].points[i1].X;  //y int
                            lines[i1, 2] = shapes[i].points[i1].X; lines[i1, 3] = shapes[i].points[i2].X;  //x range

                            min = Math.Min(min, (int)shapes[i].points[i1].X);
                            max = Math.Max(max, (int)shapes[i].points[i1].X);
                        }

                        List<int> yVal = new List<int>();
                        for (int x = min; x <= max; x++)
                        {
                            for (int l = 0; l < shapes[i].points.Length; l++)
                                if (x >= lines[l, 2] ^ x >= lines[l, 3])
                                    yVal.Add((int)(lines[l, 0] * x + lines[l, 1]));

                            yVal.Sort();
                            for (int y = 0; y < yVal.Count; y += 2)
                                gMain.DrawLine(drawPen, x, yVal[y], x, yVal[y + 1]);
                            yVal.Clear();
                        }
                        break;
                }
            }

            //transfer to display
            pictureDisplay.BackgroundImage = bmpMain;
            pictureDisplay.Image = bmpDraw;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            const double tFac = 1;
            const double rFac = 0.01;

            for (int s = 0; s < shapes.Count; s++)
            {
                //gravity
                shapes[s].tVel.Y += 0.1f;

                //collision detection
                for (int p = 0; p < shapes[s].points.Length; p++)
                {
                    for (int s2 = 0; s2 < shapes.Count; s2++)
                        if (s != s2)
                        {
                            //convert shape into lines
                            double[,] lines = new double[shapes[s2].points.Length, 4];
                            int min = (int)shapes[s2].points[0].X, max = (int)shapes[s].points[0].X;
                            for (int i1 = 0; i1 < shapes[s2].points.Length; i1++)
                            {
                                int i2 = (i1 + 1) % shapes[s2].points.Length;
                                lines[i1, 0] = (shapes[s2].points[i1].Y - shapes[s2].points[i2].Y) / (shapes[s2].points[i1].X - shapes[s2].points[i2].X);  //slope
                                if (double.IsInfinity(lines[i1, 0]))
                                    lines[i1, 0] = Math.Sign(lines[i1, 0]) * 100;
                                lines[i1, 1] = shapes[s2].points[i1].Y - lines[i1, 0] * shapes[s2].points[i1].X;  //y int
                                lines[i1, 2] = shapes[s2].points[i1].X; lines[i1, 3] = shapes[s2].points[i2].X;  //x range

                                min = Math.Min(min, (int)shapes[s2].points[i1].X);
                                max = Math.Max(max, (int)shapes[s2].points[i1].X);
                            }

                            //check if point is within other shape's boundary
                            List<Vector> inters = new List<Vector>();  //storing distance from point to line
                            bool inside = false;
                            if (shapes[s].points[p].X >= min && shapes[s].points[p].X <= max)
                            {
                                int below = -1;
                                for (int l = 0; l < lines.GetLength(0); l++)
                                {
                                    if (shapes[s].points[p].X >= lines[l, 2] ^ shapes[s].points[p].X >= lines[l, 3])
                                    {
                                        if (below == -1)
                                            below = shapes[s].points[p].Y < lines[l, 0] * shapes[s].points[p].X + lines[l, 1] ? 1 : 0;
                                        else
                                         //find when point changes from below lines to above lines
                                         if (below == 1 ^ shapes[s].points[p].Y <= lines[l, 0] * shapes[s].points[p].X + lines[l, 1])
                                            inside = true;
                                    }
                                    double a2 = -1 / lines[l, 0];
                                    if (double.IsInfinity(a2))
                                        a2 = Math.Sign(a2) * 100;
                                    double b2 = shapes[s].points[p].Y - a2 * shapes[s].points[p].X;
                                    double x = (b2 - lines[l, 1]) / (lines[l, 0] - a2);
                                    if ((x > shapes[s2].points[l].X ^ x > shapes[s2].points[(l + 1) % shapes[s2].points.Length].X) ||
                                        ((a2 * x + b2) > shapes[s2].points[l].Y ^ (a2 * x + b2) > shapes[s2].points[(l + 1) % shapes[s2].points.Length].Y))
                                        inters.Add((new Vector(x, a2 * x + b2) - shapes[s].points[p]) * 0.5);
                                    else
                                        inters.Add(new Vector(double.MaxValue, double.MaxValue));
                                }
                            }
                            if (inside)
                            {
                                //find closest line to point
                                int closeLine = -1;
                                for (int i = 0; i < shapes[s2].points.Length; i++)
                                    if (inters[i].X != double.MinValue && (closeLine == -1 || inters[i].LengthSquared < inters[closeLine].LengthSquared))
                                        closeLine = i;
                                int closeLine2 = (closeLine + 1) % inters.Count;

                                //resolve collision
                                shapes[s].Translate(inters[closeLine]);
                                shapes[s2].Translate(-inters[closeLine]);

                                //project relative velocity onto collision normal
                                double relativeDist = (inters[closeLine] + shapes[s].points[p] - shapes[s2].points[closeLine]).Length / (shapes[s2].points[closeLine] - shapes[s2].points[closeLine2]).Length;
                                Vector u = shapes[s2].FindVelocity(closeLine) * relativeDist + shapes[s2].FindVelocity(closeLine2) * (1 - relativeDist) - shapes[s].FindVelocity(p);
                                u = u * inters[closeLine] / inters[closeLine].LengthSquared * inters[closeLine];
                                if (Vector.AngleBetween(u, inters[closeLine]) < 1)
                                {
                                    //find the point's direction of rotation
                                    shapes[s].FindCenter();
                                    Vector v = shapes[s].center - shapes[s].points[p];
                                    v = new Vector(v.Y, -v.X);
                                    shapes[s2].FindCenter();
                                    Vector v2 = shapes[s2].center - (inters[closeLine] + shapes[s].points[p]);
                                    v2 = new Vector(v2.Y, -v2.X);

                                    //project new collision velocity onto rotation vector
                                    Vector rVel = u * v / v.LengthSquared * v;
                                    Vector rVel2 = -u * v2 / v2.LengthSquared * v2;

                                    //apply forces
                                    shapes[s].rVel += Vector.AngleBetween(shapes[s].points[p] - shapes[s].center, shapes[s].points[p] + rVel - shapes[s].center) * rFac;
                                    shapes[s].tVel += (u - rVel) * tFac;
                                    shapes[s2].rVel += Vector.AngleBetween(inters[closeLine] + shapes[s].points[p] - shapes[s2].center, inters[closeLine] + shapes[s].points[p] + rVel - shapes[s2].center) * rFac;
                                    shapes[s2].tVel += (-u - rVel2) * tFac;
                                }
                            }
                        }

                    //boundary
                    if (shapes[s].points[p].X < 0)
                    {
                        shapes[s].Translate(-shapes[s].points[p].X, 0);

                        Vector u = -shapes[s].FindVelocity(p);
                        if (u.X > 0)
                        {
                            u.Y = 0;
                            shapes[s].FindCenter();
                            Vector v = shapes[s].center - shapes[s].points[p];
                            v = new Vector(v.Y, -v.X);

                            Vector rVel = u * v / v.LengthSquared * v;
                            shapes[s].rVel += Vector.AngleBetween(shapes[s].points[p] - shapes[s].center, shapes[s].points[p] + rVel - shapes[s].center) * rFac;
                            shapes[s].tVel += (u - rVel) * tFac;
                        }
                    }
                    else if (shapes[s].points[p].X >= bmpMain.Width)
                    {
                        shapes[s].Translate(bmpMain.Width - shapes[s].points[p].X, 0);

                        Vector u = -shapes[s].FindVelocity(p);
                        if (u.X < 0)
                        {
                            u.Y = 0;
                            shapes[s].FindCenter();
                            Vector v = shapes[s].center - shapes[s].points[p];
                            v = new Vector(v.Y, -v.X);

                            Vector rVel = u * v / v.LengthSquared * v;
                            shapes[s].rVel += Vector.AngleBetween(shapes[s].points[p] - shapes[s].center, shapes[s].points[p] + rVel - shapes[s].center) * rFac;
                            shapes[s].tVel += (u - rVel) * tFac;
                        }
                    }
                    if (shapes[s].points[p].Y < 0)
                    {
                        shapes[s].Translate(0, -shapes[s].points[p].Y);

                        Vector u = -shapes[s].FindVelocity(p);
                        if (u.Y > 0)
                        {
                            u.X = 0;
                            shapes[s].FindCenter();
                            Vector v = shapes[s].center - shapes[s].points[p];
                            v = new Vector(v.Y, -v.X);

                            Vector rVel = u * v / v.LengthSquared * v;
                            shapes[s].rVel += Vector.AngleBetween(shapes[s].points[p] - shapes[s].center, shapes[s].points[p] + rVel - shapes[s].center) * rFac;
                            shapes[s].tVel += (u - rVel) * tFac;
                        }
                    }
                    else if (shapes[s].points[p].Y >= bmpMain.Height)
                    {
                        //resolve collision
                        shapes[s].Translate(0, bmpMain.Height - shapes[s].points[p].Y);

                        //project velocity onto collision normal
                        Vector u = -shapes[s].FindVelocity(p);
                        if (u.Y < 0)
                        {
                            u.X = 0;
                            shapes[s].FindCenter();

                            //find the point's direction of rotation
                            Vector v = shapes[s].center - shapes[s].points[p];
                            v = new Vector(v.Y, -v.X);

                            //project new collision velocity onto rotation vector
                            Vector rVel = u * v / v.LengthSquared * v;

                            //apply forces
                            shapes[s].rVel += Vector.AngleBetween(shapes[s].points[p] - shapes[s].center, shapes[s].points[p] + rVel - shapes[s].center) * rFac;
                            shapes[s].tVel += (u - rVel) * tFac;
                        }
                    }
                }
                //drag
                shapes[s].tVel.X *= 0.99f;
                shapes[s].tVel.Y *= 0.99f;
                shapes[s].rVel *= 0.99f;

                //rotate
                shapes[s].Rotate(shapes[s].rVel);
                //translate
                shapes[s].Translate(shapes[s].tVel);
            }

            if (selectShape != -1)
            {
                shapes[selectShape].tVel += (pictureDisplay.PointToClient(Cursor.Position).ToVector() - shapes[selectShape].center) * 0.02;
                shapes[selectShape].tVel *= 0.9;
            }

            Render();
        }

        int CompareLength(Vector s1, Vector s2)
        {
            if (s1.LengthSquared < s2.LengthSquared)
                return -1;
            else if (s1.LengthSquared == s2.LengthSquared)
                return 0;
            else
                return 1;
        }

        private void buttonTetro_Click(object sender, EventArgs e)
        {
            switch (new Random().Next(7))
            {
                case 0:   //L
                    shapes.Add(new Shape(new Vector[] { new Vector(0, 0), new Vector(50, 0), new Vector(50, 100), new Vector(100, 100), new Vector(100, 150), new Vector(0, 150) }));
                    break;
                case 1:   //J
                    shapes.Add(new Shape(new Vector[] { new Vector(100, 0), new Vector(100, 150), new Vector(0, 150), new Vector(0, 100), new Vector(50, 100), new Vector(50, 0) }));
                    break;
                case 2:   //S
                    shapes.Add(new Shape(new Vector[] { new Vector(150, 0), new Vector(150, 50), new Vector(100, 50), new Vector(100, 100), new Vector(0, 100), new Vector(0, 50), new Vector(50, 50), new Vector(50, 0) }));
                    break;
                case 3:   //Z
                    shapes.Add(new Shape(new Vector[] { new Vector(0, 0), new Vector(100, 0), new Vector(100, 50), new Vector(150, 50), new Vector(150, 100), new Vector(50, 100), new Vector(50, 50), new Vector(0, 50) }));
                    break;
                case 4:   //T
                    shapes.Add(new Shape(new Vector[] { new Vector(0, 0), new Vector(150, 0), new Vector(150, 50), new Vector(100, 50), new Vector(100, 100), new Vector(50, 100), new Vector(50, 50), new Vector(0, 50) }));
                    break;
                case 5:   //O
                    shapes.Add(new Shape(new Vector[] { new Vector(0, 0), new Vector(100, 0), new Vector(100, 100), new Vector(0, 100) }));
                    break;
                case 6:   //I
                    shapes.Add(new Shape(new Vector[] { new Vector(0, 0), new Vector(50, 0), new Vector(50, 200), new Vector(0, 200) }));
                    break;
                default:
                    break;
            }

            Render();
        }
    }
}
