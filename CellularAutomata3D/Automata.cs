using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CellularAutomata3D
{
    public partial class Automata : Form
    {
        Bitmap bmp = new Bitmap(1000, 1000);
        Graphics g;

        readonly Brush colTop = Brushes.LightBlue;
        readonly Brush colLeft = Brushes.CornflowerBlue;
        readonly Brush colRight = Brushes.DarkBlue;

        bool[,,,] grid = new bool[2, size, size, size];
        int gridIndex;
        readonly SIDE[,] triangleScans = new SIDE[2, 3] {
            { SIDE.TOP, SIDE.LEFT, SIDE.RIGHT },
            { SIDE.LEFT, SIDE.RIGHT, SIDE.TOP }, };
        //[even/odd, side]
        enum SIDE { X = 0, Y = 1, Z = 2, RIGHT = 0, TOP = 1, LEFT = 2 }

        const int size = 60;
        const double spawnChance = 0.3;
        Random rng = new Random();

        bool enabled = false;
        bool stopRequested = false;

        public Automata()
        {
            InitializeComponent();

            g = Graphics.FromImage(bmp);
            //OnKeyPress(new KeyPressEventArgs('r'));

            //conway glider
            //grid[0, 1, 0, 0] = true;
            //grid[0, 1, 0, 1] = true;
            //grid[0, 2, 1, 0] = true;
            //grid[0, 2, 1, 1] = true;
            //grid[0, 0, 2, 0] = true;
            //grid[0, 0, 2, 1] = true;
            //grid[0, 1, 2, 0] = true;
            //grid[0, 1, 2, 1] = true;
            //grid[0, 2, 2, 0] = true;
            //grid[0, 2, 2, 1] = true;

            //4555 glider
            grid[0, 3, 5, 3] = true;
            grid[0, 3, 5, 4] = true;
            grid[0, 3, 3, 2] = true;
            grid[0, 3, 4, 2] = true;
            grid[0, 3, 3, 5] = true;
            grid[0, 3, 4, 5] = true;
            grid[0, 4, 3, 3] = true;
            grid[0, 4, 4, 3] = true;
            grid[0, 4, 3, 4] = true;
            grid[0, 4, 4, 4] = true;

            Render();
        }

        private void pictureDisplay_Click(object sender, EventArgs e)
        {
            enabled = !enabled;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            while (!stopRequested)
            {
                if (enabled)
                {
                    Calculate();
                    Render();
                }
                Application.DoEvents();
            }
        }

        void Calculate()
        {
            gridIndex = 1 ^ gridIndex;

            int count;
            //move cells
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    for (int z = 0; z < size; z++)
                    {
                        count = 0;
                        for (int x2 = -1; x2 <= 1; x2++)
                            for (int y2 = -1; y2 <= 1; y2++)
                                for (int z2 = -1; z2 <= 1; z2++)
                                    if (x + x2 >= 0 && x + x2 < size && y + y2 >= 0 && y + y2 < size && z + z2 >= 0 && z + z2 < size && grid[gridIndex ^ 1, x + x2, y + y2, z + z2])
                                        count++;

                        //define rules
                        if (/*count == 6*/ count == 5)
                            grid[gridIndex, x, y, z] = true;
                        else if (/*count >= 6 && count <= 8*/ count == 6)
                            grid[gridIndex, x, y, z] = grid[gridIndex ^ 1, x, y, z];
                        else
                            grid[gridIndex, x, y, z] = false;
                    }
        }

        void Render()
        {
            g.Clear(Color.White);

            //stepping through grid method (triangles)
            int x, y, z, x2, y2, z2;
            //diagonal grid u,v
            for (int u = 0; u < size * 2; u++)
            {
                if (u < size)
                {
                    x = z = 0;
                    y = u;
                }
                else
                {
                    //break;
                    y = size - 1;
                    z = 0;
                    x = u - size;
                }
                for (int v = 0; v < size * 2 + 1 + 2 * (int)(size - Math.Abs(size - u - 0.5)); v++)
                {
                    x2 = x; y2 = y; z2 = z;

                    //pick render triangle color
                    SIDE side;
                    if (v < 1 + 2 * (int)(size - Math.Abs(size - u - 0.5)))
                        side = SIDE.LEFT;
                    else if (u < size)
                        side = SIDE.TOP;
                    else
                        side = SIDE.RIGHT;
                    Brush triBrush = Brushes.Transparent;
                    while (true)
                    {
                        if (grid[gridIndex, x2, y2, z2])
                        {
                            switch (side)
                            {
                                case SIDE.RIGHT:
                                    triBrush = colRight;
                                    break;
                                case SIDE.TOP:
                                    triBrush = colTop;
                                    break;
                                case SIDE.LEFT:
                                    triBrush = colLeft;
                                    break;
                            }
                            goto TRIANGLEDRAW;
                        }
                        switch (side = triangleScans[(v + (u < size ? 0 : 1)) % 2, (int)side])
                        {
                            case SIDE.X:
                                if (x2 > 0)
                                    x2--;
                                else
                                    goto TRIANGLEDONE;
                                break;
                            case SIDE.Y:
                                if (y2 < size - 1)
                                    y2++;
                                else
                                    goto TRIANGLEDONE;
                                break;
                            case SIDE.Z:
                                if (z2 < size - 1)
                                    z2++;
                                else
                                    goto TRIANGLEDONE;
                                break;
                        }
                    }
                TRIANGLEDRAW:
                    //draw triangle
                    //triBrush = Brushes.Lime;
                    //triBrush = Brushes.Blue;
                    if (v % 2 == (u < size ? 0 : 1))
                        g.FillPolygon(triBrush, new PointF[] {
                                new PointF(bmp.Width * (v + 2 + Math.Max((u-size)*2+1,0)) / size / 4, (size - v / 2 + u * 2 - Math.Max(u-size+1,0)) * bmp.Height / size / 4),
                                new PointF(bmp.Width * (v + Math.Max((u-size)*2+1,0)) / size / 4, (size - v / 2 + u * 2 - 1 - Math.Max(u-size+1,0)) * bmp.Height / size / 4),
                                new PointF(bmp.Width * (v + Math.Max((u-size)*2+1,0)) / size / 4, (size - v / 2 + u * 2 + 1 - Math.Max(u-size+1,0)) * bmp.Height / size / 4), });
                    else
                        g.FillPolygon(triBrush, new PointF[] {
                                new PointF(bmp.Width * (v - 1 + Math.Max((u-size)*2+1,0)) / size / 4, (size - v / 2 + u * 2 - 1 - Math.Max(u-size,0)) * bmp.Height / size / 4),
                                new PointF(bmp.Width * (v + 1 + Math.Max((u-size)*2+1,0)) / size / 4, (size - v / 2 + u * 2 - 2 - Math.Max(u-size,0)) * bmp.Height / size / 4),
                                new PointF(bmp.Width * (v + 1 + Math.Max((u-size)*2+1,0)) / size / 4, (size - v / 2 + u * 2 - Math.Max(u-size,0)) * bmp.Height / size / 4), });

                    //pictureDisplay.Image = bmp;
                    //Application.DoEvents(); ;

                    TRIANGLEDONE:
                    //step next v
                    if (u < size)
                    {
                        if (v % 2 == 0)
                            if (y == 0)
                            {
                                if (v / 2 != u)
                                    z++;
                            }
                            else
                                y--;
                        else
                          if (y != 0 || (v + 1) / 2 == u)
                            x++;
                    }
                    else
                        if (v % 2 == 0)
                        if (x == size - 1)
                        {
                            if (v / 2 != size * 2 - u - 1)
                                z++;
                        }
                        else
                            x++;
                    else
                            if (x != size - 1 || (v + 1) / 2 == size * 2 - u - 1)
                        y--;
                }
            }

            pictureDisplay.Image = bmp;
        }

        private void Automata_FormClosed(object sender, FormClosedEventArgs e)
        {
            stopRequested = true;
        }

        private void Automata_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void Automata_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'r')
                for (int x = 0; x < size; x++)
                    for (int y = 0; y < size; y++)
                        for (int z = 0; z < size; z++)
                            if (rng.NextDouble() < spawnChance)
                                grid[gridIndex, x, y, z] = true;
        }
    }
}
