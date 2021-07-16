using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stock
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        Graphics g;
        List<double>[] values = new List<double>[4];
        double[] curValues = new double[4];
        const int spacing = 5;
        Random rng = new Random();
        readonly int midValue;
        readonly Pen[] colors = new Pen[4] { Pens.Black, Pens.Red, Pens.Lime, Pens.Blue };

        public Form1()
        {
            InitializeComponent();

            for (int i = 0; i < values.Length; i++)
                values[i] = new List<double>();

            bmp = new Bitmap(500, 200);
            g = Graphics.FromImage(bmp);
            for (int i = 0; i < curValues.Length; i++)
                curValues[i] = midValue = bmp.Height / 2;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = timer1.Interval.ToString();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int num) && num > 0)
                timer1.Interval = num;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < curValues.Length; i++)
            {
                curValues[i] = NextStockValue(curValues[i]);
                values[i].Add(curValues[i]);
                if (values[i].Count * spacing > bmp.Width)
                    values[i].RemoveAt(0);
            }

            g.Clear(Color.White);
            for (int i = 0; i < values.Length; i++)
                for (int j = 1; j < values[0].Count; j++)
                    g.DrawLine(colors[i], (j - 1) * spacing, bmp.Height - (float)values[i][j - 1], j * spacing, bmp.Height - (float)values[i][j]);

            pictureBox1.Image = bmp;
        }

        double NextStockValue(double curValue)
        {
            double delta = 30 / (Math.Pow(Math.Abs(midValue - curValue), 0.5) + 1);
            //double chance = (bmp.Height - curValue) / bmp.Height / 2 + 0.25;
            double chance = curValue > midValue ? 0.45 : 0.5;
            if (rng.NextDouble() < chance)
                curValue += delta;
            else
                curValue -= delta;
            if (curValue < 0)
                curValue = midValue;
            return curValue;
        }
    }
}
