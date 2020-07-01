using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TerrainGeneration
{
    public partial class Form1 : Form
    {
        const int gX = 100, gY = 100;
        TILES[,] grid = new TILES[gX, gY];
        enum TILES
        {
            AIR, DIRT, GRASS, STONE, REDORE, BLUEORE, GREENORE,
            Length
        }

        //chances of getting each tile at a certain layer (interpolate)
        readonly double[][] tileChances = new double[2][] {
            //surface probabilities
            new double[(int)TILES.Length] {
                0.1, 0.57, 0, 0.3, 0.01, 0.01, 0.01,
            },
            //underground probabilities
            new double[(int)TILES.Length] {
                0.01, 0.05, 0, 0.64, 0.1, 0.1, 0.1,
            },
        };
        Random rng = new Random();

        Bitmap bmp = new Bitmap(gX * 5, gY * 5);
        Graphics g;

        public Form1()
        {
            InitializeComponent();

            g = Graphics.FromImage(bmp);

            Generate();
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            Generate();
        }

        void Generate()
        {
            Create();
            Spread();
            Grass();
            Render();
        }

        private void Create()
        {
            //reset grid
            for (int x = 0; x < gX; x++)
                for (int y = 0; y < gY; y++)
                    grid[x, y] = TILES.AIR;

            //select random tiles
            for (int i = 0; i < 15000; i++)
                grid[rng.Next(gX), rng.Next(gY)] = TILES.DIRT;

            //measure pixels in each column
            int[] height = new int[gX];
            int minHeight = 100;
            for (int x = 0; x < gY; x++)
            {
                for (int y = gX - 1; y >= 0; y--)
                    if (grid[x, y] != TILES.AIR)
                        height[x]++;
                minHeight = Math.Min(minHeight, height[x]);
            }

            //smooth columns
            for (int x = 1; x < gX - 1; x++)
                if (Math.Abs(height[x - 1] - height[x]) > 2 && Math.Abs(height[x] - height[x + 1]) > 2)
                {
                    double avg = (height[x - 1] + height[x] + height[x + 1]) / 3.0;
                    int delta = (int)(avg - height[x - 1]);
                    height[x - 1] += delta;
                    delta = (int)(avg - height[x]);
                    height[x] += delta;
                    delta = (int)(avg - height[x + 1]);
                    height[x + 1] += delta;
                }

            //generate tiles
            for (int x = 0; x < gX; x++)
            {
                int y;
                for (y = 0; y < gY - height[x]; y++)
                    grid[x, y] = TILES.AIR;
                for (; y < gY; y++)
                    if (y < gY - minHeight)
                        grid[x, y] = GetTile(rng.NextDouble(), 0);
                    else
                        grid[x, y] = GetTile(rng.NextDouble(), (y - gY + minHeight) / (double)minHeight);
            }
        }

        TILES GetTile(double val, double layer)
        {
            //returns the tile from a random val (0-1) and the given depth layer (0-1)
            int i = 0;
            while (val > 0)
            {
                val -= tileChances[0][i] * (1 - layer) + tileChances[1][i] * layer;
                i++;
            }
            return (TILES)(i - 1);
        }

        private void Spread()
        {
            //spreads the tiles to create clusters so it looks more natural
            for (int i = 0; i < 10000; i++)
            {
                Point pos = new Point(rng.Next(gX), rng.Next(gY));
                grid[(pos.X - 1 + gX) % gX, pos.Y] = grid[pos.X, pos.Y];
                grid[(pos.X + 1) % gX, pos.Y] = grid[pos.X, pos.Y];
                if (pos.Y > 0)
                    grid[pos.X, pos.Y - 1] = grid[pos.X, pos.Y];
                if (pos.Y < gY - 1)
                    grid[pos.X, pos.Y + 1] = grid[pos.X, pos.Y];
            }
        }

        private void Grass()
        {
            for (int x = 0; x < gX; x++)
                for (int y = 0; y < gY; y++)
                    if (grid[x, y] != TILES.AIR)
                    {
                        if (grid[x, y] == TILES.DIRT)
                            grid[x, y] = TILES.GRASS;
                        break;
                    }
        }

        private void Render()
        {
            for (int x = 0; x < grid.GetLength(0); x++)
                for (int y = 0; y < grid.GetLength(1); y++)
                    switch (grid[x, y])
                    {
                        case TILES.AIR:
                            g.FillRectangle(Brushes.White, x * 5, y * 5, 5, 5);
                            break;
                        case TILES.DIRT:
                            g.FillRectangle(Brushes.SaddleBrown, x * 5, y * 5, 5, 5);
                            if (y > 0)
                            {
                                if (x > 0 && grid[x - 1, y] == TILES.GRASS && grid[x, y - 1] == TILES.GRASS)
                                    g.FillRectangle(Brushes.LimeGreen, x * 5, y * 5, 2, 2);
                                if (x < gX - 1 && grid[x + 1, y] == TILES.GRASS && grid[x, y - 1] == TILES.GRASS)
                                    g.FillRectangle(Brushes.LimeGreen, x * 5 + 3, y * 5, 2, 2);
                            }
                            break;
                        case TILES.GRASS:
                            g.FillRectangle(Brushes.SaddleBrown, x * 5, y * 5, 5, 5);
                            g.FillRectangle(Brushes.LimeGreen, x * 5, y * 5, 5, 2);
                            if (y < gY - 1)
                            {
                                if (x > 0 && grid[x - 1, y + 1] == TILES.GRASS)
                                    g.FillRectangle(Brushes.LimeGreen, x * 5, y * 5, 2, 5);
                                if (x < gX - 1 && grid[x + 1, y + 1] == TILES.GRASS)
                                    g.FillRectangle(Brushes.LimeGreen, x * 5 + 3, y * 5, 2, 5);
                            }
                            break;
                        case TILES.STONE:
                            g.FillRectangle(Brushes.Gray, x * 5, y * 5, 5, 5);
                            break;
                        case TILES.REDORE:
                            g.FillRectangle(Brushes.Gray, x * 5, y * 5, 5, 5);
                            for (int i = 0; i < 10; i++)
                                bmp.SetPixel(x * 5 + rng.Next(5), y * 5 + rng.Next(5), Color.Red);
                            break;
                        case TILES.BLUEORE:
                            g.FillRectangle(Brushes.Gray, x * 5, y * 5, 5, 5);
                            for (int i = 0; i < 10; i++)
                                bmp.SetPixel(x * 5 + rng.Next(5), y * 5 + rng.Next(5), Color.Blue);
                            break;
                        case TILES.GREENORE:
                            g.FillRectangle(Brushes.Gray, x * 5, y * 5, 5, 5);
                            for (int i = 0; i < 10; i++)
                                bmp.SetPixel(x * 5 + rng.Next(5), y * 5 + rng.Next(5), Color.LimeGreen);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
            pictureBox1.Image = bmp;
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            //placeholder for testing functions

        }
    }
}
