using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShapeColorGenerator
{
    public partial class Form1 : Form
    {
        Random rng = new Random();
        const int shapeCount = 10;
        const int maxPts = 5;
        const int minPts = 3;

        public Form1()
        {
            InitializeComponent();
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textCount.Text, out int count))
            {
                MessageBox.Show("There was an issue parsing " + textCount.Text);
                return;
            }

            Bitmap bmp = new Bitmap(128, 128);
            Graphics g = Graphics.FromImage(bmp);

            Directory.CreateDirectory("output");

            for (int i = 0; i < count; i++)
            {
                g.Clear(Color.White);

                int[] cols = new int[shapeCount];
                Point[][] pts = new Point[shapeCount][];

                //generate points and colors
                for (int j = 0; j < shapeCount; j++)
                    cols[j] = rng.Next() % maxHue;
                for (int j = 0; j < shapeCount; j++)
                {
                    pts[j] = new Point[rng.Next(minPts, maxPts + 1)];
                    for (int k = 0; k < pts[j].Length; k++)
                        pts[j][k] = new Point(rng.Next(bmp.Width), rng.Next(bmp.Height));
                }

                //draw lines for x
                for (int j = 0; j < shapeCount ; j++)
                    g.FillPolygon(new SolidBrush(HueToARGB(cols[j])), pts[j]);

                int fileN = rng.Next();
                if (checkXY.Checked)
                {
                    //display and save
                    pictureDisplay.Image = bmp;
                    bmp.Save("output\\" + fileN + "x.png");

                    //clear image
                    g.Clear(Color.White);

                    //draw hue shifted lines for y
                    for (int j = 0; j < shapeCount - 1; j++)
                        g.FillPolygon(new SolidBrush(HueToARGB(cols[j]+256)), pts[j]);

                    //save
                    bmp.Save("output\\" + fileN + "y.png");
                }
                else
                {
                    //display and save
                    pictureDisplay.Image = bmp;
                    bmp.Save("output\\" + fileN + ".png");
                }
            }
        }

        //converts a number from 0 to 1535 into a color
        const int maxHue = 1535;
        private Color HueToARGB(int hue)
        {
            hue %= maxHue;
            if (hue >= 0)
                if (hue < 256)
                    return Color.FromArgb(255, hue, 0);   //G^
                else if (hue < 512)
                    return Color.FromArgb(511 - hue, 255, 0);   //Rv
                else if (hue < 768)
                    return Color.FromArgb(0, 255, hue - 512);   //B^
                else if (hue < 1024)
                    return Color.FromArgb(0, 1023 - hue, 255);   //Gv
                else if (hue < 1280)
                    return Color.FromArgb(hue - 1024, 0, 255);   //R^
                else if (hue < 1536)
                    return Color.FromArgb(255, 0, 1535 - hue);   //Bv
            return Color.Black;
        }
    }
}
