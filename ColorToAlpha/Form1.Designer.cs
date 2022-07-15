
namespace ColorToAlpha
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.trackThreshold = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureColorDisplay = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labelThreshold = new System.Windows.Forms.Label();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.pictureDisplay = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.trackThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureColorDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // trackThreshold
            // 
            this.trackThreshold.Location = new System.Drawing.Point(78, 30);
            this.trackThreshold.Maximum = 100;
            this.trackThreshold.Name = "trackThreshold";
            this.trackThreshold.Size = new System.Drawing.Size(104, 45);
            this.trackThreshold.TabIndex = 0;
            this.trackThreshold.TickFrequency = 25;
            this.trackThreshold.Scroll += new System.EventHandler(this.trackThreshold_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Color:";
            // 
            // pictureColorDisplay
            // 
            this.pictureColorDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureColorDisplay.Location = new System.Drawing.Point(52, 9);
            this.pictureColorDisplay.Name = "pictureColorDisplay";
            this.pictureColorDisplay.Size = new System.Drawing.Size(15, 15);
            this.pictureColorDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureColorDisplay.TabIndex = 2;
            this.pictureColorDisplay.TabStop = false;
            this.pictureColorDisplay.Click += new System.EventHandler(this.pictureColorDisplay_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Threshold: ";
            // 
            // labelThreshold
            // 
            this.labelThreshold.AutoSize = true;
            this.labelThreshold.Location = new System.Drawing.Point(188, 30);
            this.labelThreshold.Name = "labelThreshold";
            this.labelThreshold.Size = new System.Drawing.Size(21, 13);
            this.labelThreshold.TabIndex = 4;
            this.labelThreshold.Text = "0%";
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(12, 81);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 23);
            this.buttonLoad.TabIndex = 5;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // pictureDisplay
            // 
            this.pictureDisplay.BackColor = System.Drawing.SystemColors.Control;
            this.pictureDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureDisplay.Location = new System.Drawing.Point(12, 110);
            this.pictureDisplay.Name = "pictureDisplay";
            this.pictureDisplay.Size = new System.Drawing.Size(100, 50);
            this.pictureDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureDisplay.TabIndex = 6;
            this.pictureDisplay.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureDisplay);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.labelThreshold);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureColorDisplay);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackThreshold);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.trackThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureColorDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureDisplay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.TrackBar trackThreshold;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureColorDisplay;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelThreshold;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.PictureBox pictureDisplay;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

