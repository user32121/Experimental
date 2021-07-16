namespace ShapeColorGenerator
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
            this.textCount = new System.Windows.Forms.TextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.checkXY = new System.Windows.Forms.CheckBox();
            this.pictureDisplay = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // textCount
            // 
            this.textCount.Location = new System.Drawing.Point(12, 12);
            this.textCount.Name = "textCount";
            this.textCount.Size = new System.Drawing.Size(100, 26);
            this.textCount.TabIndex = 0;
            this.textCount.Text = "1";
            // 
            // buttonStart
            // 
            this.buttonStart.AutoSize = true;
            this.buttonStart.Location = new System.Drawing.Point(118, 12);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(87, 30);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Generate";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // checkXY
            // 
            this.checkXY.AutoSize = true;
            this.checkXY.Checked = true;
            this.checkXY.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkXY.Location = new System.Drawing.Point(12, 44);
            this.checkXY.Name = "checkXY";
            this.checkXY.Size = new System.Drawing.Size(103, 24);
            this.checkXY.TabIndex = 2;
            this.checkXY.Text = "x y format";
            this.checkXY.UseVisualStyleBackColor = true;
            // 
            // pictureDisplay
            // 
            this.pictureDisplay.BackColor = System.Drawing.Color.White;
            this.pictureDisplay.Location = new System.Drawing.Point(12, 74);
            this.pictureDisplay.Name = "pictureDisplay";
            this.pictureDisplay.Size = new System.Drawing.Size(100, 50);
            this.pictureDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureDisplay.TabIndex = 3;
            this.pictureDisplay.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureDisplay);
            this.Controls.Add(this.checkXY);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.textCount);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureDisplay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textCount;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.CheckBox checkXY;
        private System.Windows.Forms.PictureBox pictureDisplay;
    }
}

