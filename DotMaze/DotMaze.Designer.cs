namespace DotMaze
{
    partial class DotMaze
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
            this.components = new System.ComponentModel.Container();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonPause = new System.Windows.Forms.Button();
            this.pictureDisplay = new System.Windows.Forms.PictureBox();
            this.textInterval = new System.Windows.Forms.TextBox();
            this.labelStep = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.labelScore = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(13, 13);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // buttonPause
            // 
            this.buttonPause.Location = new System.Drawing.Point(94, 13);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(75, 23);
            this.buttonPause.TabIndex = 1;
            this.buttonPause.Text = "pause";
            this.buttonPause.UseVisualStyleBackColor = true;
            this.buttonPause.Click += new System.EventHandler(this.ButtonPause_Click);
            // 
            // pictureDisplay
            // 
            this.pictureDisplay.BackColor = System.Drawing.Color.White;
            this.pictureDisplay.Location = new System.Drawing.Point(13, 43);
            this.pictureDisplay.Name = "pictureDisplay";
            this.pictureDisplay.Size = new System.Drawing.Size(400, 400);
            this.pictureDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureDisplay.TabIndex = 2;
            this.pictureDisplay.TabStop = false;
            // 
            // textInterval
            // 
            this.textInterval.Location = new System.Drawing.Point(175, 15);
            this.textInterval.Name = "textInterval";
            this.textInterval.Size = new System.Drawing.Size(50, 20);
            this.textInterval.TabIndex = 3;
            this.textInterval.Text = "100";
            this.textInterval.TextChanged += new System.EventHandler(this.TextInterval_TextChanged);
            // 
            // labelStep
            // 
            this.labelStep.AutoSize = true;
            this.labelStep.Location = new System.Drawing.Point(231, 18);
            this.labelStep.Name = "labelStep";
            this.labelStep.Size = new System.Drawing.Size(13, 13);
            this.labelStep.TabIndex = 4;
            this.labelStep.Text = "0";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // labelScore
            // 
            this.labelScore.AutoSize = true;
            this.labelScore.Location = new System.Drawing.Point(300, 18);
            this.labelScore.Name = "labelScore";
            this.labelScore.Size = new System.Drawing.Size(13, 13);
            this.labelScore.TabIndex = 5;
            this.labelScore.Text = "0";
            // 
            // DotMaze
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 450);
            this.Controls.Add(this.labelScore);
            this.Controls.Add(this.labelStep);
            this.Controls.Add(this.textInterval);
            this.Controls.Add(this.pictureDisplay);
            this.Controls.Add(this.buttonPause);
            this.Controls.Add(this.buttonStart);
            this.Name = "DotMaze";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureDisplay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonPause;
        private System.Windows.Forms.PictureBox pictureDisplay;
        private System.Windows.Forms.TextBox textInterval;
        private System.Windows.Forms.Label labelStep;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label labelScore;
    }
}

