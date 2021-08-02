using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeySpam
{
    class Program
    {
        static void Main(string[] args)
        {
            //get user input
            string input = string.Join(" ", args).ToLower();

            //setup dictionary
            Dictionary<char, Point> charToPos = new Dictionary<char, Point>() {
                //letters
                //row 2
                { 'q', new Point(117, 90) },
                { 'w', new Point(170, 90) },
                { 'e', new Point(224, 90) },
                { 'r', new Point(276, 90) },
                { 't', new Point(331, 90) },
                { 'y', new Point(387, 90) },
                { 'u', new Point(440, 90) },
                { 'i', new Point(494, 90) },
                { 'o', new Point(547, 90) },
                { 'p', new Point(600, 90) },
                //row 3
                { 'a', new Point(129, 143) },
                { 's', new Point(183, 143) },
                { 'd', new Point(237, 143) },
                { 'f', new Point(291, 143) },
                { 'g', new Point(345, 143) },
                { 'h', new Point(401, 143) },
                { 'j', new Point(454, 143) },
                { 'k', new Point(506, 143) },
                { 'l', new Point(561, 143) },
                //row 4
                { 'z', new Point(158, 194) },
                { 'x', new Point(212, 194) },
                { 'c', new Point(267, 194) },
                { 'v', new Point(320, 194) },
                { 'b', new Point(375, 194) },
                { 'n', new Point(428, 194) },
                { 'm', new Point(481, 194) },

                //numbers
                { '1', new Point(89,  38) },
                { '2', new Point(144, 38) },
                { '3', new Point(198, 38) },
                { '4', new Point(252, 38) },
                { '5', new Point(306, 38) },
                { '6', new Point(360, 38) },
                { '7', new Point(414, 38) },
                { '8', new Point(467, 38) },
                { '9', new Point(522, 38) },
                { '0', new Point(575, 38) },
                //number symbols
                { '!', new Point(89,  38) },
                { '@', new Point(144, 38) },
                { '#', new Point(198, 38) },
                { '$', new Point(252, 38) },
                { '%', new Point(306, 38) },
                { '^', new Point(360, 38) },
                { '&', new Point(414, 38) },
                { '*', new Point(467, 38) },
                { '(', new Point(522, 38) },
                { ')', new Point(575, 38) },

                //other symbols
                //row 1
                { '~', new Point(36,  37) },
                { '`', new Point(36,  37) },
                { '-', new Point(629, 37) },
                { '_', new Point(629, 37) },
                { '=', new Point(683, 37) },
                { '+', new Point(683, 37) },
                //row 2
                { '[', new Point( 655, 89) },
                { '{', new Point( 655, 89) },
                { ']', new Point( 709, 89) },
                { '}', new Point( 709, 89) },
                { '\\', new Point(762, 89) },
                { '|', new Point( 762, 89) },
                //row 3
                { ';', new Point( 614, 142) },
                { ':', new Point( 614, 142) },
                { '\'', new Point(667, 142) },
                { '"', new Point( 667, 142) },
                //row 4
                { ',', new Point(535, 196) },
                { '<', new Point(535, 196) },
                { '.', new Point(590, 196) },
                { '>', new Point(590, 196) },
                { '/', new Point(644, 196) },
                { '?', new Point(644, 196) },
            };

            Bitmap bmp = new Bitmap("modules\\keyboard.jpg");
            Graphics g = Graphics.FromImage(bmp);
            HashSet<char> skippedChars = new HashSet<char>();
            Point p1 = new Point(-1, -1);
            Point p2 = new Point(-1, -1);
            List<Point> line1 = new List<Point>();
            List<Point> line2 = new List<Point>();
            Random rng = new Random();

            //start processing
            for (int i = 0; i < input.Length; i++)
            {
                if (charToPos.TryGetValue(input[i], out Point pos))
                {
                    //check if either point is empty
                    pos.Offset(rng.Next(-10, 10), rng.Next(-10, 10));
                    if (p1.X == -1)
                    {
                        line1.Add(pos);
                        p1 = pos;
                    }
                    else if (p2.X == -1)
                    {
                        line2.Add(pos);
                        p2 = pos;
                    }
                    //find closer point
                    else if (getSqrDistBtwPts(p1, pos) < getSqrDistBtwPts(p2, pos))
                    {
                        line1.Add(pos);
                        p1 = pos;
                    }
                    else
                    {
                        line2.Add(pos);
                        p2 = pos;
                    }
                }
                else
                    skippedChars.Add(input[i]);
            }

            //draw lines
            if (line1.Count >= 3)
                g.DrawCurve(Pens.Red, line1.ToArray());
            else if (line1.Count > 1)
                g.DrawLines(Pens.Red, line1.ToArray());
            if (line2.Count >= 3)
                g.DrawCurve(Pens.Blue, line2.ToArray());
            else if (line2.Count > 1)
                g.DrawLines(Pens.Blue, line2.ToArray());

            if (skippedChars.Count > 0)
                Console.WriteLine(string.Format("Skipped characters: \"{0}\"", string.Join("\", \"", skippedChars)));

            bmp.Save("modules\\keyboardNew.png");
        }

        static int getSqrDistBtwPts(Point p1, Point p2)
        {
            int dx = p1.X - p2.X;
            int dy = p1.Y - p2.Y;
            return dx * dx + dy + dy;
        }
    }
}
