using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace ShapeFill
{
    public partial class CollisionSv3r1 : Form
    {
        Bitmap bmpMain = new Bitmap(400, 400);  //main layer
        Graphics gMain;

        Bitmap bmpDraw = new Bitmap(400, 400);  //drawing layer
        Graphics gDraw;
        List<Vector> drawShape = new List<Vector>();
        Vector curPos;
        Pen drawPen = Pens.Black;

        List<Pointv1[,]> shapes = new List<Pointv1[,]>();
        List<System.Drawing.Point> centers = new List<System.Drawing.Point>();

        const int pointDist = 3;

        public CollisionSv3r1()
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
            centers.Clear();
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
            {
                for (int x = 0; x < shapes[i].GetLength(0); x++)
                    for (int y = 0; y < shapes[i].GetLength(1); y++)
                        if (shapes[i][x, y] != null)
                            gMain.DrawRectangle(drawPen, (float)shapes[i][x, y].pos.X, (float)shapes[i][x, y].pos.Y, 3, 3);
            }

            //transfer to display
            pictureDisplay.BackgroundImage = bmpMain;
            pictureDisplay.Image = bmpDraw;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            for (int s = 0; s < shapes.Count; s++)
            {
                //gravity (apply to single point)
                shapes[s][centers[s].X, centers[s].Y].vel.Y += 5;

                for (int x = 0; x < shapes[s].GetLength(0); x++)
                    for (int y = 0; y < shapes[s].GetLength(1); y++)
                        if (shapes[s][x, y] != null)
                        {
                            ////collision detection
                            //for (int s2 = 0; s2 < shapes.Count; s2++)
                            //    if (s != s2)
                            //    {
                            //        //convert shape into lines
                            //        double[,] lines = new double[shapes[s2].Length, 4];
                            //        int min = (int)shapes[s2][0].pos.X, max = (int)shapes[s][0].pos.X;
                            //        for (int i1 = 0; i1 < shapes[s2].Length; i1++)
                            //        {
                            //            int i2 = (i1 + 1) % shapes[s2].Length;
                            //            lines[i1, 0] = (shapes[s2][i1].pos.Y - shapes[s2][i2].pos.Y) / (shapes[s2][i1].pos.X - shapes[s2][i2].pos.X);  //slope
                            //            if (double.IsInfinity(lines[i1, 0]))
                            //                lines[i1, 0] = Math.Sign(lines[i1, 0]) * 1.0E9;
                            //            lines[i1, 1] = shapes[s2][i1].pos.Y - lines[i1, 0] * shapes[s2][i1].pos.X;  //y int
                            //            lines[i1, 2] = shapes[s2][i1].pos.X; lines[i1, 3] = shapes[s2][i2].pos.X;  //x range

                            //            min = Math.Min(min, (int)shapes[s2][i1].pos.X);
                            //            max = Math.Max(max, (int)shapes[s2][i1].pos.X);
                            //        }

                            //        //check if point is within other shape's boundary
                            //        List<(Vector, int)> inters = new List<(Vector, int)>();
                            //        bool inside = false;
                            //        if (shapes[s][p].pos.X >= min && shapes[s][p].pos.X <= max)
                            //        {
                            //            int below = -1;
                            //            for (int l = 0; l < lines.GetLength(0); l++)
                            //            {
                            //                if (shapes[s][p].pos.X >= lines[l, 2] ^ shapes[s][p].pos.X >= lines[l, 3])
                            //                {
                            //                    if (below == -1)
                            //                        below = shapes[s][p].pos.Y < lines[l, 0] * shapes[s][p].pos.X + lines[l, 1] ? 1 : 0;
                            //                    else
                            //                     //find when point changes from below lines to above lines
                            //                     if (below == 1 ^ shapes[s][p].pos.Y < lines[l, 0] * shapes[s][p].pos.X + lines[l, 1])
                            //                        inside = true;

                            //                    double a2 = -1 / lines[l, 0];
                            //                    if (double.IsInfinity(a2))
                            //                        a2 = Math.Sign(a2) * 1.0E9;
                            //                    double b2 = shapes[s][p].pos.Y - a2 * shapes[s][p].pos.X;
                            //                    double x = ((b2 - lines[l, 1]) / (lines[l, 0] - a2));
                            //                    inters.Add((new Vector(x, a2 * x + b2), l));
                            //                    (Vector, int) intersection = inters[inters.Count - 1];
                            //                    intersection.Item1 -= shapes[s][p].pos;
                            //                    intersection.Item1 *= 0.5f;
                            //                    inters[inters.Count - 1] = intersection;
                            //                }
                            //            }
                            //            if (inside)
                            //            {
                            //                //find smallest intersection
                            //                inters.Sort(new Comparison<(Vector, int)>(CompareLength));

                            //                //resolve collision
                            //                //normal of collision line
                            //                Vector normal2 = shapes[s2][inters[0].Item2].pos -
                            //                    shapes[s2][(inters[0].Item2 + 1) % shapes[s2].Length].pos;
                            //                normal2.Normalize();

                            //                //find normal
                            //                Vector normal = normal2;
                            //                Vector center = FindCenter(shapes[s2]);
                            //                if (center.Y < lines[inters[0].Item2, 0] * center.X + lines[inters[0].Item2, 1])
                            //                    normal.X *= -1;
                            //                else
                            //                    normal.Y *= -1;
                            //                //project velocity onto normal
                            //                Vector proj = (shapes[s][p].vel.X * normal.X + shapes[s][p].vel.Y * normal.Y) *
                            //                    normal * 0.5;  //halve it

                            //                //velocity
                            //                shapes[s][p].vel -= proj;
                            //                //shapes[s][line1.p1].vel += proj * 0.5;
                            //                //shapes[s][line1.p2].vel += proj * 0.5;

                            //                //position
                            //                shapes[s][p].pos -= proj;
                            //                //shapes[s][line1.p1].pos += proj * 0.5;
                            //                //shapes[s][line1.p2].pos += proj * 0.5;
                            //            }
                            //        }
                            //    }

                            //boundary
                            if (shapes[s][x, y].pos.X < 0)
                            {
                                shapes[s][x, y].vel.X *= 0.5;
                                shapes[s][x, y].pos.X = 0;
                            }
                            else if (shapes[s][x, y].pos.X >= bmpMain.Width)
                            {
                                shapes[s][x, y].vel.X *= 0.5;
                                shapes[s][x, y].pos.X = bmpMain.Width;
                            }
                            if (shapes[s][x, y].pos.Y < 0)
                            {
                                shapes[s][x, y].vel.Y *= 0.5;
                                shapes[s][x, y].pos.Y = 0;
                            }
                            else if (shapes[s][x, y].pos.Y >= /*bmpMain.Height*/200)
                            {
                                shapes[s][x, y].vel.Y *= 0.5;
                                shapes[s][x, y].pos.Y = /*bmpMain.Height*/200;
                            }

                            //target pos
                            if (x == 0 || shapes[s][x - 1, y] == null)
                            {
                                Vector dist = shapes[s][x + 1, y].pos - shapes[s][x + 2, y].pos;
                                dist.Normalize();
                                dist *= pointDist;
                                shapes[s][x, y].vel += (shapes[s][x + 1, y].pos + dist - shapes[s][x, y].pos) / 4;
                            }
                            else if (x == shapes[s].GetLength(0) - 1 || shapes[s][x + 1, y] == null)
                            {
                                Vector dist = shapes[s][x - 1, y].pos - shapes[s][x - 2, y].pos;
                                dist.Normalize();
                                dist *= pointDist;
                                shapes[s][x, y].vel += (shapes[s][x - 1, y].pos + dist - shapes[s][x, y].pos) / 4;
                            }
                            else
                            {
                                //middle pos
                                shapes[s][x, y].vel += ((shapes[s][x - 1, y].pos + shapes[s][x + 1, y].pos) / 2 - shapes[s][x, y].pos) / 2;

                                //dist
                                Vector dist = shapes[s][x, y].pos - shapes[s][x - 1, y].pos;
                                dist *= (pointDist - dist.Length) / dist.Length;
                                if (!double.IsNaN(dist.Length))
                                {
                                    shapes[s][x, y].vel += dist / 4;
                                    shapes[s][x - 1, y].vel -= dist / 4;
                                }

                                dist = shapes[s][x, y].pos - shapes[s][x + 1, y].pos;
                                dist *= (pointDist - dist.Length) / dist.Length;
                                if (!double.IsNaN(dist.Length))
                                {
                                    shapes[s][x, y].vel += dist / 4;
                                    shapes[s][x + 1, y].vel -= dist / 4;
                                }
                            }
                            if (y == 0 || shapes[s][x, y - 1] == null)
                            {
                                Vector dist = shapes[s][x, y + 1].pos - shapes[s][x, y + 2].pos;
                                dist.Normalize();
                                dist *= pointDist;
                                shapes[s][x, y].vel += (shapes[s][x, y + 1].pos + dist - shapes[s][x, y].pos) / 4;
                            }
                            else if (y == shapes[s].GetLength(1) - 1 || shapes[s][x, y + 1] == null)
                            {
                                Vector dist = shapes[s][x, y - 1].pos - shapes[s][x, y - 2].pos;
                                dist.Normalize();
                                dist *= pointDist;
                                shapes[s][x, y].vel += (shapes[s][x, y - 1].pos + dist - shapes[s][x, y].pos) / 4;
                            }
                            else
                            {
                                //middle pos
                                shapes[s][x, y].vel += ((shapes[s][x, y - 1].pos + shapes[s][x, y + 1].pos) / 2 - shapes[s][x, y].pos) / 2;

                                //dist
                                Vector dist = shapes[s][x, y].pos - shapes[s][x, y - 1].pos;
                                dist *= (pointDist - dist.Length) / dist.Length;
                                if (!double.IsNaN(dist.Length))
                                {
                                    shapes[s][x, y].vel += dist / 4;
                                    shapes[s][x, y - 1].vel -= dist / 4;
                                }

                                dist = shapes[s][x, y].pos - shapes[s][x, y + 1].pos;
                                dist *= (pointDist - dist.Length) / dist.Length;
                                if (!double.IsNaN(dist.Length))
                                {
                                    shapes[s][x, y].vel += dist / 4;
                                    shapes[s][x, y + 1].vel -= dist / 4;
                                }
                            }


                            //            //distance
                            //            Vector u = shapes[s][p0].pos - shapes[s][p].pos;
                            //            Vector v = shapes[s][p2].pos - shapes[s][p].pos;
                            //            //find offset from target distance
                            //            Vector vel = u * (u.Length - shapes[s][p].pRelation.Item1) / u.Length * tFac;
                            //            shapes[s][p].vel += vel;
                            //            shapes[s][p0].vel -= vel;

                            //            //angle
                            //            double a = Vector.AngleBetween(u, v) * Math.PI / 180;  //angle between points p0 and p2 from p
                            //            a -= shapes[s][p].pRelation.Item2;  //how much more it needs to rotate
                            //                                                //keep in a circle
                            //            if (a >= Math.PI)
                            //                a -= Math.PI * 2;
                            //            else if (a <= -Math.PI)
                            //                a += Math.PI * 2;
                            //            if (Math.Abs(a) > 1) ;
                            //            a /= 2;

                            //            shapes[s][p0].vel += (new Vector(u.X * Math.Cos(a) - u.Y * Math.Sin(a),
                            //                u.X * Math.Sin(a) + u.Y * Math.Cos(a)) - u) * 0.2 * tFac;
                            //            shapes[s][p2].vel += (new Vector(v.X * Math.Cos(a) + v.Y * Math.Sin(a),
                            //                v.X * -Math.Sin(a) + v.Y * Math.Cos(a)) - v) * 0.2 * tFac;
                        }
                for (int x = 0; x < shapes[s].GetLength(0); x++)
                    for (int y = 0; y < shapes[s].GetLength(1); y++)
                        if (shapes[s][x, y] != null)
                        {
                            //drag
                            shapes[s][x, y].vel *= 0.8f;

                            //translate
                            shapes[s][x, y].pos += shapes[s][x, y].vel;
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
            //find bounds
            Vector min = points[0], max = points[0];
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].X < min.X)
                    min.X = points[i].X;
                else if (points[i].X > max.X)
                    max.X = points[i].X;
                if (points[i].Y < min.Y)
                    min.Y = points[i].Y;
                else if (points[i].Y > max.Y)
                    max.Y = points[i].Y;
            }
            int xSize = (int)(max.X - min.X) / pointDist, ySize = (int)(max.Y - min.Y) / pointDist;

            //create shape
            Pointv1[,] pts = new Pointv1[xSize, ySize];

            //generate points
            System.Drawing.Point cen = new System.Drawing.Point(-1, 0);
            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    bool inside = false;
                    for (int i = 0; i < points.Length; i++)
                    {
                        int i2 = i == 0 ? points.Length - 1 : i - 1;
                        if (y * pointDist + min.Y >= Math.Min(points[i].Y, points[i2].Y) && y * pointDist + min.Y < Math.Max(points[i].Y, points[i2].Y) &&
                            x * pointDist + min.X >= (y * pointDist + min.Y - points[i].Y) / (points[i2].Y - points[i].Y) * (points[i2].X - points[i].X) + points[i].X)
                            inside = !inside;
                    }
                    if (inside)
                    {
                        pts[x, y] = new Pointv1() { pos = new Vector(min.X + x * pointDist, min.Y + y * pointDist) };
                        //find center
                        if (cen.X == -1 || Math.Pow(x - xSize / 2, 2) + Math.Pow(y - ySize / 2, 2) <
                            Math.Pow(cen.X - xSize / 2, 2) + Math.Pow(cen.Y - ySize / 2, 2))
                        {
                            cen.X = x;
                            cen.Y = y;
                        }
                    }
                }
            }
            shapes.Add(pts);
            centers.Add(cen);
        }

        private static int CompareLineMin(double[] x, double[] y)
        {
            if (x[3] == y[3])
                return 0;
            else if (x[3] < y[3])
                return -1;
            else
                return 1;
        }
    }
}
