using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Tesseract;

namespace OCR
{
    class Program
    {
        static void Main(string[] args)
        {
            TesseractEngine engine = new TesseractEngine(@"./tessdata", "digits", EngineMode.Default, "tessConfig.txt");
            Console.WriteLine("tesseract version: {0}", engine.Version);

            while (true)
            {
                Console.WriteLine("enter path to image file:");
                string path = Console.ReadLine();
                path.Trim('\'');

                if (!File.Exists(path))
                {
                    Console.WriteLine("file does not exist");
                    continue;
                }
                Pix img = Pix.LoadFromFile(path);
                Page page = engine.Process(img);

                string dirPath = Path.Combine("output", Path.GetFileNameWithoutExtension(path));
                if (Directory.Exists(dirPath))
                    Directory.Delete(dirPath, true);
                Directory.CreateDirectory(dirPath);

                ResultIterator iter = page.GetIterator();
                if (iter.Next(PageIteratorLevel.Symbol))
                {
                    iter.Begin();
                    do
                    {
                        Console.WriteLine("\"{0}\" (confidence: {1})", iter.GetText(PageIteratorLevel.Symbol), iter.GetConfidence(PageIteratorLevel.Symbol));
                        Pix pix = iter.GetImage(PageIteratorLevel.Symbol, 10, out int x, out int y);
                        pix.Save(string.Format("output/{0}/img_{1}_{2}___{3}_{4}.png", Path.GetFileNameWithoutExtension(path), iter.GetText(PageIteratorLevel.Symbol), (int)iter.GetConfidence(PageIteratorLevel.Symbol), x, y));
                    } while (iter.Next(PageIteratorLevel.Symbol));
                }
                else
                    Console.WriteLine("nothing detected");

                page.Dispose();
                img.Dispose();
            }
            engine.Dispose();
        }
    }
}
