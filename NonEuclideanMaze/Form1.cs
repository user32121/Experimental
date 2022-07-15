using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NonEuclideanMaze
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        Graphics g;

        Tile current;

        Random rng = new Random();

        //animation
        enum DIR
        {
            NONE,
            XN,
            XP,
            YN,
            YP,
        }
        DIR dir = DIR.NONE;
        int speed = 10;
        int offsetX;
        int offsetY;

        public Form1()
        {
            InitializeComponent();

            bmp = new Bitmap(500, 500);
            g = Graphics.FromImage(bmp);

            current = new Tile();
            InitTile(current, 1);
            Render();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (dir != DIR.NONE)
                return;
            else if (e.KeyCode == Keys.Up && current.yn != null)
                dir = DIR.YN;
            else if (e.KeyCode == Keys.Down && current.yp != null)
                dir = DIR.YP;
            else if (e.KeyCode == Keys.Left && current.xn != null)
                dir = DIR.XN;
            else if (e.KeyCode == Keys.Right && current.xp != null)
                dir = DIR.XP;
        }

        private void InitTile(Tile t, double probability)
        {
            if (t.xn == null && rng.NextDouble() < probability)
                t.xn = new Tile() { xp = t };
            if (t.xp == null && rng.NextDouble() < probability)
                t.xp = new Tile() { xn = t };
            if (t.yn == null && rng.NextDouble() < probability)
                t.yn = new Tile() { yp = t };
            if (t.yp == null && rng.NextDouble() < probability)
                t.yp = new Tile() { yn = t };
            t.state = Tile.INITALIZATIONSTATE.INITIALIZED;
        }

        private void Render()
        {
            g.Clear(Color.DarkGray);

            //draw tiles
            DrawTile(current, 200 + offsetX, 200 + offsetY, 2);  //don't go above depth 2 since it breaks illusion

            g.FillEllipse(Brushes.Gray, 240, 240, 20, 20);
            pictureBox1.Image = bmp;
        }

        private void DrawTile(Tile t, int left, int top, int depth)
        {
            if (depth == 0)
                return;

            if (t.state == Tile.INITALIZATIONSTATE.UNINITIALIZED)
                InitTile(t, 0.5);

            g.FillRectangle(Brushes.White, left, top, 100, 100);

            //draw corners
            //TL
            g.FillRectangle(Brushes.Black, left - 5, top - 5, 10, 10);
            //TR
            g.FillRectangle(Brushes.Black, left + 95, top - 5, 10, 10);
            //BL
            g.FillRectangle(Brushes.Black, left - 5, top + 95, 10, 10);
            //BR
            g.FillRectangle(Brushes.Black, left + 95, top + 95, 10, 10);

            //left
            if (t.xn == null)
                g.FillRectangle(Brushes.Black, left - 5, top - 5, 10, 110);
            else
                DrawTile(t.xn, left - 100, top, depth - 1);
            //right
            if (t.xp == null)
                g.FillRectangle(Brushes.Black, left + 95, top - 5, 10, 110);
            else
                DrawTile(t.xp, left + 100, top, depth - 1);
            //up
            if (t.yn == null)
                g.FillRectangle(Brushes.Black, left - 5, top - 5, 110, 10);
            else
                DrawTile(t.yn, left, top - 100, depth - 1); 
            //down
            if (t.yp == null)
                g.FillRectangle(Brushes.Black, left - 5, top + 95, 110, 10);
            else
                DrawTile(t.yp, left, top + 100, depth - 1);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (dir)
            {
                case DIR.NONE:
                    return;
                case DIR.XN:
                    offsetX += speed;
                    if (offsetX >= 100)
                    {
                        dir = DIR.NONE;
                        offsetX = 0;
                        current = current.xn;
                    }
                    break;
                case DIR.XP:
                    offsetX -= speed;
                    if (offsetX <= -100)
                    {
                        dir = DIR.NONE;
                        offsetX = 0;
                        current = current.xp;
                    }
                    break;
                case DIR.YN:
                    offsetY += speed;
                    if (offsetY >= 100)
                    {
                        dir = DIR.NONE;
                        offsetY = 0;
                        current = current.yn;
                    }
                    break;
                case DIR.YP:
                    offsetY -= speed;
                    if (offsetY <= -100)
                    {
                        dir = DIR.NONE;
                        offsetY = 0;
                        current = current.yp;
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
            Render();
        }
    }
}
