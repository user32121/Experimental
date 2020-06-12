using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DotMaze
{
    public partial class DotMaze : Form
    {
        Random rng = new Random();
        bool paused;
        int fasterSpeed;
        int fasterSpeedMax;

        int maxMoves = 4000;
        int step;
        Bitmap bmp = new Bitmap(200, 200);
        Point goal = new Point(100, 10);
        Point start = new Point(100, 190);
        Rectangle[] obstacles;
        Dot[] dots = new Dot[100];

        public DotMaze()
        {
            InitializeComponent();

            obstacles = new Rectangle[] {
                new Rectangle(50, 100, 100, 10),
                new Rectangle(75, 100, 50, 30),

                new Rectangle(70, 0, 10, 50),
                new Rectangle(120, 0, 10, 50),
                new Rectangle(0, 40, 80, 10),
                new Rectangle(120, 40, 80, 10),
            };

            for (int i = 0; i < dots.Length; i++)
            {
                dots[i] = new Dot { moves = new Point[maxMoves], pos = start };
            }
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dots.Length; i++)
                for (int j = 0; j < dots[i].moves.Length; j++)
                    dots[i].moves[j] = new Point(rng.Next(-1, 2), rng.Next(-1, 2));

            Reset();
            timer1.Start();
        }
        private void ButtonPause_Click(object sender, EventArgs e)
        {
            if (paused)
            {
                timer1.Start();
                buttonPause.Text = "pause";
                paused = false;
            }
            else
            {
                timer1.Stop();
                buttonPause.Text = "resume";
                paused = true;
            }
        }
        private void ButtonStop_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }
        private void TextInterval_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textInterval.Text, out int num))
            {
                if (num > 0)
                {
                    timer1.Interval = num;
                    fasterSpeedMax = 0;
                }
            }
            else if (double.TryParse(textInterval.Text, out double value) && value > 0 && value < 1)
            {
                timer1.Interval = 1;
                fasterSpeedMax = (int)(1 / value);
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            do
            {
                fasterSpeed--;

                for (int i = 0; i < dots.Length; i++)
                    dots[i].Move(goal, bmp.Size, obstacles);

                if (step >= maxMoves)
                {
                    step = 0;
                    for (int i = 0; i < dots.Length; i++)
                        dots[i].Score(goal, bmp.Size, maxMoves);

                    Learn();
                    Reset();
                }
                else
                    step++;
            } while (fasterSpeed > 0);

            Render();       //visualization happens after calculations
            labelStep.Text = step.ToString();

            fasterSpeed = fasterSpeedMax;
        }

        void Render()
        {
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            //render start/goal
            g.FillRectangle(Brushes.LightBlue, start.X - 1, start.Y - 1, 3, 3);
            g.FillRectangle(Brushes.LightGreen, goal.X - 1, goal.Y - 1, 3, 3);

            //render obstacles
            g.FillRectangles(Brushes.SlateGray, obstacles);

            g.Dispose();

            //render dots
            for (int i = 0; i < dots.Length; i++)
            {
                bmp.SetPixel(dots[i].pos.X, dots[i].pos.Y, Color.Black);
            }
            pictureDisplay.Image = bmp;
        }

        void Reset()
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].pos = start;
                dots[i].step = 0;
                dots[i].atGoal = false;
                dots[i].dead = false;
            }
        }

        void Learn()
        {
            //"randomization"
            //for (int i = 0; i < dots.Length; i++)
            //    for (int j = 0; j < dots[i].moves.Length; j++)
            //        dots[i].moves[j] = new Point(rng.Next(-1, 2), rng.Next(-1, 2));

            //"random recombination"
            int first = 0;
            int second = 0;
            int third = 0;
            for (int i = 0; i < dots.Length; i++)
            {
                if (dots[i].score > dots[first].score)
                    first = i;
                if (dots[i].score > dots[second].score && i != first)
                    second = i;
                if (dots[i].score > dots[third].score && i != first && i != second)
                    third = i;
            }
            labelScore.Text = dots[first].score.ToString();
            Point[] moves = new Point[maxMoves];
            dots[first].moves.CopyTo(moves, 0);

            int rndDot;
            for (int i = 0; i < 50; i++)   //first gets 50
            {
                rndDot = rng.Next(dots.Length);
                if (rndDot != first && rndDot != second && rndDot != third)
                    for (int j = 0; j < maxMoves; j++)
                        dots[rndDot].moves[j] = dots[first].moves[j];
            }
            for (int i = 0; i < 40; i++)   //second gets 40
            {
                rndDot = rng.Next(dots.Length);
                if (rndDot != first && rndDot != second && rndDot != third)
                    for (int j = 0; j < maxMoves; j++)
                        dots[rndDot].moves[j] = dots[second].moves[j];
            }
            for (int i = 0; i < 30; i++)   //third gets 30
            {
                rndDot = rng.Next(dots.Length);
                if (rndDot != first && rndDot != second && rndDot != third)
                    for (int j = 0; j < maxMoves; j++)
                        dots[rndDot].moves[j] = dots[third].moves[j];
            }
            for (int i = 0; i < 10; i++)    //random 10
            {
                rndDot = rng.Next(dots.Length);
                if (rndDot != first && rndDot != second && rndDot != third)
                    for (int j = 0; j < maxMoves; j++)
                        dots[rndDot].moves[j] = new Point(rng.Next(-1, 2), rng.Next(-1, 2));
            }

            //"mutations"
            for (int i = 0; i < rng.Next(maxMoves * dots.Length); i++)
            {
                rndDot = rng.Next(dots.Length);
                if (rndDot != first && rndDot != second && rndDot != third)
                    dots[rndDot].moves[rng.Next(maxMoves - 1)] = new Point(rng.Next(-1, 2), rng.Next(-1, 2));
            }
        }
    }

    class Dot
    {
        public Point pos;
        public Point[] moves;
        public int step;
        public bool atGoal;
        public bool dead;
        public double score = 0;

        public void Move(Point goal, Size boundary, Rectangle[] walls)
        {
            if (atGoal || step >= moves.Length || dead)
                return;

            pos = Point.Add(pos, (Size)moves[step]);    //move

            //edge detection
            if (pos.X < 0)
            { pos.X++; dead = true; }
            if (pos.X >= boundary.Width)
            { pos.X--; dead = true; }
            if (pos.Y < 0)
            { pos.Y++; dead = true; }
            if (pos.Y >= boundary.Height)
            { pos.Y--; dead = true; }

            foreach (Rectangle wall in walls)        //obstacle detection
                if (pos.X >= wall.X && pos.X < wall.X + wall.Width && pos.Y >= wall.Y && pos.Y < wall.Y + wall.Height)
                    dead = true;

            if (pos == goal)    //goal detection
                atGoal = true;

            step++;
        }

        public void Score(Point goal, Size boundary, int maxTime)
        {
            //scoring system (dist, goal, time or dead)

            double scDist = 1 - (double)Math.Abs(goal.Y - pos.Y) / boundary.Height +
                (1 - (double)Math.Abs(goal.X - pos.X) / boundary.Width) / 2;

            int scGoal = atGoal ? 1 : 0;

            double scTime = 1 - (double)step / maxTime;
            if (dead)
            { scTime *= -1; scTime -= 1; }

            score = scDist + scGoal + scTime;
        }
    }
}
