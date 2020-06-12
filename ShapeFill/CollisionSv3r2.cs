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
    public partial class CollisionSv3r2 : Form
    {
        Bitmap bmpMain = new Bitmap(400, 400);  //main layer
        Graphics gMain;

        Bitmap bmpDraw = new Bitmap(400, 400);  //drawing layer
        Graphics gDraw;
        List<Vector> drawShape = new List<Vector>();
        Vector curPos;
        Pen drawPen = Pens.Black;
        Brush drawBrush = Brushes.Black;

        List<Pointv1[,]> shapes = new List<Pointv1[,]>();
        List<System.Drawing.Point> centers = new List<System.Drawing.Point>();

        Pointv1 mousePoint;

        const int pointDist = 5;

        public CollisionSv3r2()
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
            Vector pos = pictureDisplay.PointToClient(Cursor.Position).ToVector();

            if (((MouseEventArgs)e).Button == MouseButtons.Left)
            {
                //add point to shape
                drawShape.Add(pos);
                Render();
            }
            else if (((MouseEventArgs)e).Button == MouseButtons.Middle)
            {
                mousePoint = null;
                for (int s = 0; s < shapes.Count; s++)
                    for (int x = 0; x < shapes[s].GetLength(0); x++)
                        for (int y = 0; y < shapes[s].GetLength(1); y++)
                            if (mousePoint == null || shapes[s][x, y] != null && (shapes[s][x, y].pos - pos).Length < (mousePoint.pos - pos).Length)
                                mousePoint = shapes[s][x, y];
            }
            else
                mousePoint = null;
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
                            //gMain.DrawRectangle(drawPen, (float)shapes[i][x, y].pos.X, (float)shapes[i][x, y].pos.Y, pointDist, pointDist);
                            gMain.FillRectangle(drawBrush, (float)shapes[i][x, y].pos.X, (float)shapes[i][x, y].pos.Y, pointDist * 2, pointDist * 2);
            }

            //transfer to display
            pictureDisplay.BackgroundImage = bmpMain;
            pictureDisplay.Image = bmpDraw;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            //move point
            if (mousePoint != null)
            {
                Vector dir = pictureDisplay.PointToClient(Cursor.Position).ToVector() - mousePoint.pos;
                if (dir.LengthSquared > pointDist * pointDist)
                    dir *= pointDist / dir.Length;
                mousePoint.vel += dir * 0.9;
            }

            for (int s = 0; s < shapes.Count; s++)
            {
                //gravity (apply to single point)
                shapes[s][centers[s].X, centers[s].Y].vel.Y += 2;

                for (int x = 0; x < shapes[s].GetLength(0); x++)
                    for (int y = 0; y < shapes[s].GetLength(1); y++)
                        if (shapes[s][x, y] != null)
                        {
                            //collision with other shapes
                            //for (int s2 = 0; s2 < shapes.Count; s2++)
                            //{
                            //    if (s != s2)
                            //        for (int x2 = 0; x2 < shapes[s2].GetLength(0); x2++)
                            //            for (int y2 = 0; y2 < shapes[s2].GetLength(1); y2++)
                            //                if (shapes[s2][x2, y2] != null && !(s == s2 && x == x2 && y == y2))
                            //                {
                            //                    Vector dist = shapes[s][x, y].pos - shapes[s2][x2, y2].pos;
                            //                    if (dist.LengthSquared < pointDist * pointDist / 4)
                            //                    {
                            //                        dist.Normalize();
                            //                        shapes[s][x, y].pos += dist * pointDist / 10;
                            //                        shapes[s2][x2, y2].pos -= dist * pointDist / 10;
                            //                    }
                            //                }
                            //}

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
                            else if (shapes[s][x, y].pos.Y >= bmpMain.Height)
                            {
                                shapes[s][x, y].vel.Y *= 0.5;
                                shapes[s][x, y].pos.Y = bmpMain.Height;
                            }

                            //target pos
                            //right
                            if (x != shapes[s].GetLength(0) - 1 && shapes[s][x + 1, y] != null)
                            {
                                Vector dist = shapes[s][x, y].pos - shapes[s][x + 1, y].pos;
                                dist.Normalize();
                                dist = shapes[s][x + 1, y].pos + dist * pointDist - shapes[s][x, y].pos;
                                shapes[s][x, y].vel += dist / 3;
                                shapes[s][x + 1, y].vel -= dist / 3;
                            }
                            //down
                            if (y != shapes[s].GetLength(1) - 1 && shapes[s][x, y + 1] != null)
                            {
                                Vector dist = shapes[s][x, y].pos - shapes[s][x, y + 1].pos;
                                dist.Normalize();
                                dist = shapes[s][x, y + 1].pos + dist * pointDist - shapes[s][x, y].pos;
                                shapes[s][x, y].vel += dist / 3;
                                shapes[s][x, y + 1].vel -= dist / 3;
                            }
                            //diagonal RD
                            if (x != shapes[s].GetLength(0) - 1 && y != shapes[s].GetLength(1) - 1 && shapes[s][x + 1, y + 1] != null)
                            {
                                Vector dist = shapes[s][x, y].pos - shapes[s][x + 1, y + 1].pos;
                                dist.Normalize();
                                dist = shapes[s][x + 1, y + 1].pos + dist * pointDist * Math.Sqrt(2) - shapes[s][x, y].pos;
                                shapes[s][x, y].vel += dist / 3;
                                shapes[s][x + 1, y + 1].vel -= dist / 3;
                            }
                            //diagonal RU
                            if (x != shapes[s].GetLength(0) - 1 && y != 0 && shapes[s][x + 1, y - 1] != null)
                            {
                                Vector dist = shapes[s][x, y].pos - shapes[s][x + 1, y - 1].pos;
                                dist.Normalize();
                                dist = shapes[s][x + 1, y - 1].pos + dist * pointDist * Math.Sqrt(2) - shapes[s][x, y].pos;
                                shapes[s][x, y].vel += dist / 3;
                                shapes[s][x + 1, y - 1].vel -= dist / 3;
                            }
                        }
                for (int x = 0; x < shapes[s].GetLength(0); x++)
                    for (int y = 0; y < shapes[s].GetLength(1); y++)
                        if (shapes[s][x, y] != null)
                        {
                            //drag
                            shapes[s][x, y].vel *= 0.99f;

                            //translate
                            shapes[s][x, y].pos += shapes[s][x, y].vel;
                        }
            }
            Render();
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
    }
}
