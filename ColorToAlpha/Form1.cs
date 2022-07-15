using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ColorToAlpha
{
    public partial class Form1 : Form
    {
        Color alphaCol;
        Bitmap bmpColDisplay;
        Graphics gColDisplay;

        Bitmap bmpBackground;
        Graphics gBackground;

        Bitmap bmpMain;
        Bitmap bmpProcessed;

        public Form1()
        {
            InitializeComponent();

            bmpColDisplay = new Bitmap(15, 15);
            gColDisplay = Graphics.FromImage(bmpColDisplay);

            UpdateColDisplay(Color.Black);
            UpdateBackgroundImage();
        }

        private void trackThreshold_Scroll(object sender, EventArgs e)
        {
            labelThreshold.Text = string.Format("{0}%", trackThreshold.Value);
            UpdateImage();
        }

        private void pictureColorDisplay_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                UpdateColDisplay(colorDialog1.Color);
                UpdateImage();
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bmpMain = (Bitmap)Bitmap.FromStream(openFileDialog1.OpenFile());
                bmpProcessed = new Bitmap(bmpMain);
                UpdateImage();
                UpdateBackgroundImage();
            }
        }

        private void UpdateColDisplay(Color col)
        {
            alphaCol = col;
            gColDisplay.FillRectangle(new SolidBrush(alphaCol), 0, 0, bmpColDisplay.Width, bmpColDisplay.Height);
            pictureColorDisplay.Image = bmpColDisplay;
        }

        private void UpdateBackgroundImage()
        {
            bmpBackground = new Bitmap(pictureDisplay.Width, pictureDisplay.Height);
            gBackground = Graphics.FromImage(bmpBackground);

            for (int x = 0; x <= pictureDisplay.Width / 10; x++)
                for (int y = 0; y <= pictureDisplay.Height / 10; y++)
                {
                    if ((x + y) % 2 == 0)
                        gBackground.FillRectangle(Brushes.Gray, x * 10, y * 10, 10, 10);
                    else
                        gBackground.FillRectangle(Brushes.LightGray, x * 10, y * 10, 10, 10);
                }
            pictureDisplay.BackgroundImage = bmpBackground;
        }

        private void UpdateImage()
        {
            if (bmpMain == null)
                return;

            for (int x = 0; x < bmpMain.Width; x++)
                for (int y = 0; y < bmpMain.Height; y++)
                {
                    Color c = bmpMain.GetPixel(x, y);
                    double dist = (Math.Abs(c.R - alphaCol.R) + Math.Abs(c.G - alphaCol.G) + Math.Abs(c.B - alphaCol.B)) / (256 * 3.0);
                    double threshold = trackThreshold.Value / 100.0;
                    if (dist < 1 - threshold)
                        bmpProcessed.SetPixel(x, y, Color.FromArgb((int)(dist / (1 - threshold) * 256), c.R, c.G, c.B));
                    else
                        bmpProcessed.SetPixel(x, y, c);
                }
            pictureDisplay.Image = bmpProcessed;
        }
    }
}
