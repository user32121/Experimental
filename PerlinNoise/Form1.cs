using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerlinNoise
{
    public struct Vector
    {
        public double angle, magnitude, x, y;
        public Vector(double angle, double magnitude)
        {
            this.angle = angle;
            this.magnitude = magnitude;
            this.x = magnitude * Math.Cos(angle);
            this.y = magnitude * Math.Sin(angle);
        }
        public void setAngle(double a)
        {
            angle = a;
            this.x = magnitude * Math.Cos(angle);
            this.y = magnitude * Math.Sin(angle);
        }
    }

    public partial class Form1 : Form
    {
        Bitmap bmp = new Bitmap(400, 400);
        const int w = 15, h = 15;
        Vector[,] vecs = new Vector[w + 1, h + 1];
        Random rng = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void generateNoise(bool randomizeVecs)
        {
            //variables
            double scaleX = bmp.Width / w;
            double scaleY = bmp.Height / h;

            //grid definition
            if (randomizeVecs)
                for (int i = 0; i <= w; i++)
                    for (int j = 0; j <= h; j++)
                    {
                        vecs[i, j] = new Vector(rng.NextDouble() * Math.PI * 2, 1);
                    }

            //iterate over pixels
            for (int x = 0; x < bmp.Width; x++)
                for (int y = 0; y < bmp.Height; y++)
                {
                    //normalize space
                    double normX = x / scaleX;
                    double normY = y / scaleY;

                    //find nearest corners
                    int i = (int)normX;
                    int j = (int)normY;
                    if (i >= w)
                        i = w - 1;
                    if (j >= h)
                        j = h - 1;

                    //offset vectors
                    double distL = normX - i;
                    double distR = -(i + 1 - normX);
                    double distT = normY - j;
                    double distB = -(j + 1 - normY);

                    //dot product
                    double dotTL = distL * vecs[i, j].x + distT * vecs[i, j].y;
                    double dotTR = distR * vecs[i + 1, j].x + distT * vecs[i + 1, j].y;
                    double dotBL = distL * vecs[i, j + 1].x + distB * vecs[i, j + 1].y;
                    double dotBR = distR * vecs[i + 1, j + 1].x + distB * vecs[i + 1, j + 1].y;

                    //clamp
                    //dotTR = Math.Max(Math.Min(dotTR, 1), -1);
                    //dotTL = Math.Max(Math.Min(dotTL, 1), -1);
                    //dotBR = Math.Max(Math.Min(dotBR, 1), -1);
                    //dotBL = Math.Max(Math.Min(dotBL, 1), -1);

                    //interpolation
                    //bilinear
                    //double inter1 = (dotTR - dotTL) * distL + dotTL;
                    //double inter2 = (dotBR - dotBL) * distL + dotBL;
                    //double interFinal = (inter2 - inter1) * distT + inter1;
                    //bicubic    (a1 - a0) * (3.0 - w * 2.0) * w * w + a0;
                    //
                    //smoother bicubic
                    double inter1 = (dotTR - dotTL) * ((distL * (distL * 6.0 - 15.0) + 10.0) * distL * distL * distL) + dotTL;
                    double inter2 = (dotBR - dotBL) * ((distL * (distL * 6.0 - 15.0) + 10.0) * distL * distL * distL) + dotBL;
                    double inter3 = (inter2 - inter1) * ((distT * (distT * 6.0 - 15.0) + 10.0) * distT * distT * distT) + inter1;

                    //scale
                    int scaled = Math.Min(Math.Max((int)((inter3 + 1) * 128), 0), 255);
                    bmp.SetPixel(x, y, Color.FromArgb(scaled, scaled, scaled));
                }

            pictureBox1.Image = bmp;
        }

        private void buttonGen_Click(object sender, EventArgs e)
        {
            generateNoise(true);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = checkBox1.Checked;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //move vectors
            for (int i = 0; i <= w; i++)
                for (int j = 0; j <= h; j++)
                {
                    vecs[i, j].setAngle(vecs[i, j].angle + 0.1);
                }

            generateNoise(false);
        }
    }
}
