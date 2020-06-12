using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageLayers
{
    public partial class ImageLayers : Form
    {
        Bitmap bmp;
        Bitmap bmpR;
        Bitmap bmpG;
        Bitmap bmpB;
        Bitmap bmpH;
        Bitmap bmpS;
        Bitmap bmpV;
        PointF scale = new PointF(1, 1);

        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        bool stopRequested;

        enum PIXELVALUE
        {
            R, G, B,
            H, S, V,
        }

        public ImageLayers()
        {
            InitializeComponent();
        }

        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            //open an image file
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;

            //load it
            bmp = (Bitmap)Image.FromStream(openFileDialog.OpenFile());

            MovePictureBoxes(bmp.Size);

            //extract pixel values
            ExtractPixelValues2(bmp, ref bmpR, pictureR, PIXELVALUE.R);
            ExtractPixelValues2(bmp, ref bmpG, pictureG, PIXELVALUE.G);
            ExtractPixelValues2(bmp, ref bmpB, pictureB, PIXELVALUE.B);
            ExtractPixelValues2(bmp, ref bmpH, pictureH, PIXELVALUE.H);
            ExtractPixelValues2(bmp, ref bmpS, pictureS, PIXELVALUE.S);
            ExtractPixelValues2(bmp, ref bmpV, pictureV, PIXELVALUE.V);
        }

        void ExtractPixelValues(Bitmap source, ref Bitmap targetBmp, PictureBox targetBox, PIXELVALUE type)
        {
            stopwatch.Start();
            targetBmp = new Bitmap(source.Width, source.Height);
            float val;

            for (int x = 0; x < source.Width && !stopRequested; x++)
                for (int y = 0; y < source.Height && !stopRequested; y++)
                {
                    switch (type)
                    {
                        case PIXELVALUE.R:
                            targetBmp.SetPixel(x, y, Color.FromArgb(source.GetPixel(x, y).R, 0, 0));
                            break;
                        case PIXELVALUE.G:
                            targetBmp.SetPixel(x, y, Color.FromArgb(0, source.GetPixel(x, y).G, 0));
                            break;
                        case PIXELVALUE.B:
                            targetBmp.SetPixel(x, y, Color.FromArgb(0, 0, source.GetPixel(x, y).B));
                            break;
                        case PIXELVALUE.H:
                            targetBmp.SetPixel(x, y, IsolateHue(source.GetPixel(x, y)));
                            break;
                        case PIXELVALUE.S:
                            val = source.GetPixel(x, y).GetSaturation();
                            targetBmp.SetPixel(x, y, Color.FromArgb((int)(val * 255), (int)(val * 255), (int)(val * 255)));
                            break;
                        case PIXELVALUE.V:
                            val = source.GetPixel(x, y).GetBrightness();
                            targetBmp.SetPixel(x, y, Color.FromArgb((int)(val * 255), (int)(val * 255), (int)(val * 255)));
                            break;
                        default:
                            break;
                    }

                    //update display
                    if (stopwatch.ElapsedMilliseconds > 100)
                    {
                        targetBox.Image = targetBmp;
                        Application.DoEvents();
                        stopwatch.Restart();
                    }
                }
        }

        void ExtractPixelValues2(Bitmap source, ref Bitmap targetBmp, PictureBox targetBox, PIXELVALUE type)
        {
            stopwatch.Start();

            BitmapData bmpData = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadWrite, source.PixelFormat);
            MemoryStream stream = new MemoryStream();
            source.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] image = stream.ToArray();
            float val;
            Color col;

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

            int pxlCount = 0;
            int lineStart = image[10] + (image[11] << 8) + (image[12] << 16) + (image[13] << 24);
            for (int i = lineStart; i < image.Length - bpp; i += bpp)
            {
                switch (type)
                {
                    case PIXELVALUE.R:
                        image[i] = 0; image[i + 1] = 0;
                        break;
                    case PIXELVALUE.G:
                        image[i] = 0; image[i + 2] = 0;
                        break;
                    case PIXELVALUE.B:
                        image[i + 1] = 0; image[i + 2] = 0;
                        break;
                    case PIXELVALUE.H:
                        col = IsolateHue(Color.FromArgb(image[i + 2], image[i + 1], image[i]));
                        image[i + 2] = col.R;
                        image[i + 1] = col.G;
                        image[i] = col.B;
                        break;
                    case PIXELVALUE.S:
                        val = Color.FromArgb(image[i + 2], image[i + 1], image[i]).GetSaturation();
                        image[i] = (byte)(val * 255);
                        image[i + 1] = (byte)(val * 255);
                        image[i + 2] = (byte)(val * 255);
                        break;
                    case PIXELVALUE.V:
                        val = Color.FromArgb(image[i + 2], image[i + 1], image[i]).GetBrightness();
                        image[i] = (byte)(val * 255);
                        image[i + 1] = (byte)(val * 255);
                        image[i + 2] = (byte)(val * 255);
                        break;
                    default:
                        break;
                }

                //update display
                if (stopwatch.ElapsedMilliseconds > 100)
                {
                    targetBox.Image = new Bitmap(new MemoryStream(image));
                    Application.DoEvents();
                    stopwatch.Restart();
                }

                pxlCount++;
                if (pxlCount == bmpData.Width)
                {
                    lineStart += bmpData.Stride;
                    pxlCount = 0;
                    i = lineStart;
                }
            }
            source.UnlockBits(bmpData);

            targetBmp = new Bitmap(new MemoryStream(image));
            targetBox.Image = new Bitmap(new MemoryStream(image));
        }

        Color IsolateHue(Color col)
        {
            int[] vals = new int[] { col.R, col.G, col.B };
            int min = col.R < col.G ? (col.R < col.B ? 0 : 2) : (col.G < col.B ? 1 : 2);
            int max = col.R > col.G ? (col.R > col.B ? 0 : 2) : (col.G > col.B ? 1 : 2);

            if (min == max)
                return Color.Black;

            //neutralize saturation
            vals[3 - min - max] -= vals[min] * (vals[max] - vals[3 - min - max]) / (vals[max] - vals[min]);
            vals[min] = 0;
            //neutralize value
            vals[3 - min - max] += (255 - vals[max]) * vals[3 - min - max] / vals[max];
            vals[max] = 255;
            return Color.FromArgb(vals[0], vals[1], vals[2]);
        }

        void MovePictureBoxes(Size imgSize)
        {
            Size scaledSize = new Size((int)(imgSize.Width * scale.X), (int)(imgSize.Height * scale.Y));
            pictureR.Size = scaledSize;
            pictureG.Size = scaledSize;
            pictureB.Size = scaledSize;
            pictureH.Size = scaledSize;
            pictureS.Size = scaledSize;
            pictureV.Size = scaledSize;

            pictureR.Location = new Point(0, 50);
            pictureG.Location = new Point(scaledSize.Width, 50);
            pictureB.Location = new Point(scaledSize.Width * 2, 50);
            pictureH.Location = new Point(0, scaledSize.Height + 50);
            pictureS.Location = new Point(scaledSize.Width, scaledSize.Height + 50);
            pictureV.Location = new Point(scaledSize.Width * 2, scaledSize.Height + 50);
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            saveFileDialog.FileName = "Red";
            saveFileDialog.ShowDialog();
            bmpR.Save(saveFileDialog.OpenFile(), System.Drawing.Imaging.ImageFormat.Bmp);
            saveFileDialog.FileName = "Green";
            saveFileDialog.ShowDialog();
            bmpG.Save(saveFileDialog.OpenFile(), System.Drawing.Imaging.ImageFormat.Bmp);
            saveFileDialog.FileName = "Blue";
            saveFileDialog.ShowDialog();
            bmpB.Save(saveFileDialog.OpenFile(), System.Drawing.Imaging.ImageFormat.Bmp);
            saveFileDialog.FileName = "Hue";
            saveFileDialog.ShowDialog();
            bmpH.Save(saveFileDialog.OpenFile(), System.Drawing.Imaging.ImageFormat.Bmp);
            saveFileDialog.FileName = "Saturation";
            saveFileDialog.ShowDialog();
            bmpS.Save(saveFileDialog.OpenFile(), System.Drawing.Imaging.ImageFormat.Bmp);
            saveFileDialog.FileName = "Value";
            saveFileDialog.ShowDialog();
            bmpV.Save(saveFileDialog.OpenFile(), System.Drawing.Imaging.ImageFormat.Bmp);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopRequested = true;
        }

        private void TextScale_TextChanged(object sender, EventArgs e)
        {
            string[] line = textScale.Text.Split(',');
            if (line.Length >= 2)
            {
                if (float.TryParse(line[0], out float result))
                    scale.X = result;
                else
                    return;

                if (float.TryParse(line[1], out result))
                    scale.Y = result;
                else
                    return;

                if (pictureR.Image != null)
                    MovePictureBoxes(pictureR.Image.Size);
                else
                    MovePictureBoxes(new Size(100, 100));
            }
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            Bitmap testbmp = new Bitmap(800, 400);

            //fill it
            MemoryStream stream = new MemoryStream();
            testbmp.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] image = stream.GetBuffer();
            //int pos;
            //for (byte r = 0; r <= 30; r++)
            //    for (byte g = 0; g <= 100; g++)
            //        for (byte b = 0; b <= 100; b++)
            //        {
            //            pos = 4 * ((r * 100 + g) * 100 + b) + 54;
            //            image[pos] = (byte)(r * 2);
            //            image[pos + 1] = (byte)(g * 2);
            //            image[pos + 2] = (byte)(b * 2);
            //            image[pos + 3] = 255;
            //        }
            for (int i = image[10] + image[11] * 256 + image[12] * 655356 + image[13] * 1677216; i < image.Length; i += 4)
            {
                image[i + 0] = 255;
                image[i + 1] = 117;
                image[i + 2] = 211;
            }
            testbmp = new Bitmap(new MemoryStream(image));

            MovePictureBoxes(testbmp.Size);

            //extract pixel values
            ExtractPixelValues2(testbmp, ref bmpR, pictureR, PIXELVALUE.R);
            ExtractPixelValues2(testbmp, ref bmpG, pictureG, PIXELVALUE.G);
            ExtractPixelValues2(testbmp, ref bmpB, pictureB, PIXELVALUE.B);
            ExtractPixelValues2(testbmp, ref bmpH, pictureH, PIXELVALUE.H);
            ExtractPixelValues2(testbmp, ref bmpS, pictureS, PIXELVALUE.S);
            ExtractPixelValues2(testbmp, ref bmpV, pictureV, PIXELVALUE.V);
        }
    }
}

