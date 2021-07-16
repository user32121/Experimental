using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MondrianArt
{
    public partial class Form1 : Form
    {
        const int delay = 100;  //delay between steps: [0, infinity)
        const double initContChance = 2;  //inital chance to continuing diving: [0, 1]
        const double contChanceDecay = 0.75;  //decay in the chance: [0, 1]
        const double squareFactor = 0.8;  //how square the rectangles will tend to be: [0, 1]

        Bitmap bmp;
        Graphics g;
        Random rng = new Random();

        static readonly Brush[] colors = new Brush[] { Brushes.White, Brushes.White, Brushes.White, Brushes.White, Brushes.White, Brushes.White, Brushes.Red, Brushes.Blue, Brushes.Yellow, };

        public Form1()
        {
            InitializeComponent();

            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);
        }

        private void ButtonGen_Click(object sender, EventArgs e)
        {
            buttonGen.BackColor = Color.Lime;
            Rectangle rect = new Rectangle(Point.Empty, bmp.Size);

            g.FillRectangle(Brushes.Gray, rect);

            ProcessRegion(rect, initContChance);

            pictureBox1.Image = bmp;
            buttonGen.BackColor = SystemColors.Control;
        }

        private void ProcessRegion(Rectangle region, double contChance)
        {
            //pause and update display
            pictureBox1.Image = bmp;
            Refresh();
            Thread.Sleep(delay);

            if (rng.NextDouble() < contChance)
            {
                //choose split location and direction
                double splitPos = 0.2 + rng.NextDouble() * 0.6;
                bool isSplitVert = rng.NextDouble() < squareFactor ^ region.Width > region.Height;

                if (isSplitVert)
                {
                    Rectangle r1 = region;
                    r1.Height = (int)(r1.Height * splitPos);

                    Rectangle r2 = region;
                    r2.Height -= r1.Height;
                    r2.Y += r1.Height;

                    g.DrawRectangle(Pens.Black, r1);
                    g.DrawRectangle(Pens.Black, r2);

                    //update split regions
                    ProcessRegion(r1, contChance * contChanceDecay);
                    ProcessRegion(r2, contChance * contChanceDecay);
                }
                else
                {
                    Rectangle r1 = region;
                    r1.Width = (int)(r1.Width * splitPos);

                    Rectangle r2 = region;
                    r2.Width -= r1.Width;
                    r2.X += r1.Width;

                    g.DrawRectangle(Pens.Black, r1);
                    g.DrawRectangle(Pens.Black, r2);

                    //update split regions
                    ProcessRegion(r1, contChance * contChanceDecay);
                    ProcessRegion(r2, contChance * contChanceDecay);
                }
            }
            else
            {
                //choose color and exit
                region.X++;
                region.Y++;
                region.Width--;
                region.Height--;
                g.FillRectangle(colors[rng.Next(colors.Length)], region);
            }
        }
    }
}
