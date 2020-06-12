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
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pictureDisplay = new System.Windows.Forms.PictureBox();
            this.textDisplay = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(20, 20);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 35);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "bitmaps|*.bmp;*.gif;*.jpg;*.jpeg;*.jpe;*.jif;*.jfif;*.png;*.tiff*.tif;";
            // 
            // pictureDisplay
            // 
            this.pictureDisplay.BackColor = System.Drawing.Color.White;
            this.pictureDisplay.Location = new System.Drawing.Point(20, 66);
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
            this.textDisplay.Font = new System.Drawing.Font("Consolas", 2F);
            this.textDisplay.Location = new System.Drawing.Point(178, 66);
            this.textDisplay.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textDisplay.Name = "textDisplay";
            this.textDisplay.Size = new System.Drawing.Size(148, 75);
            this.textDisplay.TabIndex = 3;
            this.textDisplay.Text = "";
            this.textDisplay.WordWrap = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(800, 449);
            this.Controls.Add(this.textDisplay);
            this.Controls.Add(this.pictureDisplay);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureDisplay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.PictureBox pictureDisplay;
        private System.Windows.Forms.RichTextBox textDisplay;
    }
}

