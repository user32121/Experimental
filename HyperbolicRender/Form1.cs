using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Point = System.Drawing.Point;

namespace HyperbolicRender
{
    public partial class Form1 : Form
    {
        Bitmap bmp = new Bitmap(500, 500, PixelFormat.Format32bppArgb);
        Graphics g;
        List<Tuple<int, int, KnownColor>> toDraw = new List<Tuple<int, int, KnownColor>>();

        List<Tile> tiles = new List<Tile>();
        int curTile = 0;
        int rotation = 0;
        Random rng = new Random();

        const int KnownColorLength = 174;
        const double scale = 1;

        const bool ignoreTileData = false;  //allows rendering of a plain hyperbolic plane

        public Form1()
        {
            InitializeComponent();

            g = Graphics.FromImage(bmp);

            tiles.Add(new Tile() { color = KnownColor.Black, sides = new int[3] { 3, 2, 1 } });  //0
            tiles.Add(new Tile() { color = KnownColor.Red, sides = new int[3] { 0, 5, 4 } });  //1
            tiles.Add(new Tile() { color = KnownColor.Lime, sides = new int[3] { 0, 7, 6 } });  //2
            tiles.Add(new Tile() { color = KnownColor.Blue, sides = new int[3] { 0, 8, 9 } });  //3
            tiles.Add(new Tile() { color = KnownColor.Yellow, sides = new int[3] { 1, -1, 10 } });  //4
            tiles.Add(new Tile() { color = KnownColor.Cyan, sides = new int[3] { 1, 11, -1 } });  //5
            tiles.Add(new Tile() { color = KnownColor.Magenta, sides = new int[3] { 2, -1, 12 } });  //6
            tiles.Add(new Tile() { color = KnownColor.Pink, sides = new int[3] { 2, 13, -1 } });  //7
            tiles.Add(new Tile() { color = KnownColor.Purple, sides = new int[3] { 3, 14, -1 } });  //8
            tiles.Add(new Tile() { color = KnownColor.WhiteSmoke, sides = new int[3] { 3, -1, 15 } });  //9
            tiles.Add(new Tile() { color = KnownColor.Orange, sides = new int[3] { 4, -1, 14 } });  //10
            tiles.Add(new Tile() { color = KnownColor.Brown, sides = new int[3] { 5, 12, -1 } });  //11
            tiles.Add(new Tile() { color = KnownColor.Green, sides = new int[3] { 6, -1, 11 } });  //12
            tiles.Add(new Tile() { color = KnownColor.Gray, sides = new int[3] { 7, 15, -1 } });  //13
            tiles.Add(new Tile() { color = KnownColor.Goldenrod, sides = new int[3] { 8, 10, -1 } });  //14
            tiles.Add(new Tile() { color = KnownColor.Aquamarine, sides = new int[3] { 9, -1, 13 } });  //15

            Render();
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            if (!ignoreTileData)
            {
                //move
                Point pos = ((MouseEventArgs)e).Location;
                double angle = Math.Atan2(pos.Y - bmp.Height / 2, pos.X - bmp.Width / 2);
                if (angle < 0)
                    angle += Math.PI * 2;
                int index = (int)(angle / Math.PI / 2 * 3);
                int prevTile = curTile;
                curTile = tiles[curTile].sides[(index + rotation) % 3];

                rotation = (Array.IndexOf(tiles[curTile].sides, prevTile) - index + 2) % 3;

                //add more tiles if neccessary
                for (int i = 0; i < 3; i++)
                    if (tiles[curTile].sides[i] == -1)
                    {
                        tiles.Add(new Tile() { color = (KnownColor)rng.Next(1, KnownColorLength), sides = new int[3] { curTile, -1, -1 } });
                        tiles[curTile].sides[i] = tiles.Count - 1;
                        //check if tile connects to other tiles
                        //right vertex
                        int vertexTiles = 0, scanTile = curTile;
                        prevTile = tiles.Count - 1;
                        while (scanTile != -1 && vertexTiles < 5)
                        {
                            int dir = Array.IndexOf(tiles[scanTile].sides, prevTile);
                            vertexTiles++;
                            prevTile = scanTile;
                            scanTile = tiles[scanTile].sides[(dir + 1) % 3];
                        }
                        if (vertexTiles == 5 && scanTile != -1)
                        {
                            int dir = Array.IndexOf(tiles[scanTile].sides, prevTile);
                            tiles[scanTile].sides[(dir + 1) % 3] = tiles.Count - 1;
                            tiles[tiles.Count - 1].sides[2] = scanTile;
                        }
                        //left vertex
                        vertexTiles = 0; scanTile = curTile; prevTile = tiles.Count - 1;
                        while (scanTile != -1 && vertexTiles < 5)
                        {
                            int dir = Array.IndexOf(tiles[scanTile].sides, prevTile);
                            vertexTiles++;
                            prevTile = scanTile;
                            scanTile = tiles[scanTile].sides[(dir + 2) % 3];
                        }
                        if (vertexTiles == 5 && scanTile != -1)
                        {
                            int dir = Array.IndexOf(tiles[scanTile].sides, prevTile);
                            tiles[scanTile].sides[(dir + 2) % 3] = tiles.Count - 1;
                            tiles[tiles.Count - 1].sides[1] = scanTile;
                        }
                    }
            }

            Render();
        }

