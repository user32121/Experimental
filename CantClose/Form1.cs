using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CantClose
{
    public partial class Form1 : Form
    {
        int closeCounter = 0;

        string[] args;

        public Form1(string[] args)
        {
            this.args = args;

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (args.Length > 0)
                int.TryParse(args[0], out closeCounter);
            string s = "";
            if (args.Length > 1)
                s = string.Join(" ", args, 1, args.Length - 1);

            labelDisplay.Text = string.Format("{0}: {1}", closeCounter, s);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                closeCounter++;
                Process.Start(System.Reflection.Assembly.GetEntryAssembly().Location, closeCounter + " " + e.CloseReason);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
