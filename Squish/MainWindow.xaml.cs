using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Squish
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //double targetPos = 0;
        //double curVel = 0;

        int steps = 0;

        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //slider.Value += curVel;
            //curVel += (targetPos - slider.Value) / 2;
            //curVel *= 0.9;

            steps++;
            slider.Value = Math.Sin(steps / 3.0) / 2;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double value = Math.Pow(2, slider.Value);
            image2.Width = image.Width = 100 * value;
            image2.Height = image.Height = 100 / value;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //curVel += 0.5;
        }
    }
}
