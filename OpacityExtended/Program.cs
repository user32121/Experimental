using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpacityExtended
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter path to base image: ");
            string strBmpBase = Console.ReadLine();
            Bitmap bmpBase = new Bitmap(strBmpBase);
            Console.Write("Enter path to final image: ");
            string strBmpFinal = Console.ReadLine();
            Bitmap bmpFinal = new Bitmap(strBmpFinal);
            bmpFinal = new Bitmap(bmpFinal, bmpBase.Width, bmpBase.Height);

            Bitmap bmpOutput = new Bitmap(bmpBase.Width, bmpBase.Height);

            Directory.CreateDirectory("opacity extended output");

            while (true)
            {
                Console.Write("\nEnter opacity: ");
                if (double.TryParse(Console.ReadLine(), out double opacity))
                {
#if normalized
                    BlendImagesNormalized(bmpBase, bmpFinal, opacity, ref bmpOutput);
#else
                    for (int x = 0; x < bmpBase.Width; x++)
                        for (int y = 0; y < bmpBase.Height; y++)
                            bmpOutput.SetPixel(x, y, BlendPixels(bmpBase.GetPixel(x, y), bmpFinal.GetPixel(x, y), opacity));
#endif
                    bmpOutput.Save(string.Format("opacity extended output\\output{0:f3}.png", opacity));
                }
                else
                    Console.WriteLine("invalid number");
            }
        }

        //blends base and final colors based on given opacity
        static Color BlendPixels(Color b, Color f, double o)
        {
            return Color.FromArgb(
                Math.Min(Math.Max((int)(b.R * (1 - o) + f.R * o), 0), 255),
                Math.Min(Math.Max((int)(b.G * (1 - o) + f.G * o), 0), 255),
                Math.Min(Math.Max((int)(b.B * (1 - o) + f.B * o), 0), 255)
                );
        }

        //blends base and final colors based on given opacity and normalizes the output if it becomes saturated
        static void BlendImagesNormalized(Bitmap b, Bitmap f, double o, ref Bitmap output)
        {
            //array of non normalized pixels
            (double, double, double)[,] pixels = new (double, double, double)[output.Width, output.Height];
            double minR, maxR, minG, maxG, minB, maxB;
            minR = minG = minB = 0;
            maxR = maxG = maxB = 0;

            for (int x = 0; x < b.Width; x++)
                for (int y = 0; y < b.Height; y++)
                {
                    Color colB = b.GetPixel(x, y);
                    Color colF = f.GetPixel(x, y);
                    pixels[x, y] = (
                        (colB.R * (1 - o) + colF.R * o),
                        (colB.G * (1 - o) + colF.G * o),
                        (colB.B * (1 - o) + colF.B * o));

                    if (pixels[x, y].Item1 < minR)
                        minR = pixels[x, y].Item1;
                    else if (pixels[x, y].Item1 > maxR)
                        maxR = pixels[x, y].Item1;
                    if (pixels[x, y].Item2 < minG)
                        minG = pixels[x, y].Item2;
                    else if (pixels[x, y].Item2 > maxG)
                        maxG = pixels[x, y].Item2;
                    if (pixels[x, y].Item3 < minB)
                        minB = pixels[x, y].Item3;
                    else if (pixels[x, y].Item3 > maxB)
                        maxB = pixels[x, y].Item3;
                }

            for (int x = 0; x < b.Width; x++)
                for (int y = 0; y < b.Height; y++)
                {
                    output.SetPixel(x, y, Color.FromArgb(
                        (int)((pixels[x, y].Item1 - minR) / (maxR - minR) * 255),
                        (int)((pixels[x, y].Item2 - minG) / (maxG - minG) * 255),
                        (int)((pixels[x, y].Item3 - minB) / (maxB - minB) * 255)
                        ));
                }
        }
    }
}
