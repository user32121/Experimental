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
        Stopwatch stopwatch = new Stopwatch();

        Bitmap bmp;
        string text;

        bool imageChanged;

        public ASCIIArt()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            stopwatch.Start();

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            bmp = (Bitmap)Image.FromStream(openFileDialog1.OpenFile());
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            MemoryStream stream = new MemoryStream();
            bmp.Save(stream, ImageFormat.Bmp);
            byte[] image = stream.ToArray();

            int bpp = 4;
            switch (bmpData.PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    bpp = 3;
                    break;
                case PixelFormat.Format32bppArgb:
                    break;
                default:
                    MessageBox.Show(bmpData.PixelFormat + " pixel format is not supported.\nUnexpected results may occur.");
                    break;
            }

            text = "";
            int pxlCount = 0;
            int lineStart = image[10] + (image[11] << 8) + (image[12] << 16) + (image[13] << 24);
            for (int i = lineStart; i < image.Length - bpp; i += bpp * 2)
            {
                //convert to text
                switch ((int)Math.Round(Color.FromArgb(image[i + 2], image[i + 1], image[i]).GetBrightness() * 10))
                {
                    case 0:
                        text += "##";
                        break;
                    case 1:
                        text += "$$";
                        break;
                    case 2:
                        text += "%%";
                        break;
                    case 3:
                        text += "EE";
                        break;
                    case 4:
                        text += "||";
                        break;
                    case 5:
                        text += "ww";
                        break;
                    case 6:
                        text += "ii";
                        break;
                    case 7:
                        text += "::";
                        break;
                    case 8:
                        text += "--";
                        break;
                    case 9:
                        text += "..";
                        break;
                    case 10:
                        text += "  ";
                        break;
                }
                //update display
                if (stopwatch.ElapsedMilliseconds > 1000)
                {
                    Render();
                    imageChanged = false;
                    Application.DoEvents();
                    if (imageChanged)
                        return;
                    stopwatch.Restart();
                }

                pxlCount += 2;
                if (pxlCount >= bmpData.Width)
                {
                    lineStart += bmpData.Stride * 2;
                    pxlCount = 0;
                    i = lineStart;
                    text += "\n";
                }
            }
            //unlock
            bmp.UnlockBits(bmpData);

            //reflect image/text (reverse order of lines)
            text = string.Join("\n", text.Split('\n').Reverse());

            imageChanged = true;

            Render();
        }

        void Render()
        {
            textDisplay.Left = pictureDisplay.Right;
            Size size = TextRenderer.MeasureText(text, textDisplay.Font);
            textDisplay.Size = size + new Size(5, 5);

            pictureDisplay.Image = (Bitmap)bmp.Clone();
            textDisplay.Text = text;
        }
    }
}
