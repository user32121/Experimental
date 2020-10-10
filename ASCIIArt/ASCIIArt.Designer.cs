namespace ASCIIArt
{
    partial class ASCIIArt
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
            this.btnOpen = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pictureDisplay = new System.Windows.Forms.PictureBox();
            this.textDisplay = new System.Windows.Forms.RichTextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.textFont = new System.Windows.Forms.TextBox();
            this.labelProgress = new System.Windows.Forms.Label();
            this.buttonLoad = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.AutoSize = true;
            this.btnOpen.Location = new System.Drawing.Point(12, 50);
            this.btnOpen.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(100, 30);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "bitmaps|*.bmp;*.gif;*.jpg;*.jpeg;*.jpe;*.jif;*.jfif;*.png;*.tiff*.tif;";
            // 
            // pictureDisplay
            // 
            this.pictureDisplay.BackColor = System.Drawing.Color.White;
            this.pictureDisplay.Location = new System.Drawing.Point(12, 128);
            this.pictureDisplay.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureDisplay.Name = "pictureDisplay";
            this.pictureDisplay.Size = new System.Drawing.Size(100, 50);
            this.pictureDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureDisplay.TabIndex = 1;
            this.pictureDisplay.TabStop = false;
            // 
            // textDisplay
            // 
            this.textDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textDisplay.Font = new System.Drawing.Font("Consolas", 8F);
            this.textDisplay.Location = new System.Drawing.Point(122, 128);
            this.textDisplay.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textDisplay.Name = "textDisplay";
            this.textDisplay.Size = new System.Drawing.Size(148, 75);
            this.textDisplay.TabIndex = 3;
            this.textDisplay.Text = "";
            this.textDisplay.WordWrap = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 90);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(100, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // textFont
            // 
            this.textFont.Location = new System.Drawing.Point(120, 13);
            this.textFont.Name = "textFont";
            this.textFont.Size = new System.Drawing.Size(200, 26);
            this.textFont.TabIndex = 5;
            // 
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.Location = new System.Drawing.Point(118, 55);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(60, 20);
            this.labelProgress.TabIndex = 6;
            this.labelProgress.Text = "loading";
            // 
            // buttonLoad
            // 
            this.buttonLoad.AutoSize = true;
            this.buttonLoad.Location = new System.Drawing.Point(12, 12);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(100, 30);
            this.buttonLoad.TabIndex = 7;
            this.buttonLoad.Text = "reload";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.ButtonLoad_Click);
            // 
            // ASCIIArt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(800, 449);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.labelProgress);
            this.Controls.Add(this.textFont);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.textDisplay);
            this.Controls.Add(this.pictureDisplay);
            this.Controls.Add(this.btnOpen);
            this.Name = "ASCIIArt";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.ASCIIArt_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureDisplay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.PictureBox pictureDisplay;
        private System.Windows.Forms.RichTextBox textDisplay;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox textFont;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.Button buttonLoad;
    }
}

