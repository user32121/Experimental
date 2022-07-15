using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagicCircles
{
    public partial class Form1 : Form
    {
        Random rng = new Random();

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

        private readonly float[] threePoints = new float[] { 0.2f, 0.5f, 0.8f };
        private readonly float[] fourPoints = new float[] { 0.2f, 0.4f, 0.6f, 0.8f };
        enum LINE_TYPE
        {
            STRAIGHT,
            SPLINE,
            BEZIER,
            ARC,
        }
        private readonly LINE_TYPE[] availableLineTypes = new LINE_TYPE[]
        {
            LINE_TYPE.STRAIGHT,
            LINE_TYPE.SPLINE,
            LINE_TYPE.BEZIER,
        };
        private Bitmap GenerateRune(int thickness, bool display)
        {
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
            for (int i = 0; i < 5; i++)
            {
                PointF p1 = runePoints[rng.Next(runePoints.Count)].Scale(bmp.Size);
                PointF p2 = runePoints[rng.Next(runePoints.Count)].Scale(bmp.Size);
                switch (availableLineTypes[rng.Next(availableLineTypes.Length)])
                {
                    case LINE_TYPE.STRAIGHT:
                        {
                            g.DrawLine(pen, p1, p2);
                            break;
                        }
                    case LINE_TYPE.SPLINE:
                        {
                            bool b1 = rng.NextDouble() < 0.5;
                            PointF p3;
                            if (b1)
                                p3 = new PointF(p1.X, p2.Y);
                            else
                                p3 = new PointF(p2.X, p1.Y);
                            p3 = new PointF((p1.X + p2.X + p3.X * 2) / 4, (p1.Y + p2.Y + p3.Y * 2) / 4);
                            g.DrawCurve(pen, new PointF[] { p1, p3, p2 });
                            break;
                        }
                    case LINE_TYPE.BEZIER:
                        {
                            bool b1 = rng.NextDouble() < 0.5;
                            PointF p3, p4;
                            if (b1)
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
                pictureBox1.Image = bmp;
            return bmp;
        }

        enum LAYER_PATTERN
        {
            DIAGONALS,
            DOUBLE_DIAGONALS,
            RUNES,
            POLYGONS,
            CIRCLES,
            CUSTOM,
        }
        private readonly LAYER_PATTERN[] availablePatterns = new LAYER_PATTERN[]
        {
            LAYER_PATTERN.DIAGONALS,
            LAYER_PATTERN.DOUBLE_DIAGONALS,
            LAYER_PATTERN.RUNES,
        };
        enum LAYER_BORDER
        {
            SOLID,
            DASHED,
            SPIKES,
            CROSSED,
        }
        private readonly LAYER_BORDER[] availableBorders = new LAYER_BORDER[]
        {
            LAYER_BORDER.SOLID,
            LAYER_BORDER.DASHED,
            LAYER_BORDER.SPIKES,
            LAYER_BORDER.CROSSED,
        };
        private void GenerateCircle()
        {
            Bitmap bmp = new Bitmap(512, 512);
            Graphics g = Graphics.FromImage(bmp);
            SizeF center = new SizeF(bmp.Width / 2, bmp.Height / 2);

            //generation parameters
            int minRadius = rng.Next((bmp.Width / 2) * 4 / 5);
            int layers = rng.Next(2, Math.Max(2, (bmp.Width / 2 - minRadius) * 10 / (bmp.Width / 2)));
            float[] layerSizes = new float[layers];
            layerSizes[0] = 0;
            //distribute random thicknesses, then scale to fit the system
            float totalThickness = 0;
            for (int i = 0; i < layers; i++)
                totalThickness += (layerSizes[i] = rng.Next(1, 11));
            for (int i = 0; i < layers; i++)
                layerSizes[i] = layerSizes[i] / totalThickness * (bmp.Width / 2 - minRadius);  //[0,arbitrary] -> [0,1] -> [minRadius, width/2]

            float curRadius = minRadius;
            for (int i = 0; i < layers; i++)
            {
                LAYER_PATTERN pattern = availablePatterns[rng.Next(availablePatterns.Length)];
                switch (pattern)
                {
                    case LAYER_PATTERN.DIAGONALS:
                        {
                            bool flipped = rng.NextDouble() < 0.5f;
                            double delta = Math.PI * 2 / rng.Next(10, 30);
                            for (double theta = 0; theta < Math.PI * 2; theta += 0.1)
                            {
                                PointF p1 = PointFFromRadial(theta, curRadius);
                                PointF p2 = PointFFromRadial(theta + (flipped ? -0.1 : 0.1), curRadius + layerSizes[i]);
                                p1 += center;
                                p2 += center;
                                g.DrawLine(Pens.Black, p1, p2);
                            }
                            break;
                        }
                    case LAYER_PATTERN.DOUBLE_DIAGONALS:
                        {
                            double delta = Math.PI * 2 / rng.Next(10, 30);
                            for (double theta = 0; theta < Math.PI * 2; theta += 0.1)
                            {
                                PointF p1 = PointFFromRadial(theta, curRadius);
                                PointF p2 = PointFFromRadial(theta + 0.1, curRadius + layerSizes[i]);
                                PointF p3 = PointFFromRadial(theta - 0.1, curRadius + layerSizes[i]);
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
                            if (layerSizes[i] == 0)
                                break;

                            int count = rng.Next(20, 40);
                            for (int j = 0; j < count; j++)
                            {
                                float theta = (float)(j * Math.PI * 2 / count);
                                PointF pos = PointFFromRadial(theta, curRadius);
                                Bitmap runeBmp = GenerateRune(3, false);
                                g.ResetTransform();
                                float scale = layerSizes[i] / runeBmp.Height;
                                PointF offset = PointFFromRadial(theta + Math.PI / 2, runeBmp.Width / 2).Scale(scale);  //create an offset to align the center of the card to the tangent of the circle
                                g.ScaleTransform(scale, scale, MatrixOrder.Append);
                                g.RotateTransform((float)(theta / Math.PI * 180 - 90), MatrixOrder.Append);
                                g.TranslateTransform(center.Width + pos.X + offset.X, center.Height + pos.Y + offset.Y, MatrixOrder.Append);
                                g.DrawImage(runeBmp, 0, 0);
                                g.ResetTransform();
                            }
                            break;
                        }
                    default:
                        throw new NotImplementedException(pattern + " is not implemented");
                }
                curRadius += layerSizes[i];

                LAYER_BORDER border = availableBorders[rng.Next(availableBorders.Length)];
                switch (border)
                {
                    case LAYER_BORDER.SOLID:
                        {
                            g.DrawEllipse(Pens.Black, center.Width - curRadius, center.Height - curRadius, curRadius * 2, curRadius * 2);
                            break;
                        }
                    case LAYER_BORDER.DASHED:
                        {
                            int count = rng.Next(2, 15) * 2;
                            float thetaOffset = rng.Next(360);
                            float dashLengthModifier = (float)rng.NextDouble() * 2;
                            for (int j = 0; j < count; j++)
                                if (j % 2 == 0)
                                    g.DrawArc(Pens.Black, center.Width - curRadius, center.Height - curRadius, curRadius * 2, curRadius * 2, j * 360 / count + thetaOffset, dashLengthModifier * 360 / count);
                            break;
                        }
                    case LAYER_BORDER.SPIKES:
                        {
                            g.DrawEllipse(Pens.Black, center.Width - curRadius, center.Height - curRadius, curRadius * 2, curRadius * 2);
                            int count = rng.Next(10, 30);
                            double spikeWidth = rng.NextDouble() / 10;  //in radians
                            int spikeLength = rng.Next((int)layerSizes[i] / 2);
                            for (int j = 0; j < count; j++)
                            {
                                double theta = j * Math.PI * 2 / count;
                                PointF p1 = PointFFromRadial(theta, curRadius);
                                PointF p2 = PointFFromRadial(theta + spikeWidth, curRadius);
                                PointF p3 = PointFFromRadial(theta + spikeWidth / 2, curRadius + spikeLength);
                                PointF p4 = PointFFromRadial(theta + spikeWidth / 2, curRadius - spikeLength);
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
                            int count = rng.Next(10, 30);
                            int hashLength = rng.Next((int)layerSizes[i] / 2);
                            double hashWidth = hashLength / 5;
                            for (int j = 0; j < count; j++)
                            {
                                double theta = j * Math.PI * 2 / count;
                                PointF p1 = PointFFromRadial(theta, curRadius - hashLength);
                                PointF p2 = PointFFromRadial(theta, curRadius + hashLength);
                                p1 += center;
                                p2 += center;
                                PointF t = PointFFromRadial(theta + Math.PI / 2, hashWidth / 2);
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

            pictureBox1.Image = bmp;
        }

        private PointF PointFFromRadial(double angle, double magnitude)
        {
            return new PointF((float)Math.Cos(angle), (float)Math.Sin(angle)).Scale((float)magnitude);
        }

        private void buttonSave_Click(object sender, EventArgs e)
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
    }
}
