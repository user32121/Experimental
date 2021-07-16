using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASCIIArt
{
    public partial class ASCIIArt : Form
    {
        //timing
        Stopwatch stopwatch = new Stopwatch();
        bool loading;

        //chars
        int[][,] chars = new int[charMax][,];
        Font drawingFont;
        const int charMin = 32;
        const int charMax = 256;
        Size targetSize;
        int charWidth;

        //image
        Bitmap bmp;
        string text;

        //downscaling
        private int charScaleX;
        private int charScaleY;
        private int[,] pixelScaleCount = new int[4, 4];
        readonly Rectangle rect4x4 = new Rectangle(0, 0, 4, 4);

        public ASCIIArt()
        {
            InitializeComponent();
        }

        private void ASCIIArt_Load(object sender, EventArgs e)
        {
            //load font
            FontFamily[] families = FontFamily.Families;
            for (int i = 0; i < families.Length; i++)
                if (String.Equals(families[i].Name, "consolas", StringComparison.OrdinalIgnoreCase))
                    drawingFont = new Font(families[i], 8);
            if (drawingFont == null)
                throw new Exception("Could not find Consolas font");
            textDisplay.Font = drawingFont;

            LoadChars();
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            if (loading)
                return;

            stopwatch.Restart();

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            labelProgress.Text = "Rendering";
            loading = true;

            //open image
            bmp = (Bitmap)Image.FromStream(openFileDialog1.OpenFile());
            pictureDisplay.Image = bmp;

            //convert image to text
            text = "";
            Bitmap bmpSml = new Bitmap(4, 4);
            Graphics GSml = Graphics.FromImage(bmpSml);
            progressBar1.Maximum = bmp.Height;
            for (int y = 0; y + targetSize.Height < bmp.Height; y += targetSize.Height)
            {
                for (int x = 0; x + targetSize.Width < bmp.Width; x += charWidth)
                {
                    int bestI = -1;
                    int bestScore = int.MaxValue;  //lower score is better
                    int score, dist;

                    GSml.DrawImage(bmp, rect4x4, new Rectangle(x, y, targetSize.Width, targetSize.Height), GraphicsUnit.Pixel);

                    for (int i = charMin; i < charMax; i++)
                    {
                        if (chars[i] == null)
                            continue;

                        score = 0;
                        for (int x2 = 0; x2 < bmpSml.Width; x2++)
                            for (int y2 = 0; y2 < bmpSml.Height; y2++)
                            {
                                dist = chars[i][x2, y2] - (int)(bmpSml.GetPixel(x2, y2).GetBrightness() * 256);
                                score += dist * dist;
                            }
                        if (score < bestScore)
                        {
                            bestScore = score;
                            bestI = i;
                        }
                    }
                    text += (char)bestI;

                    if (stopwatch.ElapsedMilliseconds > 1000)
                    {
                        progressBar1.Value = y;
                        Render();
                        Application.DoEvents();
                        stopwatch.Restart();
                    }
                }
                text += '\n';
            }
            Render();
            progressBar1.Value = bmp.Height;
            labelProgress.Text = "Done rendering";
            loading = false;
        }

        void Render()
        {
            textDisplay.Left = pictureDisplay.Right;
            Size size = TextRenderer.MeasureText(text, textDisplay.Font, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
            textDisplay.Size = size + new Size(5, 5);
            textDisplay.Text = text;
        }

        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            if (loading)
                return;

            if (float.TryParse(textFont.Text, out float size))
            {
                drawingFont = new Font(drawingFont.FontFamily, size);
                textDisplay.Font = drawingFont;
                LoadChars();
            }
            else
                MessageBox.Show("Could not parse \"" + textFont.Text + "\"");
        }

        private void LoadChars()
        {
            //setup
            stopwatch.Restart();
            labelProgress.Text = "Loading char bmps";
            loading = true;
            progressBar1.Maximum = charMax;

            //setup for drawing
            textFont.Text = drawingFont.Size.ToString();
            targetSize = TextRenderer.MeasureText("A", drawingFont, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
            Bitmap charBmp = new Bitmap(targetSize.Width, targetSize.Height);
            Graphics charG = Graphics.FromImage(charBmp);
            charWidth = targetSize.Width - TextRenderer.MeasureText("", drawingFont, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix).Width;
            charScaleX = (int)Math.Ceiling(targetSize.Width / 4.0);
            charScaleY = (int)Math.Ceiling(targetSize.Height / 4.0);

            //count number of pixels to each scaled pixel
            for (int x = 0; x < targetSize.Width; x++)
                for (int y = 0; y < targetSize.Height; y++)
                    pixelScaleCount[x / charScaleX, y / charScaleY]++;

            //load char set
            for (char i = (char)charMin; i < charMax; i++)
            {
                Size t = TextRenderer.MeasureText(i.ToString(), drawingFont, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
                if (t == targetSize)
                {
                    chars[i] = new int[4, 4];

                    charG.Clear(Color.White);
                    charG.DrawString(i.ToString(), drawingFont, Brushes.Black, Point.Empty);
                    for (int x = 0; x < targetSize.Width; x++)
                        for (int y = 0; y < targetSize.Height; y++)
                            chars[i][x / charScaleX, y / charScaleY] += (byte)(charBmp.GetPixel(x, y).GetBrightness() * 255);
                    for (int x = 0; x < 4; x++)
                        for (int y = 0; y < 4; y++)
                            chars[i][x, y] /= pixelScaleCount[x, y];

                    if (stopwatch.ElapsedMilliseconds > 1000)
                    {
                        progressBar1.Value = i;
                        Application.DoEvents();
                        stopwatch.Restart();
                    }
                }
                else
                    chars[i] = null;
            }
            progressBar1.Value = charMax;
            labelProgress.Text = "Done loading";
            loading = false;
        }
    }
}
