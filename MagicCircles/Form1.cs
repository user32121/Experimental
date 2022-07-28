using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagicCircles
{
    public partial class Form1 : Form
    {
        Random rng = new Random();

        CircleData circleData;
        RuneData runeData;
        bool displayingCircle;

        bool addedTime;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GenerateCircle();
        }

        private void buttonRune_Click(object sender, EventArgs e)
        {
            GenerateRune(2, true);
        }

        private void buttonCircle_Click(object sender, EventArgs e)
        {
            GenerateCircle();
        }

        private Bitmap GenerateRune(int thickness, bool display, RuneData sourceData = null)
        {
            runeData = sourceData ?? new RuneData();
            Bitmap bmp = new Bitmap(50, 100);
            Graphics g = Graphics.FromImage(bmp);

            //generation parameters
            bool doubleMidPoints = rng.NextDouble() < 0.5;

            List<PointF> runePoints = new List<PointF>();
            foreach (float x in threePoints)
                foreach (float y in doubleMidPoints ? fourPoints : threePoints)
                    runePoints.Add(new PointF(x, y));

            Pen pen = (Pen)Pens.Black.Clone();
            pen.Width = thickness;
            const int numStrokes = 5;
            if (sourceData == null)
            {
                runeData.strokes = new RuneStroke[numStrokes];
            }
            for (int i = 0; i < runeData.strokes.Length; i++)
            {
                PointF p1, p2;
                int variant;
                if (sourceData == null)
                {
                    runeData.strokes[i].start = p1 = runePoints[rng.Next(runePoints.Count)].Scale(bmp.Size);
                    runeData.strokes[i].end = p2 = runePoints[rng.Next(runePoints.Count)].Scale(bmp.Size);
                    runeData.strokes[i].type = availableLineTypes[rng.Next(availableLineTypes.Length)];
                    runeData.strokes[i].variant = variant = rng.Next(2);
                }
                else
                {
                    p1 = runeData.strokes[i].start;
                    p2 = runeData.strokes[i].end;
                    variant = runeData.strokes[i].variant;
                }
                switch (runeData.strokes[i].type)
                {
                    case LINE_TYPE.STRAIGHT:
                        {
                            g.DrawLine(pen, p1, p2);
                            break;
                        }
                    case LINE_TYPE.SPLINE:
                        {
                            PointF p3;
                            if (variant == 0)
                                p3 = new PointF(p1.X, p2.Y);
                            else
                                p3 = new PointF(p2.X, p1.Y);
                            p3 = new PointF((p1.X + p2.X + p3.X * 2) / 4, (p1.Y + p2.Y + p3.Y * 2) / 4);
                            g.DrawCurve(pen, new PointF[] { p1, p3, p2 });
                            break;
                        }
                    case LINE_TYPE.BEZIER:
                        {
                            PointF p3, p4;
                            if (variant == 0)
                            {
                                p3 = new PointF(p1.X, (p1.Y + p2.Y) / 2);
                                p4 = new PointF(p2.X, (p1.Y + p2.Y) / 2);
                            }
                            else
                            {
                                p3 = new PointF((p1.X + p2.X) / 2, p1.Y);
                                p4 = new PointF((p1.X + p2.X) / 2, p2.Y);
                            }
                            g.DrawBezier(pen, p1, p3, p4, p2);
                            break;
                        }
                    default:
                        throw new NotImplementedException();
                }
            }

            if (display)
            {
                pictureBox1.Image?.Dispose();
                pictureBox1.Image = bmp;
                displayingCircle = false;
            }
            g.Dispose();
            return bmp;
        }

        private void GenerateCircle(CircleData sourceData = null)
        {
            circleData = sourceData ?? new CircleData();
            Bitmap bmp = new Bitmap(512, 512);
            Graphics g = Graphics.FromImage(bmp);
            SizeF center = new SizeF(bmp.Width / 2 - 0.5f, bmp.Height / 2 - 0.5f);

            //generation parameters
            if (sourceData == null)
            {
                circleData.minRadius = rng.Next(bmp.Width / 2 * 4 / 5);
                circleData.layers = rng.Next(2, Math.Max(2, (bmp.Width / 2 - circleData.minRadius) * 10 / (bmp.Width / 2)));
                circleData.layerSizes = new float[circleData.layers];
                circleData.layerSizes[0] = 0;
                //distribute random thicknesses, then scale to fit the system
                float totalThickness = 0;
                for (int i = 0; i < circleData.layers; i++)
                    totalThickness += circleData.layerSizes[i] = rng.Next(1, 11);
                for (int i = 0; i < circleData.layers; i++)
                    circleData.layerSizes[i] = circleData.layerSizes[i] / totalThickness * (bmp.Width / 2 - circleData.minRadius);  //[0,arbitrary] -> [0,1] -> [minRadius, width/2]
                circleData.patterns = new LAYER_PATTERN[circleData.layers];
                for (int i = 0; i < circleData.layers; i++)
                    circleData.patterns[i] = availablePatterns[rng.Next(availablePatterns.Length)];
            }

            //adjust thicknesses to fit
            for (int i = 0; i < circleData.layers; i++)
            {
                switch (circleData.patterns[i])
                {
                    case LAYER_PATTERN.RUNES:
                        if (circleData.layerSizes[i] < 10)
                        {
                            circleData.layerSizes[i] += 10;
                            if (i == 0)
                                circleData.layerSizes[i + 1] -= 10;
                            else
                                circleData.layerSizes[i - 1] -= 10;
                        }
                        else if (circleData.layerSizes[i] > 100)
                        {
                            circleData.layerSizes[i] -= 50;
                            if (i == 0)
                                circleData.layerSizes[i + 1] += 50;
                            else
                                circleData.layerSizes[i - 1] += 50;
                        }
                        break;
                    case LAYER_PATTERN.POLYGONS:
                        break;
                    case LAYER_PATTERN.SMALL_CIRCLES:
                        break;
                }
            }
            if (sourceData == null)
            {
                circleData.patternParams = new PatternParam[circleData.layers];
                circleData.runes = new List<RuneData>();
                circleData.borders = new LAYER_BORDER[circleData.layers];
                circleData.borderParams = new BorderParam[circleData.layers];
            }

            float curRadius = circleData.minRadius;
            for (int i = 0; i < circleData.layers; i++)
            {
                switch (circleData.patterns[i])
                {
                    case LAYER_PATTERN.DIAGONALS:
                        {
                            if (!(sourceData?.patternParams[i] is DiagonalsPatternParam dpp))
                            {
                                dpp = new DiagonalsPatternParam();
                                dpp.flipped = rng.NextDouble() < 0.5f;
                                dpp.delta = Math.PI * 2 / rng.Next(10, 200);
                                dpp.count = rng.Next(30, 200);
                            }
                            circleData.patternParams[i] = dpp;
                            for (int j = 0; j < dpp.count; j++)
                            {
                                double theta = j / (double)dpp.count * Math.PI * 2;
                                PointF p1 = PointFFromRadial(theta, curRadius);
                                PointF p2 = PointFFromRadial(theta + (dpp.flipped ? -dpp.delta : dpp.delta), curRadius + circleData.layerSizes[i]);
                                p1 += center;
                                p2 += center;
                                g.DrawLine(Pens.Black, p1, p2);
                            }
                            break;
                        }
                    case LAYER_PATTERN.DOUBLE_DIAGONALS:
                        {
                            if (!(sourceData?.patternParams[i] is DoubleDiagonalsPatternParam ddpp))
                            {
                                ddpp = new DoubleDiagonalsPatternParam();
                                ddpp.delta = Math.PI * 2 / rng.Next(10, 200);
                                ddpp.count = rng.Next(30, 200);
                            }
                            circleData.patternParams[i] = ddpp;
                            for (int j = 0; j < ddpp.count; j++)
                            {
                                double theta = j / (double)ddpp.count * Math.PI * 2;
                                PointF p1 = PointFFromRadial(theta, curRadius);
                                PointF p2 = PointFFromRadial(theta + ddpp.delta, curRadius + circleData.layerSizes[i]);
                                PointF p3 = PointFFromRadial(theta - ddpp.delta, curRadius + circleData.layerSizes[i]);
                                p1 += center;
                                p2 += center;
                                p3 += center;
                                g.DrawLine(Pens.Black, p1, p2);
                                g.DrawLine(Pens.Black, p1, p3);
                            }
                            break;
                        }
                    case LAYER_PATTERN.RUNES:
                        {
                            if (!(sourceData?.patternParams[i] is RunesPatternParam rpp))
                            {
                                rpp = new RunesPatternParam();
                                rpp.count = rng.Next(20, 40);
                                rpp.runeIndices = new int[rpp.count];
                            }
                            circleData.patternParams[i] = rpp;
                            for (int j = 0; j < rpp.count; j++)
                            {
                                float theta = (float)(j * Math.PI * 2 / rpp.count);
                                PointF pos = PointFFromRadial(theta, curRadius);
                                Bitmap runeBmp;
                                if (sourceData == null)
                                {
                                    runeBmp = GenerateRune(3, false);
                                    circleData.runes.Add(runeData);
                                    rpp.runeIndices[j] = circleData.runes.Count - 1;
                                }
                                else
                                {
                                    runeBmp = GenerateRune(3, false, circleData.runes[rpp.runeIndices[j]]);
                                    circleData.runes[rpp.runeIndices[j]] = runeData;
                                }

                                g.ResetTransform();
                                float scale = circleData.layerSizes[i] / runeBmp.Height;
                                PointF offset = PointFFromRadial(theta + Math.PI / 2, runeBmp.Width / 2).Scale(scale);  //create an offset to align the center of the card to the tangent of the circle
                                g.ScaleTransform(scale, scale, MatrixOrder.Append);
                                g.RotateTransform((float)(theta / Math.PI * 180 - 90), MatrixOrder.Append);
                                g.TranslateTransform(center.Width + pos.X + offset.X, center.Height + pos.Y + offset.Y, MatrixOrder.Append);
                                g.DrawImage(runeBmp, 0, 0);
                                g.ResetTransform();
                            }
                            break;
                        }
                    case LAYER_PATTERN.POLYGONS:
                        {
                            if (!(sourceData?.patternParams[i] is PolygonsPatternParam ppp))
                            {
                                ppp = new PolygonsPatternParam();
                                ppp.sides = rng.Next(3, 10);
                                ppp.isWinding = rng.NextDouble() < 0.5;
                                ppp.copies = ppp.isWinding ? 1 : rng.Next(2, 4);
                                ppp.winding = ppp.isWinding ? rng.Next(2, ppp.sides) : 1;  //constructs stars
                            }
                            circleData.patternParams[i] = ppp;

                            PointF prevPos = new PointF();
                            for (int j = 0; j < ppp.copies; j++)
                            {
                                double offset = j / (double)ppp.copies;
                                for (int k = 0; k <= ppp.sides; k++)
                                {
                                    PointF pos = PointFFromRadial((k + offset) * 2 * Math.PI / ppp.sides * ppp.winding, curRadius + circleData.layerSizes[i]);
                                    pos += center;
                                    if (k != 0)
                                        g.DrawLine(Pens.Black, pos, prevPos);
                                    prevPos = pos;
                                }
                            }
                            break;
                        }
                    case LAYER_PATTERN.EMPTY:
                        {
                            circleData.patternParams[i] = new EmptyPatternParam();
                            break;
                        }
                    default:
                        throw new NotImplementedException(circleData.patterns[i] + " is not implemented");
                }
                curRadius += circleData.layerSizes[i];

                if (sourceData == null)
                    circleData.borders[i] = availableBorders[rng.Next(availableBorders.Length)];
                switch (circleData.borders[i])
                {
                    case LAYER_BORDER.SOLID:
                        {
                            circleData.borderParams[i] = new SolidBorderParam();
                            g.DrawEllipse(Pens.Black, center.Width - curRadius, center.Height - curRadius, curRadius * 2, curRadius * 2);
                            break;
                        }
                    case LAYER_BORDER.DASHED:
                        {
                            if (!(sourceData?.borderParams[i] is DashedBorderParam dbp))
                            {
                                dbp = new DashedBorderParam();
                                dbp.count = rng.Next(2, 15) * 2;
                                dbp.thetaOffset = rng.Next(360);
                                dbp.dashLengthModifier = (float)rng.NextDouble() * 2;
                            }
                            circleData.borderParams[i] = dbp;
                            for (int j = 0; j < dbp.count; j++)
                                if (j % 2 == 0)
                                    g.DrawArc(Pens.Black, center.Width - curRadius, center.Height - curRadius, curRadius * 2, curRadius * 2, j * 360 / dbp.count + dbp.thetaOffset, dbp.dashLengthModifier * 360 / dbp.count);
                            break;
                        }
                    case LAYER_BORDER.SPIKES:
                        {
                            g.DrawEllipse(Pens.Black, center.Width - curRadius, center.Height - curRadius, curRadius * 2, curRadius * 2);
                            if (!(sourceData?.borderParams[i] is SpikesBorderParam sbp))
                            {
                                sbp = new SpikesBorderParam();
                                sbp.count = rng.Next(10, 30);
                                sbp.spikeWidth = rng.NextDouble() / 10;  //in radians
                                sbp.spikeLength = rng.Next((int)circleData.layerSizes[i] / 2);
                            }
                            circleData.borderParams[i] = sbp;
                            for (int j = 0; j < sbp.count; j++)
                            {
                                double theta = j * Math.PI * 2 / sbp.count;
                                PointF p1 = PointFFromRadial(theta, curRadius);
                                PointF p2 = PointFFromRadial(theta + sbp.spikeWidth, curRadius);
                                PointF p3 = PointFFromRadial(theta + sbp.spikeWidth / 2, curRadius + sbp.spikeLength);
                                PointF p4 = PointFFromRadial(theta + sbp.spikeWidth / 2, curRadius - sbp.spikeLength);
                                p1 += center;
                                p2 += center;
                                p3 += center;
                                p4 += center;
                                g.FillPolygon(Brushes.Black, new PointF[] { p1, p3, p2, p4 });
                            }
                            break;
                        }
                    case LAYER_BORDER.CROSSED:
                        {
                            g.DrawEllipse(Pens.Black, center.Width - curRadius, center.Height - curRadius, curRadius * 2, curRadius * 2);
                            if (!(sourceData?.borderParams[i] is CrossedBorderParam cbp))
                            {
                                cbp = new CrossedBorderParam();
                                cbp.count = rng.Next(10, 30);
                                cbp.hashLength = rng.Next((int)circleData.layerSizes[i] / 2);
                                cbp.hashWidth = cbp.hashLength / 5;
                                cbp.isInner = rng.NextDouble() < 0.5;
                            }
                            circleData.borderParams[i] = cbp;

                            for (int j = 0; j < cbp.count; j++)
                            {
                                double theta = j * Math.PI * 2 / cbp.count;
                                PointF p1 = PointFFromRadial(theta, curRadius - (cbp.isInner ? cbp.hashLength : 0));
                                PointF p2 = PointFFromRadial(theta, curRadius + (cbp.isInner ? 0 : cbp.hashLength));
                                p1 += center;
                                p2 += center;
                                PointF t = PointFFromRadial(theta + Math.PI / 2, cbp.hashWidth / 2);
                                SizeF deltaW = new SizeF(t.X, t.Y);
                                PointF p3 = p1 + deltaW;
                                PointF p4 = p1 - deltaW;
                                PointF p5 = p2 + deltaW;
                                PointF p6 = p2 - deltaW;
                                g.FillPolygon(Brushes.Black, new PointF[] { p3, p4, p6, p5 });
                            }
                            break;
                        }
                    default:
                        throw new NotImplementedException();
                }
            }

            pictureBox1.Image?.Dispose();
            pictureBox1.Image = bmp;
            displayingCircle = true;
            addedTime = false;
            g.Dispose();
        }

        private PointF PointFFromRadial(double angle, double magnitude)
        {
            return new PointF((float)Math.Cos(angle), (float)Math.Sin(angle)).Scale((float)magnitude);
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory("output");
            string[] files = Directory.GetFiles("output");
            int i = 0;
            string fileBase = "img";
            string filename;
            do
            {
                filename = Path.Combine("output", fileBase + i + ".png");
                i++;
            } while (files.Contains(filename));

            pictureBox1.Image.Save(filename);
        }

        private void Button_effect_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
                return;

            Bitmap bmp = new Bitmap(pictureBox1.Image.Width, pictureBox1.Image.Height);
            Image sourceBmp = pictureBox1.Image;
            ImageAttributes imgAttr = new ImageAttributes();
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);

            Color col;
            if (displayingCircle && circleData?.effectColor != null)
                col = circleData.effectColor.Value;
            else
                col = Color.FromKnownColor((KnownColor)rng.Next((int)KnownColor.AliceBlue, (int)KnownColor.YellowGreen));
            float R = col.R / (float)255;
            float G = col.G / (float)255;
            float B = col.B / (float)255;
            if (displayingCircle)
                circleData.effectColor = col;

            //invert + color (f(x) = -R(x + 1))
            ColorMatrix colMatrix = new ColorMatrix(new float[][]
            {
                new float[]{-R,0,0,0,0},
                new float[]{0,-G,0,0,0},
                new float[]{0,0,-B,0,0},
                new float[]{0,0,0,1,0},
                new float[]{R,G,B,0,1},
            });
            imgAttr.SetColorMatrix(colMatrix);

            //blur

            const int radius = 10;
            const int iterations = 3;
            colMatrix.Matrix33 = 1;
            imgAttr.SetColorMatrix(colMatrix);
            for (int x = -1; x <= 2; x++)
                for (int y = -1; y <= 2; y++)
                    g.DrawImage(sourceBmp, new Rectangle(new Point(x, y), bmp.Size), 0, 0, sourceBmp.Width, sourceBmp.Height, GraphicsUnit.Pixel, imgAttr);
            unsafe
            {
                BitmapData bmpData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.ReadWrite, bmp.PixelFormat);
                Color[,] buffer = new Color[bmpData.Width, bmpData.Height];
                
                for (int i = 0; i < iterations; i++)
                {
                    //horizontal pass
                    int totalR, totalG, totalB, totalA, count;
                    Color pixelCol;
                    int* scan0 = (int*)bmpData.Scan0;
                    for (int x = 0; x < bmpData.Width; x++)
                        for (int y = 0; y < bmpData.Height; y++)
                        {
                            totalR = totalG = totalB = totalA = count = 0;
                            for (int x2 = Math.Max(x - radius, 0); x2 <= Math.Min(x + radius, bmpData.Width - 1); x2++)
                            {
                                pixelCol = Color.FromArgb(*(scan0 + (y * bmpData.Stride / sizeof(int)) + x2));
                                totalR += pixelCol.R;
                                totalG += pixelCol.G;
                                totalB += pixelCol.B;
                                totalA += pixelCol.A;
                                count++;
                            }
                            buffer[x, y] = Color.FromArgb(totalA / count, totalR / count, totalG / count, totalB / count);
                        }

                    //vertical pass
                    for (int x = 0; x < bmpData.Width; x++)
                        for (int y = 0; y < bmpData.Height; y++)
                        {
                            totalR = totalG = totalB = totalA = count = 0;
                            for (int y2 = Math.Max(y - radius, 0); y2 <= Math.Min(y + radius, bmpData.Height - 1); y2++)
                            {
                                pixelCol = buffer[x, y2];
                                totalR += pixelCol.R;
                                totalG += pixelCol.G;
                                totalB += pixelCol.B;
                                totalA += pixelCol.A;
                                count++;
                            }
                            *(scan0 + (y * bmpData.Stride / sizeof(int)) + x) = Color.FromArgb(totalA / count, totalR / count, totalG / count, totalB / count).ToArgb();
                        }
                }

                bmp.UnlockBits(bmpData);
            }

            //full alpha
            colMatrix.Matrix33 = 1;
            imgAttr.SetColorMatrix(colMatrix);

            //main
            g.DrawImage(sourceBmp, new Rectangle(Point.Empty, bmp.Size), 0, 0, sourceBmp.Width, sourceBmp.Height, GraphicsUnit.Pixel, imgAttr);

            pictureBox1.Image?.Dispose();
            pictureBox1.Image = bmp;
            g.Dispose();
        }

        private void ButtonImportCircle_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Stream stream = ofd.OpenFile();

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(CircleData));
                try
                {
                    circleData = (CircleData)serializer.ReadObject(stream);
                    GenerateCircle(circleData);
                }
                catch (SerializationException)
                {
                    MessageBox.Show("Could not read circle data");
                }
                stream.Close();
            }
        }

        private void ButtonExportCircle_Click(object sender, EventArgs e)
        {
            if (!displayingCircle)
            {
                MessageBox.Show("Not displaying a circle");
                return;
            }

            Directory.CreateDirectory("output");
            string[] files = Directory.GetFiles("output");
            int i = 0;
            string fileBase = "circleData";
            string filename;
            do
            {
                filename = Path.Combine("output", fileBase + i + ".json");
                i++;
            } while (files.Contains(filename));

            FileStream fs = new FileStream(filename, FileMode.Create);
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(CircleData));
            serializer.WriteObject(fs, circleData);

            fs.Close();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!addedTime && e.KeyCode == Keys.T)
                AddTime();
        }

        private void AddTime()
        {
            if (!displayingCircle)
                return;

            addedTime = true;
            Bitmap blade1 = new Bitmap("blade1.png");
            Bitmap blade2 = new Bitmap("blade2.png");

            Graphics g = Graphics.FromImage(pictureBox1.Image);

            g.ResetTransform();
            g.RotateTransform(90 + 360f / 16 / 16, MatrixOrder.Append);
            g.TranslateTransform(pictureBox1.Image.Width / 2, pictureBox1.Image.Height / 2, MatrixOrder.Append);
            g.DrawImage(blade1, -blade1.Width / 2, -blade1.Height / 2);

            g.ResetTransform();
            g.RotateTransform(360 / 16, MatrixOrder.Append);
            g.TranslateTransform(pictureBox1.Image.Width / 2, pictureBox1.Image.Height / 2, MatrixOrder.Append);
            g.DrawImage(blade2, -blade2.Width / 2, -blade2.Height / 2);

            g.ResetTransform();

            g.Dispose();
            pictureBox1.Image = pictureBox1.Image;
        }

        private void ButtonImportRune_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Stream stream = ofd.OpenFile();

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RuneData));
                try
                {
                    runeData = (RuneData)serializer.ReadObject(stream);
                    GenerateRune(2, true, runeData);
                }
                catch (SerializationException)
                {
                    MessageBox.Show("Could not read rune data");
                }
                stream.Close();
            }
        }

        private void ButtonExportRune_Click(object sender, EventArgs e)
        {
            if (displayingCircle)
            {
                MessageBox.Show("Not displaying a rune");
                return;
            }

            Directory.CreateDirectory("output");
            string[] files = Directory.GetFiles("output");
            int i = 0;
            string fileBase = "runeData";
            string filename;
            do
            {
                filename = Path.Combine("output", fileBase + i + ".json");
                i++;
            } while (files.Contains(filename));

            FileStream fs = new FileStream(filename, FileMode.Create);
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RuneData));
            serializer.WriteObject(fs, runeData);

            fs.Close();
        }
    }
}
