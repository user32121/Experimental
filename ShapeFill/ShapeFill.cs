using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShapeFill
{
    public partial class ShapeFill : Form
    {
        Bitmap bmpDisplay = new Bitmap(400, 400);  //displayed image
        Graphics gDisplay;

        Bitmap bmpMain = new Bitmap(400, 400);  //main layer
        Graphics gMain;

        Bitmap bmpDraw = new Bitmap(400, 400);  //drawing layer
        Graphics gDraw;
        List<System.Drawing.Point> shape = new List<System.Drawing.Point>();
        System.Drawing.Point curPos;
        Pen drawPen = Pens.Black;
        Brush drawBrush = Brushes.Black;

        public ShapeFill()
        {
            InitializeComponent();

            gDisplay = Graphics.FromImage(bmpDisplay);
            gMain = Graphics.FromImage(bmpMain);
            gMain.Clear(Color.White);
            gDraw = Graphics.FromImage(bmpDraw);
            Render();
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            shape.Clear();
            gMain.Clear(Color.White);
            gDraw.Clear(Color.Transparent);
            Render();
        }

        private void ButtonConfirm_Click(object sender, EventArgs e)
        {
            if (shape.Count >= 3)
            {
                //fill shape (scan line method)
                double[,] lines = new double[shape.Count, 4];
                int min = shape[0].X, max = shape[0].X;
                for (int i1 = 0; i1 < shape.Count; i1++)
                {
                    int i2 = (i1 + 1) % shape.Count;
                    lines[i1, 0] = (double)(shape[i1].Y - shape[i2].Y) / (shape[i1].X - shape[i2].X);  //slope
                    lines[i1, 1] = shape[i1].Y - lines[i1, 0] * shape[i1].X;  //y int
                    lines[i1, 2] = shape[i1].X; lines[i1, 3] = shape[i2].X;  //x range

                    min = Math.Min(min, shape[i1].X);
                    max = Math.Max(max, shape[i1].X);
                }

                List<int> yVal = new List<int>();
                for (int x = min; x <= max; x++)
                {
                    for (int l = 0; l < shape.Count; l++)
                        if (x >= lines[l, 2] ^ x >= lines[l, 3])
                            yVal.Add((int)(lines[l, 0] * x + lines[l, 1]));

                    yVal.Sort();
                    for (int y = 0; y < yVal.Count; y += 2)
                        gMain.DrawLine(drawPen, x, yVal[y], x, yVal[y + 1]);
                    yVal.Clear();
                }
            }

            //clear shape
            shape.Clear();
            gDraw.Clear(Color.Transparent);
            Render();
        }
        private void ButtonConfirm_Click2(object sender, EventArgs e)
        {
            if (shape.Count >= 3)
                gMain.FillPolygon(drawBrush, shape.ToArray());
            
            //clear shape
            shape.Clear();
            gDraw.Clear(Color.Transparent);
            Render();
        }

        private void PictureDisplay_Click(object sender, EventArgs e)
        {
            //add point to shape
            System.Drawing.Point pos = pictureDisplay.PointToClient(Cursor.Position);

            if (shape.Count == 0)
                //draw a point
                bmpMain.SetPixel(pos.X, pos.Y, drawPen.Color);
            else
                //draw a line
                gMain.DrawLine(drawPen, shape[shape.Count - 1], pos);

            shape.Add(pos);
            Render();
        }

        private void PictureDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            //move line
            if (shape.Count > 0)
            {
                //erase old line
                DrawLine(Color.Transparent, shape[shape.Count - 1], curPos);
                //draw new line
                curPos = pictureDisplay.PointToClient(Cursor.Position);
                DrawLine(drawPen.Color, shape[shape.Count - 1], curPos);
                Render();
            }
        }

        void DrawLine(Color col, System.Drawing.Point p1, System.Drawing.Point p2)
        {
            //put a line on bmpDraw (overwrite mode)
            int dMax = Math.Max(Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y));
            double x = p1.X, y = p1.Y,
                dx = (double)(p2.X - p1.X) / dMax, dy = (double)(p2.Y - p1.Y) / dMax;
            for (int i = 0; i < dMax; i++)
            {
                x += dx;
                y += dy;
                if (x >= 0 && y < bmpDraw.Width && y >= 0 && y < bmpDraw.Height)
                    bmpDraw.SetPixel((int)Math.Round(x), (int)Math.Round(y), col);
            }
        }

        void Render()
        {
            gDisplay.DrawImage(bmpMain, 0, 0);
            gDisplay.DrawImage(bmpDraw, 0, 0);
            pictureDisplay.Image = bmpDisplay;
        }
    }
}
