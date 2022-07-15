using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace GifMaker
{
    internal class Program
    {
        //simple tool that takes a folder of images and makes a gif
        static void Main(string[] args)
        {
            Console.Write("Folder path: ");
            string path = Console.ReadLine();
            GifBitmapEncoder encoder = new GifBitmapEncoder();
            string[] files;
            long totalSize = 0;
            try
            {
                files = Directory.GetFiles(path);
                Array.Sort(files);
                Console.WriteLine("found {0} files, loading files...", files.Length);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                return;
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                return;
            }
            foreach (string file in files)
            {
                if (file.EndsWith(".gif"))
                {
                    Console.WriteLine("skipping {0} because it ends with .gif", file);
                    continue;
                }
                try
                {
                    FileInfo info = new FileInfo(file);
                    totalSize += info.Length;
                    encoder.Frames.Add(BitmapFrame.Create(new Uri(file), BitmapCreateOptions.DelayCreation, BitmapCacheOption.None));
                }
                catch (NotSupportedException e)
                {
                    Console.WriteLine("Could not load {0}: {1}", file, e.Message);
                }
            }

            if (encoder.Frames.Count == 0)
            {
                Console.WriteLine("0 frames loaded, exiting");
                Console.ReadLine();
                return;
            }
            else
            {
                Console.WriteLine("loaded {0} files ({1} bytes), saving...", encoder.Frames.Count, totalSize);
            }

            encoder.Save(new FileStream(Path.Combine(path, "file.gif"), FileMode.Create));
        }
    }
}
