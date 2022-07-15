using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageStretch
{
    public partial class Form1 : Form
    {
        Bitmap bmpSource, bmpSourcePts, bmpDest, bmpDestPts, bmpPoint;
        Graphics gSourcePts, gDestPts;

        Point[,] sourcePts;  //the array of reference points to use when deriving points
        Point[,] destPts;  //the array of points to drag to deform the image
        (int, int) selectedPoint = (-1, -1);

        int pointAnimationIndex = 0;

        public Form1()
        {
            InitializeComponent();

            bmpPoint = new Bitmap("point.png");
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            pictureBox2.Left = pictureBox1.Right + Math.Max(pictureBox1.Margin.Right, pictureBox2.Margin.Left);
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.BackgroundImage = bmpSource = new Bitmap(openFileDialog1.OpenFile());
                pictureBox2.BackgroundImage = bmpDest = new Bitmap(bmpSource);
                bmpSourcePts = new Bitmap(bmpSource.Width, bmpSource.Height);
                bmpDestPts = new Bitmap(bmpDest.Width, bmpDest.Height);
                gSourcePts = Graphics.FromImage(bmpSourcePts);
                gDestPts = Graphics.FromImage(bmpDestPts);

                RegeneratePoints();
                RenderPoints();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (bmpSource != null)
            {
                pointAnimationIndex++;
                RenderPoints();
            }
        }

        private void RegeneratePoints()
        {
            if (bmpDest == null)
                return;

            sourcePts = new Point[(int)upDownX.Value, (int)upDownY.Value];
            destPts = new Point[(int)upDownX.Value, (int)upDownY.Value];
            for (int i = 0; i < sourcePts.GetLength(0); i++)
                for (int j = 0; j < sourcePts.GetLength(1); j++)
                    sourcePts[i, j] = destPts[i, j] = new Point((bmpDest.Width - 1) * i / (sourcePts.GetLength(0) - 1), (bmpDest.Height - 1) * j / (sourcePts.GetLength(1) - 1));
        }

        private void RenderPoints()
        {
            gSourcePts.Clear(Color.Transparent);
            gDestPts.Clear(Color.Transparent);

            ImageAttributes imgAtt = new ImageAttributes(), imgAttHover = new ImageAttributes();
            imgAtt.SetColorMatrix(new ColorMatrix(
                new float[][] {
                    new float[] { 1, 0, 0, 0, 0},
                    new float[] { 0, 1, 0, 0, 0},
                    new float[] { 0, 0, 1, 0, 0},
                    new float[] { 0, 0, 0, (float)(Math.Cos(pointAnimationIndex/2.0)+3)/4, 0},
                    new float[] { 0, 0, 0, 0, 1}
                }));
            imgAttHover.SetColorMatrix(new ColorMatrix(
                new float[][] {
                    new float[] { 1, 0, 0, 0, 0},
                    new float[] { 0, 1, 0, 0, 0},
                    new float[] { 0, 0, 1, 0, 0},
                    new float[] { 0, 0, 0, (float)(Math.Cos(pointAnimationIndex/2.0)+3)/4, 0},
                    new float[] { 0, 0, 0.8f, 0, 1}
                }));

            for (int i = 0; i < sourcePts.GetLength(0); i++)
                for (int j = 0; j < sourcePts.GetLength(1); j++)
                {
                    gSourcePts.DrawImage(bmpPoint, new Rectangle(sourcePts[i, j] - new Size(bmpPoint.Width / 2, bmpPoint.Height / 2), bmpPoint.Size), 0, 0, bmpPoint.Width, bmpPoint.Height, GraphicsUnit.Pixel, selectedPoint == (i, j) ? imgAttHover : imgAtt);
                    gDestPts.DrawImage(bmpPoint, new Rectangle(destPts[i, j] - new Size(bmpPoint.Width / 2, bmpPoint.Height / 2), bmpPoint.Size), 0, 0, bmpPoint.Width, bmpPoint.Height, GraphicsUnit.Pixel, selectedPoint == (i, j) ? imgAttHover : imgAtt);
                }

            pictureBox1.Image = bmpSourcePts;
            pictureBox2.Image = bmpDestPts;
        }

        private void UpDownX_ValueChanged(object sender, EventArgs e)
        {
            RegeneratePoints();
        }

        private void UpDownY_ValueChanged(object sender, EventArgs e)
        {
            RegeneratePoints();
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (destPts == null)
                return;

            if (e.Button.HasFlag(MouseButtons.Left) && selectedPoint.Item1 != -1)
            {
                destPts[selectedPoint.Item1, selectedPoint.Item2] = e.Location;
                if (destPts[selectedPoint.Item1, selectedPoint.Item2].X < 0)
                    destPts[selectedPoint.Item1, selectedPoint.Item2].X = 0;
                else if (destPts[selectedPoint.Item1, selectedPoint.Item2].X >= bmpDest.Width)
                    destPts[selectedPoint.Item1, selectedPoint.Item2].X = bmpDest.Width - 1;
                if (destPts[selectedPoint.Item1, selectedPoint.Item2].Y < 0)
                    destPts[selectedPoint.Item1, selectedPoint.Item2].Y = 0;
                else if (destPts[selectedPoint.Item1, selectedPoint.Item2].Y >= bmpDest.Height)
                    destPts[selectedPoint.Item1, selectedPoint.Item2].Y = bmpDest.Height - 1;
                //UpdateDestBmp();  //commented out due to lag
            }

            bool success = false;
            for (int i = 0; i < destPts.GetLength(0); i++)
                for (int j = 0; j < destPts.GetLength(1); j++)
                    if (new Rectangle(destPts[i, j] - Scale(bmpPoint.Size, 0.5), bmpPoint.Size).Contains(e.Location))
                    {
                        Cursor = Cursors.Hand;
                        success = true;
                        if (selectedPoint.Item1 == -1)
                            selectedPoint = (i, j);
                        else if (selectedPoint != (i, j) && !e.Button.HasFlag(MouseButtons.Left))
                            success = false;
                    }
            if (!success)
            {
                Cursor = Cursors.Default;
                selectedPoint = (-1, -1);
            }
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            if (selectedPoint.Item1 != -1)
                UpdateDestBmp();
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            selectedPoint = (-1, -1);
        }

        private Size Scale(Size s, double d)
        {
            return new Size((int)(s.Width * d), (int)(s.Height * d));
        }

        private void UpdateDestBmp()
        {
            List<Point> polygon = new List<Point>();
            for (int x = 0; x < destPts.GetLength(0); x++)  //top
                polygon.Add(destPts[x, 0]);
            for (int y = 1; y < destPts.GetLength(1); y++)  //right
                polygon.Add(destPts[destPts.GetLength(0) - 1, y]);
            for (int x = destPts.GetLength(0) - 2; x >= 0; x--)  //bottom; - 2 because first point is already added
                polygon.Add(destPts[x, destPts.GetLength(1) - 1]);
            for (int y = destPts.GetLength(1) - 2; y >= 0; y--)  //left
                polygon.Add(destPts[0, y]);

            List<Point>[,] quads = new List<Point>[destPts.GetLength(0) - 1, destPts.GetLength(1) - 1];

            for (int i = 0; i < destPts.GetLength(0) - 1; i++)
                for (int j = 0; j < destPts.GetLength(1) - 1; j++)
                    quads[i, j] = new List<Point>(5) { destPts[i, j], destPts[i + 1, j], destPts[i + 1, j + 1], destPts[i, j + 1], destPts[i, j] };

            unsafe
            {
                BitmapData bmpDestData = bmpDest.LockBits(new Rectangle(Point.Empty, bmpDestPts.Size), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                int red = Color.Red.ToArgb();
                int lime = Color.Lime.ToArgb();
                int blue = Color.Blue.ToArgb();
                int white = Color.White.ToArgb();

                for (int x = 0; x < bmpDest.Width; x++)
                    for (int y = 0; y < bmpDest.Height; y++)
                    {
                        if (Geometry.inPolygon(new Point(x, y), polygon))
                        {
                            for (int i = 0; i < destPts.GetLength(0) - 1; i++)
                                for (int j = 0; j < destPts.GetLength(1) - 1; j++)
                                    if (Geometry.inPolygon(new Point(x, y), quads[i, j]))
                                    {
                                        //transform position from quad to rect coords to get corresponding pixel
                                        //   a   b   c   d   e   f   g   h
                                        int c0, c1, c2, c3, c4, c5, c6, c7;  //constants in (ax + b) / (cx + d) = (ex + f) / (gx + h)
                                        //   i   j   k
                                        int q0, q1, q2;  //constants in quadratic
                                        float rx, ry;  //ratio from point to edge

                                        c0 = destPts[i + 1, j].X - destPts[i, j].X;
                                        c1 = destPts[i, j].X - x;
                                        c2 = destPts[i + 1, j].Y - destPts[i, j].Y;
                                        c3 = destPts[i, j].Y - y;
                                        c4 = destPts[i + 1, j + 1].X - destPts[i, j + 1].X;
                                        c5 = destPts[i, j + 1].X - x;
                                        c6 = destPts[i + 1, j + 1].Y - destPts[i, j + 1].Y;
                                        c7 = destPts[i, j + 1].Y - y;
                                        q0 = c0 * c6 - c2 * c4;  //a*g - c*e
                                        q1 = c0 * c7 + c1 * c6 - c4 * c3 - c5 * c2;  //a*h + b*g - e*d - f*c
                                        q2 = c1 * c7 - c3 * c5;  //b*h - d*f
                                        if (q0 == 0)  //avoid divide by 0 (when horizontal lines are parallel)
                                            rx = -q2 / (float)q1;  //-k/j
                                        else
                                            rx = ((-q1 + (float)Math.Sqrt(q1 * (long)q1 - 4 * (long)q0 * q2)) / (2 * q0));  //(-j - sqrt(j^2 - 4*i*k)) / 2*i

                                        //calculate coordinates of reference points on horizontal lines
                                        PointF p1, p2;
                                        p1 = new PointF(destPts[i, j].X + (destPts[i + 1, j].X - destPts[i, j].X) * rx, destPts[i, j].Y + (destPts[i + 1, j].Y - destPts[i, j].Y) * rx);
                                        p2 = new PointF(destPts[i, j + 1].X + (destPts[i + 1, j + 1].X - destPts[i, j + 1].X) * rx, destPts[i, j + 1].Y + (destPts[i + 1, j + 1].Y - destPts[i, j + 1].Y) * rx);

                                        //ratio of (x,y) from p1 to p2
                                        if (p1.X - p2.X == 0)  //also prevent divide by 0
                                            ry = (p1.Y - y) / (p1.Y - p2.Y);
                                        else
                                            ry = (p1.X - x) / (p1.X - p2.X);

                                        //finally, get the point in rect coords
                                        Point p3 = new Point((int)(sourcePts[i, j].X + (sourcePts[i + 1, j].X - sourcePts[i, j].X) * rx), (int)(sourcePts[i, j].Y + (sourcePts[i, j + 1].Y - sourcePts[i, j].Y) * ry));
                                        if (p3.X >= 0 && p3.Y >= 0 && p3.X < bmpSource.Width && p3.Y < bmpSource.Height)
                                            *((int*)bmpDestData.Scan0.ToPointer() + x + y * bmpDestData.Stride / 4) = bmpSource.GetPixel(p3.X, p3.Y).ToArgb();
                                        else
                                            *((int*)bmpDestData.Scan0.ToPointer() + x + y * bmpDestData.Stride / 4) = red;

                                        goto FINISHEDQUADITERATION;
                                    }
                                FINISHEDQUADITERATION:;
                        }
                        else
                            *((int*)bmpDestData.Scan0.ToPointer() + x + y * bmpDestData.Stride / 4) = white;
                    }

                bmpDest.UnlockBits(bmpDestData);
            }
        }
    }
}
