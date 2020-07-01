namespace ProceduralPathways
{
    partial class AdditionMethod
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonStep = new System.Windows.Forms.Button();
            this.checkPlay = new System.Windows.Forms.CheckBox();
            this.checkRender = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSavePalette = new System.Windows.Forms.Button();
            this.buttonLoadPalette = new System.Windows.Forms.Button();
            this.savePalette = new System.Windows.Forms.SaveFileDialog();
            this.openPalette = new System.Windows.Forms.OpenFileDialog();
            this.checkCorrect = new System.Windows.Forms.CheckBox();
            this.labelToProcess = new System.Windows.Forms.Label();
            this.saveImage = new System.Windows.Forms.SaveFileDialog();
            this.buttonSaveImage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(0, 30);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // buttonStep
            // 
            this.buttonStep.Location = new System.Drawing.Point(0, 0);
            this.buttonStep.Name = "buttonStep";
            this.buttonStep.Size = new System.Drawing.Size(100, 30);
            this.buttonStep.TabIndex = 1;
            this.buttonStep.Text = "step";
            this.buttonStep.UseVisualStyleBackColor = true;
            this.buttonStep.Click += new System.EventHandler(this.ButtonStep_Click);
            // 
            // checkPlay
            // 
            this.checkPlay.AutoSize = true;
            this.checkPlay.Location = new System.Drawing.Point(100, 0);
            this.checkPlay.Name = "checkPlay";
            this.checkPlay.Size = new System.Drawing.Size(63, 24);
            this.checkPlay.TabIndex = 2;
            this.checkPlay.Text = "play";
            this.checkPlay.UseVisualStyleBackColor = true;
            this.checkPlay.CheckedChanged += new System.EventHandler(this.CheckPlay_CheckedChanged);
            // 
            // checkRender
            // 
            this.checkRender.AutoSize = true;
            this.checkRender.Checked = true;
            this.checkRender.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkRender.Location = new System.Drawing.Point(200, 0);
            this.checkRender.Name = "checkRender";
            this.checkRender.Size = new System.Drawing.Size(81, 24);
            this.checkRender.TabIndex = 3;
            this.checkRender.Text = "render";
            this.checkRender.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(300, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "palette:";
            // 
            // buttonSavePalette
            // 
            this.buttonSavePalette.Location = new System.Drawing.Point(369, 0);
            this.buttonSavePalette.Name = "buttonSavePalette";
            this.buttonSavePalette.Size = new System.Drawing.Size(75, 30);
            this.buttonSavePalette.TabIndex = 5;
            this.buttonSavePalette.Text = "save";
            this.buttonSavePalette.UseVisualStyleBackColor = true;
            this.buttonSavePalette.Click += new System.EventHandler(this.ButtonSavePalette_Click);
            // 
            // buttonLoadPalette
            // 
            this.buttonLoadPalette.Location = new System.Drawing.Point(450, 0);
            this.buttonLoadPalette.Name = "buttonLoadPalette";
            this.buttonLoadPalette.Size = new System.Drawing.Size(75, 30);
            this.buttonLoadPalette.TabIndex = 6;
            this.buttonLoadPalette.Text = "load";
            this.buttonLoadPalette.UseVisualStyleBackColor = true;
            this.buttonLoadPalette.Click += new System.EventHandler(this.ButtonLoadPalette_Click);
            // 
            // savePalette
            // 
            this.savePalette.DefaultExt = "txt";
            this.savePalette.Filter = "Text files|*.txt|All files|*.*";
            // 
            // openPalette
            // 
            this.openPalette.DefaultExt = "txt";
            this.openPalette.Filter = "Text files|*.txt|All files|*.*";
            // 
            // checkCorrect
            // 
            this.checkCorrect.AutoSize = true;
            this.checkCorrect.Checked = true;
            this.checkCorrect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkCorrect.Location = new System.Drawing.Point(550, 0);
            this.checkCorrect.Name = "checkCorrect";
            this.checkCorrect.Size = new System.Drawing.Size(84, 24);
            this.checkCorrect.TabIndex = 7;
            this.checkCorrect.Text = "correct";
            this.checkCorrect.UseVisualStyleBackColor = true;
            // 
            // labelToProcess
            // 
            this.labelToProcess.AutoSize = true;
            this.labelToProcess.Location = new System.Drawing.Point(640, 0);
            this.labelToProcess.Name = "labelToProcess";
            this.labelToProcess.Size = new System.Drawing.Size(18, 20);
            this.labelToProcess.TabIndex = 8;
            this.labelToProcess.Text = "1";
            // 
            // saveImage
            // 
            this.saveImage.DefaultExt = "png";
            // 
            // buttonSaveImage
            // 
            this.buttonSaveImage.Location = new System.Drawing.Point(690, 0);
            this.buttonSaveImage.Name = "buttonSaveImage";
            this.buttonSaveImage.Size = new System.Drawing.Size(110, 30);
            this.buttonSaveImage.TabIndex = 9;
            this.buttonSaveImage.Text = "save image";
            this.buttonSaveImage.UseVisualStyleBackColor = true;
            this.buttonSaveImage.Click += new System.EventHandler(this.ButtonSaveImage_Click);
            // 
            // AdditionMethod
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonSaveImage);
            this.Controls.Add(this.labelToProcess);
            this.Controls.Add(this.checkCorrect);
            this.Controls.Add(this.buttonLoadPalette);
            this.Controls.Add(this.buttonSavePalette);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkRender);
            this.Controls.Add(this.checkPlay);
            this.Controls.Add(this.buttonStep);
            this.Controls.Add(this.pictureBox1);
            this.Name = "AdditionMethod";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AdditionMethod_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonStep;
        private System.Windows.Forms.CheckBox checkPlay;
        private System.Windows.Forms.CheckBox checkRender;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSavePalette;
        private System.Windows.Forms.Button buttonLoadPalette;
        private System.Windows.Forms.SaveFileDialog savePalette;
        private System.Windows.Forms.OpenFileDialog openPalette;
        private System.Windows.Forms.CheckBox checkCorrect;
        private System.Windows.Forms.Label labelToProcess;
        private System.Windows.Forms.SaveFileDialog saveImage;
        private System.Windows.Forms.Button buttonSaveImage;
    }
}

