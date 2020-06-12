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

namespace Life
{
    public partial class Life : Form
    {
        bool mouseDown;

        int speed = 1;
        Stopwatch stopwatch = new Stopwatch();
        bool[,,] grids = new bool[2, 200, 200];
        int index;

        Bitmap bmp = new Bitmap(200, 200, PixelFormat.Format32bppArgb);

        bool stopRequested;

        public Life()
        {
            InitializeComponent();

            comboWidth.SelectedIndex = 4;
            Render();
        }

        private void CheckRunning_CheckedChanged(object sender, EventArgs e)
        {
            Refresh();
            int num;
            while (checkRunning.Checked && !stopRequested)
            {
                for (int i = 0; i < speed; i++)
                {
                    for (int x = 0; x < 200; x++)
                        for (int y = 0; y < 200; y++)
                        {
                            num = 0;
                            for (int x2 = -1; x2 <= 1; x2++)
                                for (int y2 = -1; y2 <= 1; y2++)
                                    if (x + x2 >= 0 && x + x2 < 200 && y + y2 >= 0 && y + y2 < 200 && grids[index, x + x2, y + y2])
                                        num++;

                            if (num == 5 || num == 4 || num == 6)
                                grids[index ^ 1, x, y] = false;
                            else if (num == 7 || num == 8)
                                grids[index ^ 1, x, y] = true;
                            else
                                grids[index ^ 1, x, y] = grids[index, x, y];
                        }
                    index ^= 1;
                }
                Render();
                Application.DoEvents();
            }
        }

        private void TextSpeed_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textSpeed.Text, out int num) && num >= 0)
                speed = num;
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            DrawAt(e.X, e.Y);
            mouseDown = false;
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
                DrawAt(e.X, e.Y);
        }

        private void DrawAt(int x, int y)
        {
            for (int x2 = -comboWidth.SelectedIndex; x2 <= comboWidth.SelectedIndex; x2++)
                for (int y2 = -comboWidth.SelectedIndex; y2 <= comboWidth.SelectedIndex; y2++)
                    if (x + x2 >= 0 && x + x2 < 200 && y + y2 >= 0 && y + y2 < 200)
                        grids[index, y + y2, x + x2] = true;
            Render();
        }

        private void Render()
        {
            BitmapData bmpData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int length = bmpData.Stride * bmpData.Height;
            byte[] pixels = new byte[length];
            Marshal.Copy(bmpData.Scan0, pixels, 0, length);

            for (int x = 0; x < 200; x++)
                for (int y = 0; y < 200; y++)
                {
                    if (grids[index, x, y])
                    {
                        pixels[(x * 200 + y) * 4] = 255;
                        pixels[(x * 200 + y) * 4 + 1] = 255;
                        pixels[(x * 200 + y) * 4 + 2] = 255;
                        pixels[(x * 200 + y) * 4 + 3] = 255;
                    }
                    else
                    {
                        pixels[(x * 200 + y) * 4] = 0;
                        pixels[(x * 200 + y) * 4 + 1] = 0;
                        pixels[(x * 200 + y) * 4 + 2] = 0;
                        pixels[(x * 200 + y) * 4 + 3] = 255;
                    }
                }

            Marshal.Copy(pixels, 0, bmpData.Scan0, pixels.Length);
            bmp.UnlockBits(bmpData);
            pictureBox1.Image = bmp;
        }

        private void Life_FormClosed(object sender, FormClosedEventArgs e)
        {
            stopRequested = true;
        }
    }
}
