using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProceduralPathways
{
    public partial class SubtractionMethod : Form
    {
        const int gridX = 100, gridY = 100;
        bool[,,] grid = new bool[gridX, gridY, 2];  //[x,y,dir (isHorizontal)]
        Random rng = new Random();
        int editsLeft = 1000;

        Bitmap bmp = new Bitmap(gridX * 5, gridY * 5);
        Graphics g;

        bool closeRequested;

        readonly Pen lineCol = Pens.Lime;
        readonly Brush backCol = Brushes.Black;

        public SubtractionMethod()
        {
            InitializeComponent();

            g = Graphics.FromImage(bmp);

            pictureBox1.Size = new Size(1000, 1000);

            for (int x = 0; x < gridX; x++)
                for (int y = 0; y < gridY; y++)
                    grid[x, y, 0] = grid[x, y, 1] = true;

            Render();
        }

        private void SubtractionMethod_FormClosed(object sender, FormClosedEventArgs e)
        {
            closeRequested = true;
        }

        private void ButtonStep_Click(object sender, EventArgs e)
        {
            UpdateGame();
            Render();
        }

        private void CheckPlay_CheckedChanged(object sender, EventArgs e)
        {
            while (checkPlay.Checked && !closeRequested)
            {
                if (editsLeft <= 0)
                {
                    checkPlay.Checked = false;
                    break;
                }
                UpdateGame();
                Application.DoEvents();
                if (checkRender.Checked)
                    Render();
            }
            Render();
        }

        void UpdateGame()
        {
            int x = rng.Next(gridX), y = rng.Next(gridY), dir = rng.Next(2), count = 0;

            if (!checkEnds.Checked)
            {
                //check connections at start
                if (grid[x, y, 0])  //right
                    count++;
                if (grid[x, y, 1])  //down
                    count++;
                if (x > 0 && grid[x - 1, y, 0])  //left
                    count++;
                if (y > 0 && grid[x, y - 1, 1])  //up
                    count++;
                if (count == 2)
                    return;

                //check connections at end
                count = 0;
                if (grid[x, y, dir])  //self
                    count++;
                if ((dir == 0 ? x < gridX - 1 : x > 0) && (dir == 0 ? y > 0 : y < gridY - 1) && grid[x + (dir == 0 ? 1 : -1), y + (dir == 0 ? -1 : 1), dir ^ 1])  //other
                    count++;
                if ((dir == 0 ? x < gridX - 1 : y < gridY - 1) && grid[x + (dir == 0 ? 1 : 0), y + (dir == 0 ? 0 : 1), 0])  //right
                    count++;
                if ((dir == 0 ? x < gridX - 1 : y < gridY - 1) && grid[x + (dir == 0 ? 1 : 0), y + (dir == 0 ? 0 : 1), 1])  //down
                    count++;
                if (count == 2)
                    return;
            }

            grid[x, y, dir] = false;
            editsLeft--;
        }

        private void TextEdits_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textEdits.Text, out int num))
                editsLeft = num;
        }

        private void ButtonTrim_Click(object sender, EventArgs e)
        {
            //removes any loose ends
            int count;
            for (int x = 0; x < gridX; x++)
                for (int y = 0; y < gridY; y++)
                {
                    //check connections at start
                    count = 0;
                    if (grid[x, y, 0])  //right
                        count++;
                    if (grid[x, y, 1])  //down
                        count++;
                    if (x > 0 && grid[x - 1, y, 0])  //left
                        count++;
                    if (y > 0 && grid[x, y - 1, 1])  //up
                        count++;
                    if (count == 1)
                        grid[x, y, 0] = grid[x, y, 1] = false;

                    for (int dir = 0; dir < 2; dir++)
                    {
                        //check connections at end
                        count = 0;
                        if (grid[x, y, dir])  //self
                            count++;
                        if ((dir == 0 ? x < gridX - 1 : x > 0) && (dir == 0 ? y > 0 : y < gridY - 1) && grid[x + (dir == 0 ? 1 : -1), y + (dir == 0 ? -1 : 1), dir ^ 1])  //other
                            count++;
                        if ((dir == 0 ? x < gridX - 1 : y < gridY - 1) && grid[x + (dir == 0 ? 1 : 0), y + (dir == 0 ? 0 : 1), 0])  //right
                            count++;
                        if ((dir == 0 ? x < gridX - 1 : y < gridY - 1) && grid[x + (dir == 0 ? 1 : 0), y + (dir == 0 ? 0 : 1), 1])  //down
                            count++;
                        if (count == 1)
                            grid[x, y, dir] = false;
                    }
                }
            Render();
        }

        void Render()
        {
            for (int x = 0; x < gridX; x++)
                for (int y = 0; y < gridY; y++)
                {
                    //background
                    g.FillRectangle(backCol, x * 5, y * 5, 5, 5);
                    if (x > 0 && y > 0 && (grid[x - 1, y, 0] || grid[x, y - 1, 1]))
                        bmp.SetPixel(x * 5, y * 5, lineCol.Color);

                    if (grid[x, y, 0])  //horizontal
                        g.DrawLine(lineCol, x * 5, y * 5, x * 5 + 6, y * 5);
                    if (grid[x, y, 1])  //vertical
                        g.DrawLine(lineCol, x * 5, y * 5, x * 5, y * 5 + 6);
                }
            pictureBox1.Image = bmp;
            textEdits.Text = editsLeft.ToString();
        }
    }
}
