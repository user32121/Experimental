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
    public partial class CollisionSv2r0 : Form
    {
        Bitmap bmpMain = new Bitmap(400, 400);  //main layer
        Graphics gMain;

        Bitmap bmpDraw = new Bitmap(400, 400);  //drawing layer
        Graphics gDraw;
        List<Vector> drawShape = new List<Vector>();
        Vector curPos;
        Pen drawPen = Pens.Black;

        List<Pointv0[]> shapes = new List<Pointv0[]>();

        public CollisionSv2r0()
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
            if (drawShape.Count > 0)
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
                switch (shapes[i].Length)
                {
                    case 0:
                        break;
                    case 1:
                        //single point
                        if (shapes[i][0].pos.X >= 0 && shapes[i][0].pos.X < bmpMain.Width &&
                            shapes[i][0].pos.Y >= 0 && shapes[i][0].pos.Y < bmpMain.Height)
                            bmpMain.SetPixel((int)shapes[i][0].pos.X, (int)shapes[i][0].pos.Y, drawPen.Color);
                        break;
                    case 2:
                        //line
                        gMain.DrawLine(drawPen, shapes[i][0].pos.ToPointF(), shapes[i][1].pos.ToPointF());
                        break;
                    default:
                        //fill shape (scan line method)
                        double[,] lines = new double[shapes[i].Length, 4];
                        int min = (int)shapes[i][0].pos.X, max = (int)shapes[i][0].pos.X;
                        for (int i1 = 0; i1 < shapes[i].Length; i1++)
                        {
                            int i2 = (i1 + 1) % shapes[i].Length;
                            lines[i1, 0] = (shapes[i][i1].pos.Y - shapes[i][i2].pos.Y) / (shapes[i][i1].pos.X - shapes[i][i2].pos.X);  //slope
                            lines[i1, 1] = shapes[i][i1].pos.Y - lines[i1, 0] * shapes[i][i1].pos.X;  //y int
                            lines[i1, 2] = shapes[i][i1].pos.X; lines[i1, 3] = shapes[i][i2].pos.X;  //x range

                            min = Math.Min(min, (int)shapes[i][i1].pos.X);
                            max = Math.Max(max, (int)shapes[i][i1].pos.X);
                        }
                        //limit to bounds
                        min = Math.Max(min, 0);
                        max = Math.Min(max, bmpMain.Width);

                        List<int> yVal = new List<int>();
                        for (int x = min; x <= max; x++)
                        {
                            for (int l = 0; l < shapes[i].Length; l++)
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

            for (int s = 0; s < shapes.Count; s++)
            {
                //gravity
                shapes[s][0].vel.Y += 1f;

                for (int p = 0; p < shapes[s].Length; p++)
                {

                    //collision detection
                    for (int s2 = 0; s2 < shapes.Count; s2++)
                        if (s != s2)
                        {
                            //convert shape into lines
                            double[,] lines = new double[shapes[s2].Length, 4];
                            int min = (int)shapes[s2][0].pos.X, max = (int)shapes[s][0].pos.X;
                            for (int i1 = 0; i1 < shapes[s2].Length; i1++)
                            {
                                int i2 = (i1 + 1) % shapes[s2].Length;
                                lines[i1, 0] = (shapes[s2][i1].pos.Y - shapes[s2][i2].pos.Y) / (shapes[s2][i1].pos.X - shapes[s2][i2].pos.X);  //slope
                                if (double.IsInfinity(lines[i1, 0]))
                                    lines[i1, 0] = Math.Sign(lines[i1, 0]) * 1.0E9;
                                lines[i1, 1] = shapes[s2][i1].pos.Y - lines[i1, 0] * shapes[s2][i1].pos.X;  //y int
                                lines[i1, 2] = shapes[s2][i1].pos.X; lines[i1, 3] = shapes[s2][i2].pos.X;  //x range

                                min = Math.Min(min, (int)shapes[s2][i1].pos.X);
                                max = Math.Max(max, (int)shapes[s2][i1].pos.X);
                            }

                            //check if point is within other shape's boundary
                            List<(Vector, int)> inters = new List<(Vector, int)>();
                            bool inside = false;
                            if (shapes[s][p].pos.X >= min && shapes[s][p].pos.X <= max)
                            {
                                int below = -1;
                                for (int l = 0; l < lines.GetLength(0); l++)
                                {
                                    if (shapes[s][p].pos.X >= lines[l, 2] ^ shapes[s][p].pos.X >= lines[l, 3])
                                    {
                                        if (below == -1)
                                            below = shapes[s][p].pos.Y < lines[l, 0] * shapes[s][p].pos.X + lines[l, 1] ? 1 : 0;
                                        else
                                         //find when point changes from below lines to above lines
                                         if (below == 1 ^ shapes[s][p].pos.Y < lines[l, 0] * shapes[s][p].pos.X + lines[l, 1])
                                            inside = true;

                                        double a2 = -1 / lines[l, 0];
                                        if (double.IsInfinity(a2))
                                            a2 = Math.Sign(a2) * 1.0E9;
                                        double b2 = shapes[s][p].pos.Y - a2 * shapes[s][p].pos.X;
                                        double x = ((b2 - lines[l, 1]) / (lines[l, 0] - a2));
                                        inters.Add((new Vector(x, a2 * x + b2), l));
                                        (Vector, int) intersection = inters[inters.Count - 1];
                                        intersection.Item1 -= shapes[s][p].pos;
                                        intersection.Item1 *= 0.5f;
                                        inters[inters.Count - 1] = intersection;
                                    }
                                }
                                if (inside)
                                {
                                    //find smallest intersection
                                    inters.Sort(new Comparison<(Vector, int)>(CompareLength));

                                    //resolve collision
                                    /*
                                    //normal of collision line
                                    Vector normal2 = shapes[s2][inters[0].Item2].pos -
                                        shapes[s2][(inters[0].Item2 + 1) % shapes[s2].Count].pos;
                                    normal2.Normalize();

                                    //project velocity onto line
                                    double px = (shapes[s].center.X - shapes[s].points[p].X) * normal2.X + (shapes[s].center.Y - shapes[s].points[p].Y) * normal2.Y;
                                    //find normal
                                    Vector normal = new Vector(normal2.Y, normal2.X);
                                    if (shapes[s2].center.Y < lines[inters[0].Item2, 0] * shapes[s2].center.X + lines[inters[0].Item2, 1])
                                        normal.X *= -1;
                                    else
                                        normal.Y *= -1;
                                    //project onto normal
                                    double py = (shapes[s].center.X - shapes[s].points[p].X) * normal.X + (shapes[s].center.Y - shapes[s].points[p].Y) * normal.Y;
                                    double vy = shapes[s][p].vel.X * normal.X + shapes[s][p].vel.Y * normal.Y;

                                    double massRatio = shapes[s].mass / shapes[s2].mass;

                                    //apply force to s
                                    shapes[s].rVel += (Math.Atan2(px, py) * Math.Abs(vy) * rFac * 0.5f * massRatio);

                                    shapes[s].tVel -= new Vector(normal.Copy().Multiply(vy * Math.Abs(px / py) * tFac * 0.5f * massRatio));
                                    shapes[s].Translate(inters[0].Item1.Copy().Multiply(0.5f * massRatio));

                                    //apply force to s2
                                    px = (shapes[s2].center.X - shapes[s].points[p].X) * normal2.X + (shapes[s2].center.Y - shapes[s].points[p].Y) * normal2.Y;
                                    py = (shapes[s2].center.X - shapes[s].points[p].X) * normal.X + (shapes[s2].center.Y - shapes[s].points[p].Y) * normal.Y;

                                    shapes[s2].rVel -= (Math.Atan2(px, py) * Math.Abs(vy) * rFac * 0.5f / massRatio);

                                    shapes[s2].tVel += new Vector(normal.Copy().Multiply(vy * Math.Abs(px / py) * tFac * 0.5f / massRatio));
                                    shapes[s2].Translate(inters[0].Item1.Copy().Multiply(-0.5f / massRatio));
                                    */
                                }
                            }
                        }

                    //boundary
                    //if (shapes[s].points[p].X < 0)
                    //{
                    //    PointF pVel = shapes[s].FindVelocity(p);

                    //    shapes[s].rVel += (float)(Math.Atan2(shapes[s].center.Y - shapes[s].points[p].Y,
                    //        shapes[s].center.X - shapes[s].points[p].X) * Math.Abs(pVel.X) * rFac);

                    //    shapes[s].tVel.X -= pVel.X * Math.Abs((shapes[s].center.Y - shapes[s].points[p].Y) /
                    //        (shapes[s].center.X - shapes[s].points[p].X)) * tFac;
                    //    shapes[s].Translate(-shapes[s].points[p].X, 0);
                    //}
                    //else if (shapes[s].points[p].X >= bmpMain.Width)
                    //{
                    //    PointF pVel = shapes[s].FindVelocity(p);

                    //    shapes[s].rVel -= (float)(Math.Atan2(shapes[s].center.Y - shapes[s].points[p].Y,
                    //        shapes[s].center.X - shapes[s].points[p].X) * Math.Abs(pVel.X) * rFac);

                    //    shapes[s].tVel.X -= pVel.X * Math.Abs((shapes[s].center.Y - shapes[s].points[p].Y) /
                    //        (shapes[s].center.X - shapes[s].points[p].X)) * tFac;
                    //    shapes[s].Translate(bmpMain.Width - shapes[s].points[p].X, 0);
                    //}
                    //if (shapes[s].points[p].Y < 0)
                    //{
                    //    PointF pVel = shapes[s].FindVelocity(p);

                    //    shapes[s].rVel -= (float)(Math.Atan2(shapes[s].center.X - shapes[s].points[p].X,
                    //        shapes[s].center.Y - shapes[s].points[p].Y) * Math.Abs(pVel.Y) * rFac);

                    //    shapes[s].tVel.Y -= pVel.Y * Math.Abs((shapes[s].center.X - shapes[s].points[p].X) /
                    //        (shapes[s].center.Y - shapes[s].points[p].Y)) * tFac;
                    //    shapes[s].Translate(0, -shapes[s].points[p].Y);
                    //}
                    //else 
                    if (shapes[s][p].pos.Y >= bmpMain.Height)
                    {
                        shapes[s][p].vel.Y *= 0.5;
                        shapes[s][p].pos.Y = bmpMain.Height;
                    }

                    //target pos
                    Vector vel = (shapes[s][(p - 1 + shapes[s].Length) % shapes[s].Length].pos -
                        shapes[s][p].pos - shapes[s][p].pRelation) * tFac;

                    shapes[s][p].vel += vel;
                    shapes[s][(p - 1 + shapes[s].Length) % shapes[s].Length].vel -= vel;
                }
                for (int p = 0; p < shapes[s].Length; p++)
                {
                    //drag
                    shapes[s][p].vel *= 0.99f;

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
            List<Pointv0> pts = new List<Pointv0>();

            //cut into smaller segments
            Vector prevP = new Vector();
            for (int i = 0; i < points.Length; i++)
            {
                Vector dir = points[i] - points[(i + 1) % points.Length];
                double segments = Math.Ceiling(dir.Length / 5);
                Vector s = dir / segments;
                Vector p = points[i];
                for (int i2 = 0; i2 < segments; i2++, p -= s)
                {
                    pts.Add(new Pointv0() { pos = p, pRelation = prevP - p, });
                    prevP = p;
                }
            }
            pts[0].pRelation = pts[pts.Count - 1].pos - pts[0].pos;

            shapes.Add(pts.ToArray());
        }
    }
}
