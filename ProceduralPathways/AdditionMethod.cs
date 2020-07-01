using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProceduralPathways
{
    public partial class AdditionMethod : Form
    {
        const int gridX = 100, gridY = 100;
        Tile[,] grid = new Tile[gridX, gridY];
        Dictionary<Point, object> toProcess = new Dictionary<Point, object>();  //map of tiles to add
        Random rng = new Random();

        Bitmap bmp = new Bitmap(gridX * 5, gridY * 5);
        Graphics g;

        bool closeRequested;

        public AdditionMethod()
        {
            InitializeComponent();

            g = Graphics.FromImage(bmp);

            pictureBox1.Size = new Size(1000, 1000);

            availableFlags = new bool[availableTiles.Count];

            //starting tile
            toProcess[new Point(50, 50)] = null;
            Render();
        }

        private void AdditionMethod_FormClosed(object sender, FormClosedEventArgs e)
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
                if (toProcess.Count == 0)
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
            if (toProcess.Count == 0)
                return;

            Dictionary<Point, object>.KeyCollection.Enumerator enumerator = toProcess.Keys.GetEnumerator();
            int count = rng.Next(toProcess.Count);
            while (count-- >= 0)
                enumerator.MoveNext();
            Point pos = enumerator.Current;
            enumerator.Dispose();
            int i;

            for (i = 0; i < availableTiles.Count; i++)
                availableFlags[i] = true;
            count = availableFlags.Length;

            //get tile to left
            int newX = (pos.X - 1 + gridX) % gridX;
            if (grid[newX, pos.Y] != null)
                //filter out tiles with non matching connections
                for (i = 0; i < availableTiles.Count; i++)
                    if (availableFlags[i] && (grid[newX, pos.Y].right != availableTiles[i].left))
                    { availableFlags[i] = false; count--; }
            //right
            newX = (pos.X + 1) % gridX;
            if (grid[newX, pos.Y] != null)
                for (i = 0; i < availableTiles.Count; i++)
                    if (availableFlags[i] && (grid[newX, pos.Y].left != availableTiles[i].right))
                    { availableFlags[i] = false; count--; }
            //up
            int newY = (pos.Y - 1 + gridY) % gridY;
            if (grid[pos.X, newY] != null)
                for (i = 0; i < availableTiles.Count; i++)
                    if (availableFlags[i] && (grid[pos.X, newY].down != availableTiles[i].up))
                    { availableFlags[i] = false; count--; }
            //down
            newY = (pos.Y + 1) % gridY;
            if (grid[pos.X, newY] != null)
                for (i = 0; i < availableTiles.Count; i++)
                    if (availableFlags[i] && (grid[pos.X, newY].up != availableTiles[i].down))
                    { availableFlags[i] = false; count--; }

            if (count != 0)
            {
                i = -1;
                count = rng.Next(count);
                while (count >= 0)
                    if (availableFlags[++i])
                        count--;

                AddTile(availableTiles[i], pos.X, pos.Y);
            }
            else if (checkCorrect.Checked)
            {
                //find tile with least amount of issues
                int index = -1, issues = 5, newIssues;
                for (i = 0; i < availableTiles.Count; i++)
                {
                    newIssues = 0;
                    newX = (pos.X - 1 + gridX) % gridX;
                    if (grid[newX, pos.Y] != null && grid[newX, pos.Y].right != availableTiles[i].left)
                        newIssues++;
                    newX = (pos.X + 1) % gridX;
                    if (grid[newX, pos.Y] != null && grid[newX, pos.Y].left != availableTiles[i].right)
                        newIssues++;
                    newY = (pos.Y - 1 + gridY) % gridY;
                    if (grid[pos.X, newY] != null && grid[pos.X, newY].down != availableTiles[i].up)
                        newIssues++;
                    newY = (pos.Y + 1) % gridY;
                    if (grid[pos.X, newY] != null && grid[pos.X, newY].up != availableTiles[i].down)
                        newIssues++;

                    if (newIssues < issues || newIssues == issues && rng.NextDouble() > 0.5)
                    {
                        index = i;
                        issues = newIssues;
                    }
                }
                AddTile(availableTiles[index], pos.X, pos.Y);

                newX = (pos.X - 1 + gridX) % gridX;
                if (grid[newX, pos.Y] != null && grid[newX, pos.Y].right != availableTiles[index].left)
                    toProcess[new Point(newX, pos.Y)] = null;
                newX = (pos.X + 1) % gridX;
                if (grid[newX, pos.Y] != null && grid[newX, pos.Y].left != availableTiles[index].right)
                    toProcess[new Point(newX, pos.Y)] = null;
                newY = (pos.Y - 1 + gridY) % gridY;
                if (grid[pos.X, newY] != null && grid[pos.X, newY].down != availableTiles[index].up)
                    toProcess[new Point(pos.X, newY)] = null;
                newY = (pos.Y + 1) % gridY;
                if (grid[pos.X, newY] != null && grid[pos.X, newY].up != availableTiles[index].down)
                    toProcess[new Point(pos.X, newY)] = null;
            }
            else
                AddTile(errorTile, pos.X, pos.Y);

            toProcess.Remove(pos);
        }

        void AddTile(Tile tile, int x, int y)
        {
            grid[x, y] = tile;
            int newX = (x - 1 + gridX) % gridX;
            if (grid[newX, y] == null)
                toProcess[new Point(newX, y)] = null;
            newX = (x + 1) % gridX;
            if (grid[newX, y] == null)
                toProcess[new Point(newX, y)] = null;
            int newY = (y - 1 + gridY) % gridY;
            if (grid[x, newY] == null)
                toProcess[new Point(x, newY)] = null;
            newY = (y + 1) % gridY;
            if (grid[x, newY] == null)
                toProcess[new Point(x, newY)] = null;
        }

        void Render()
        {
            for (int x = 0; x < gridX; x++)
                for (int y = 0; y < gridY; y++)
                {
                    if (grid[x, y] == null)
                        g.FillRectangle(Brushes.Black, x * 5, y * 5, 5, 5);
                    else
                    {
                        g.FillRectangle(new SolidBrush(Color.FromKnownColor(grid[x, y].backCol)), x * 5, y * 5, 5, 5);
                        bmp.SetPixel(x * 5 + 2, y * 5 + 2, Color.FromKnownColor(grid[x, y].center));
                        g.DrawLine(new Pen(Color.FromKnownColor(grid[x, y].left)), x * 5, y * 5 + 2, x * 5 + 1, y * 5 + 2);
                        g.DrawLine(new Pen(Color.FromKnownColor(grid[x, y].right)), x * 5 + 3, y * 5 + 2, x * 5 + 4, y * 5 + 2);
                        g.DrawLine(new Pen(Color.FromKnownColor(grid[x, y].up)), x * 5 + 2, y * 5, x * 5 + 2, y * 5 + 1);
                        g.DrawLine(new Pen(Color.FromKnownColor(grid[x, y].down)), x * 5 + 2, y * 5 + 3, x * 5 + 2, y * 5 + 4);
                    }
                }

            Dictionary<Point, object>.KeyCollection.Enumerator enumerator = toProcess.Keys.GetEnumerator();
            while (enumerator.MoveNext())
                g.DrawRectangle(Pens.Red, enumerator.Current.X * 5, enumerator.Current.Y * 5, 5, 5);
            enumerator.Dispose();

            labelToProcess.Text = toProcess.Keys.Count.ToString();

            pictureBox1.Image = bmp;
        }

        private void ButtonSavePalette_Click(object sender, EventArgs e)
        {
            if (savePalette.ShowDialog() == DialogResult.Cancel)
                return;
            StreamWriter sw = new StreamWriter(savePalette.OpenFile());

            for (int i = 0; i < availableTiles.Count; i++)
                sw.WriteLine("L:{0},R:{1},U:{2},D:{3},C:{4},B:{5}", availableTiles[i].left, availableTiles[i].right, availableTiles[i].up, availableTiles[i].down, availableTiles[i].center, availableTiles[i].backCol);

            sw.Close();
            sw.Dispose();
        }

        private void ButtonSaveImage_Click(object sender, EventArgs e)
        {
            if (saveImage.ShowDialog() == DialogResult.Cancel)
                return;
            bmp.Save(saveImage.FileName);
        }

        private void ButtonLoadPalette_Click(object sender, EventArgs e)
        {
            if (openPalette.ShowDialog() == DialogResult.Cancel)
                return;
            availableTiles.Clear();
            StreamReader sr = new StreamReader(openPalette.OpenFile());

            string[] line;
            Tile tile;

            while (!sr.EndOfStream)
            {
                line = sr.ReadLine().Split(',');
                tile = new Tile();
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i].Length < 2)
                        goto SKIP;
                    switch (line[i].Substring(0, 2))
                    {
                        case "L:":
                            tile.left = (KnownColor)Enum.Parse(typeof(KnownColor), line[i].Substring(2), true);
                            break;
                        case "R:":
                            tile.right = (KnownColor)Enum.Parse(typeof(KnownColor), line[i].Substring(2), true);
                            break;
                        case "U:":
                            tile.up = (KnownColor)Enum.Parse(typeof(KnownColor), line[i].Substring(2), true);
                            break;
                        case "D:":
                            tile.down = (KnownColor)Enum.Parse(typeof(KnownColor), line[i].Substring(2), true);
                            break;
                        case "C:":
                            tile.center = (KnownColor)Enum.Parse(typeof(KnownColor), line[i].Substring(2), true);
                            break;
                        case "B:":
                            tile.backCol = (KnownColor)Enum.Parse(typeof(KnownColor), line[i].Substring(2), true);
                            break;
                    }
                }
                availableTiles.Add(tile);
            SKIP:;
            }
            availableFlags = new bool[availableTiles.Count];

            sr.Close();
            sr.Dispose();
        }
    }
}