        void Render()
        {
            const double size = 75;
            const int recursions = 10;

            g.Clear(Color.White);

            Vector p1 = new Vector(Math.Cos(0), Math.Sin(0)) * size;
            Vector p2 = new Vector(Math.Cos(Math.PI * 2 / 3), Math.Sin(Math.PI * 2 / 3)) * size;
            Vector p3 = new Vector(Math.Cos(Math.PI * 4 / 3), Math.Sin(Math.PI * 4 / 3)) * size;

            if (!ignoreTileData)
            {
                //reset render flags
                for (int i = 0; i < tiles.Count; i++)
                    tiles[i].alreadyDrawn = -1;
            }

            Vector circle = FindCircle(p1, p2, Math.Pow(bmp.Width / 2, 2) * p1 / p1.LengthSquared);
            double radius = (p1 - circle).Length;
            g.DrawArc(Pens.Black,
                (float)((circle.X - radius) * scale + bmp.Width / 2), (float)((circle.Y - radius) * scale + bmp.Height / 2),
                (float)(radius * 2 * scale), (float)(radius * 2 * scale),
                (float)(Math.Atan2(p1.Y - circle.Y, p1.X - circle.X) / Math.PI * 180),
                (float)Vector.AngleBetween(p1 - circle, p2 - circle));
            if (ignoreTileData)
                DrawTriangle(0, 0, p2, p1, p3, circle, radius, recursions);
            else
                DrawTriangle(tiles[curTile].sides[rotation], curTile, p1, p2, p3, circle, radius, recursions);

            circle = FindCircle(p2, p3, Math.Pow(bmp.Width / 2, 2) * p2 / p2.LengthSquared);
            radius = (p2 - circle).Length;
            g.DrawArc(Pens.Black,
                (float)((circle.X - radius) * scale + bmp.Width / 2), (float)((circle.Y - radius) * scale + bmp.Height / 2),
                (float)(radius * 2 * scale), (float)(radius * 2 * scale),
                (float)(Math.Atan2(p2.Y - circle.Y, p2.X - circle.X) / Math.PI * 180),
                (float)Vector.AngleBetween(p2 - circle, p3 - circle));
            if (ignoreTileData)
                DrawTriangle(0, 0, p3, p2, p1, circle, radius, recursions);
            else
                DrawTriangle(tiles[curTile].sides[(rotation + 1) % 3], curTile, p2, p3, p1, circle, radius, recursions);

            circle = FindCircle(p1, p3, Math.Pow(bmp.Width / 2, 2) * p1 / p1.LengthSquared);
            radius = (p1 - circle).Length;
            g.DrawArc(Pens.Black,
                (float)((circle.X - radius) * scale + bmp.Width / 2), (float)((circle.Y - radius) * scale + bmp.Height / 2),
                (float)(radius * 2 * scale), (float)(radius * 2 * scale),
                (float)(Math.Atan2(p1.Y - circle.Y, p1.X - circle.X) / Math.PI * 180),
                (float)Vector.AngleBetween(p1 - circle, p3 - circle));
            if (ignoreTileData)
                DrawTriangle(0, 0, p3, p1, p2, circle, radius, recursions);
            else
                DrawTriangle(tiles[curTile].sides[(rotation + 2) % 3], curTile, p3, p1, p2, circle, radius, recursions);

            if (!ignoreTileData)
            {
                toDraw.Add(new Tuple<int, int, KnownColor>((int)((p1.X + p2.X + p3.X) / 3 * scale) + bmp.Width / 2, (int)((p1.Y + p2.Y + p3.Y) / 3 * scale) + bmp.Height / 2, tiles[curTile].color));

                for (int i = 0; i < toDraw.Count; i++)
                    FloodFill(toDraw[i].Item1, toDraw[i].Item2, toDraw[i].Item3);
                toDraw.Clear();

                g.FillEllipse(Brushes.Gold, (float)(bmp.Width / 2 - size / 3), (float)(bmp.Height / 2 - size / 3), (float)size * 2 / 3, (float)size * 2 / 3);
                g.DrawEllipse(Pens.Black, (float)(bmp.Width / 2 - size / 3), (float)(bmp.Height / 2 - size / 3), (float)size * 2 / 3, (float)size * 2 / 3);
            }

            g.DrawEllipse(Pens.Black, (float)(bmp.Width / 2 * (1 - scale)), (float)(bmp.Height / 2 * (1 - scale)), (float)(bmp.Width * scale), (float)(bmp.Height * scale));
            bmp.SetPixel((int)(p1.X * scale + bmp.Width / 2), (int)(p1.Y * scale + bmp.Width / 2), Color.Black);
            bmp.SetPixel((int)(p2.X * scale + bmp.Width / 2), (int)(p2.Y * scale + bmp.Width / 2), Color.Black);
            bmp.SetPixel((int)(p3.X * scale + bmp.Width / 2), (int)(p3.Y * scale + bmp.Width / 2), Color.Black);

            pictureBox1.Image = bmp;
        }

