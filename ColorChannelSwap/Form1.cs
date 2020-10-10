using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ColorChannelSwap
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            comboRed.SelectedIndex = 0;
            comboGreen.SelectedIndex = 1;
            comboBlue.SelectedIndex = 2;
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK && saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream stream = openFileDialog1.OpenFile();
                Bitmap bmpOld = new Bitmap(stream);
                stream.Close();
                Bitmap bmpNew = new Bitmap(bmpOld.Width, bmpOld.Height);

                Color colOld;
                int r, g, b;
                for (int x = 0; x < bmpOld.Width; x++)
                    for (int y = 0; y < bmpOld.Height; y++)
                    {
                        colOld = bmpOld.GetPixel(x, y);
                        switch (comboRed.SelectedItem)
                        {
                            case "Red":
                                r = colOld.R;
                                break;
                            case "Green":
                                r = colOld.G;
                                break;
                            case "Blue":
                                r = colOld.B;
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        switch (comboGreen.SelectedItem)
                        {
                            case "Red":
                                g = colOld.R;
                                break;
                            case "Green":
                                g = colOld.G;
                                break;
                            case "Blue":
                                g = colOld.B;
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        switch (comboBlue.SelectedItem)
                        {
                            case "Red":
                                b = colOld.R;
                                break;
                            case "Green":
                                b = colOld.G;
                                break;
                            case "Blue":
                                b = colOld.B;
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        bmpNew.SetPixel(x, y, Color.FromArgb(r, g, b));
                    }
                stream = saveFileDialog1.OpenFile();
                bmpNew.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();
            }
        }
    }
}