        void DrawTriangle(int index, int prevIndex, Vector p1, Vector p2, Vector otherPoint, Vector circle, double radius, int recursions)
        {
            if (index == -1 || recursions == -1 || tiles[index].alreadyDrawn >= recursions && !ignoreTileData)
                return;

            tiles[index].alreadyDrawn = recursions;

            //given 2 points of the triangle and the point to be inverted, find the 3 point of the triangle
            Vector dir = otherPoint - circle;
            Vector newPoint = radius * radius * dir / dir.LengthSquared + circle;

            //find inverse of new point (relative to containing circle)
            Vector newPointInv = Math.Pow(bmp.Width / 2, 2) * newPoint / newPoint.LengthSquared;
            Vector newCircle = FindCircle(p1, newPoint, newPointInv);
            double newRadius = (newPoint - newCircle).Length;

            //draw left side and continue recursion
            //g.DrawEllipse(Pens.LightBlue,
            //    (float)((newCircle.X - newRadius) * scale + bmp.Width / 2), (float)((newCircle.Y - newRadius) * scale + bmp.Height / 2),
            //    (float)(newRadius * 2 * scale), (float)(newRadius * 2 * scale));
            g.DrawArc(Pens.Black,
                (float)((newCircle.X - newRadius) * scale + bmp.Width / 2), (float)((newCircle.Y - newRadius) * scale + bmp.Height / 2),
                (float)(newRadius * 2 * scale), (float)(newRadius * 2 * scale),
                (float)(Math.Atan2(p1.Y - newCircle.Y, p1.X - newCircle.X) / Math.PI * 180),
                (float)Vector.AngleBetween(p1 - newCircle, newPoint - newCircle));

            int sideIndex = -1;
            if (ignoreTileData)
                DrawTriangle(0, 0, p1, newPoint, p2, newCircle, newRadius, recursions - 1);
            else
            {
                //find previous triangle
                for (int i = 0; i < tiles[index].sides.Length; i++)
                    if (tiles[index].sides[i] == prevIndex)
                        sideIndex = i;
                if (sideIndex == -1)
                    Console.WriteLine("Issue with " + index + " connecting to " + prevIndex);
                else
                    DrawTriangle(tiles[index].sides[(sideIndex + 1) % 3], index, p1, newPoint, p2, newCircle, newRadius, recursions - 1);
            }

            //draw right side and continue recursion
            newCircle = FindCircle(p2, newPoint, newPointInv);
            newRadius = (newPoint - newCircle).Length;

            //g.DrawEllipse(Pens.Lime,
            //    (float)((newCircle.X - newRadius) * scale + bmp.Width / 2), (float)((newCircle.Y - newRadius) * scale + bmp.Height / 2),
            //    (float)(newRadius * 2 * scale), (float)(newRadius * 2 * scale));
            g.DrawArc(Pens.Black,
                (float)((newCircle.X - newRadius) * scale + bmp.Width / 2), (float)((newCircle.Y - newRadius) * scale + bmp.Height / 2),
                (float)(newRadius * 2 * scale), (float)(newRadius * 2 * scale),
                (float)(Math.Atan2(p2.Y - newCircle.Y, p2.X - newCircle.X) / Math.PI * 180),
                (float)Vector.AngleBetween(p2 - newCircle, newPoint - newCircle));
            if (ignoreTileData)
                DrawTriangle(0, 0, newPoint, p2, p1, newCircle, newRadius, recursions - 1);
            else
                DrawTriangle(tiles[index].sides[(sideIndex + 2) % 3], index, newPoint, p2, p1, newCircle, newRadius, recursions - 1);

            if (!ignoreTileData)
                toDraw.Add(new Tuple<int, int, KnownColor>((int)((p1.X + p2.X + newPoint.X) / 3 * scale) + bmp.Width / 2, (int)((p1.Y + p2.Y + newPoint.Y) / 3 * scale) + bmp.Height / 2, tiles[index].color));
        }

        void FloodFill(int x, int y, KnownColor col)
        {
            Color drawingColor = Color.FromKnownColor(col);
            const int white = -1;
            if (drawingColor.ToArgb() == white)
                return;

            Queue<Point> points = new Queue<Point>();
            points.Enqueue(new Point(x, y));
            Point t;

            while (points.Count > 0 && points.Count < 1000)
            {
                t = points.Dequeue();
                if (bmp.GetPixel(t.X, t.Y).ToArgb() != white)
                    continue;

                //set pixel
                bmp.SetPixel(t.X, t.Y, drawingColor);

                //check for neighbor pixels
                if (t.X > 0)
                    points.Enqueue(new Point(t.X - 1, t.Y));
                if (t.X < bmp.Width - 1)
                    points.Enqueue(new Point(t.X + 1, t.Y));
                if (t.Y > 0)
                    points.Enqueue(new Point(t.X, t.Y - 1));
                if (t.Y < bmp.Height - 1)
                    points.Enqueue(new Point(t.X, t.Y + 1));
            }
        }

        Vector FindCircle(Vector p1, Vector p2, Vector p3)
        {
            //find circle that passes through points
            //find perpendicular bisectors of lines
            Vector mid1 = (p1 + p2) / 2;
            Vector dir1 = new Vector(p1.Y - p2.Y, p2.X - p1.X);
            Vector mid2 = (p2 + p3) / 2;
            Vector dir2 = new Vector(p2.Y - p3.Y, p3.X - p2.X);
            //find intersection of bisectors
            double t1 = ((mid1.X - mid2.X) * dir2.Y + (mid2.Y - mid1.Y) * dir2.X) / (dir1.Y * dir2.X - dir1.X * dir2.Y);
            return mid1 + dir1 * t1;
        }
    }
}
