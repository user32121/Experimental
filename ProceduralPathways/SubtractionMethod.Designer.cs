namespace ProceduralPathways
{
    partial class SubtractionMethod
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
            this.checkRender = new System.Windows.Forms.CheckBox();
            this.checkPlay = new System.Windows.Forms.CheckBox();
            this.buttonStep = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textEdits = new System.Windows.Forms.TextBox();
            this.checkEnds = new System.Windows.Forms.CheckBox();
            this.buttonTrim = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // checkRender
            // 
            this.checkRender.AutoSize = true;
            this.checkRender.Checked = true;
            this.checkRender.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkRender.Location = new System.Drawing.Point(200, 0);
            this.checkRender.Name = "checkRender";
            this.checkRender.Size = new System.Drawing.Size(81, 24);
            this.checkRender.TabIndex = 7;
            this.checkRender.Text = "render";
            this.checkRender.UseVisualStyleBackColor = true;
            // 
            // checkPlay
            // 
            this.checkPlay.AutoSize = true;
            this.checkPlay.Location = new System.Drawing.Point(100, 0);
            this.checkPlay.Name = "checkPlay";
            this.checkPlay.Size = new System.Drawing.Size(63, 24);
            this.checkPlay.TabIndex = 6;
            this.checkPlay.Text = "play";
            this.checkPlay.UseVisualStyleBackColor = true;
            this.checkPlay.CheckedChanged += new System.EventHandler(this.CheckPlay_CheckedChanged);
            // 
            // buttonStep
            // 
            this.buttonStep.Location = new System.Drawing.Point(0, 0);
            this.buttonStep.Name = "buttonStep";
            this.buttonStep.Size = new System.Drawing.Size(100, 30);
            this.buttonStep.TabIndex = 5;
            this.buttonStep.Text = "step";
            this.buttonStep.UseVisualStyleBackColor = true;
            this.buttonStep.Click += new System.EventHandler(this.ButtonStep_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(0, 30);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // textEdits
            // 
            this.textEdits.Location = new System.Drawing.Point(300, 0);
            this.textEdits.Name = "textEdits";
            this.textEdits.Size = new System.Drawing.Size(100, 26);
            this.textEdits.TabIndex = 8;
            this.textEdits.TextChanged += new System.EventHandler(this.TextEdits_TextChanged);
            // 
            // checkEnds
            // 
            this.checkEnds.AutoSize = true;
            this.checkEnds.Location = new System.Drawing.Point(400, 0);
            this.checkEnds.Name = "checkEnds";
            this.checkEnds.Size = new System.Drawing.Size(109, 24);
            this.checkEnds.TabIndex = 9;
            this.checkEnds.Text = "allow ends";
            this.checkEnds.UseVisualStyleBackColor = true;
            // 
            // buttonTrim
            // 
            this.buttonTrim.Location = new System.Drawing.Point(510, 0);
            this.buttonTrim.Name = "buttonTrim";
            this.buttonTrim.Size = new System.Drawing.Size(90, 30);
            this.buttonTrim.TabIndex = 10;
            this.buttonTrim.Text = "trim ends";
            this.buttonTrim.UseVisualStyleBackColor = true;
            this.buttonTrim.Click += new System.EventHandler(this.ButtonTrim_Click);
            // 
            // SubtractionMethod
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonTrim);
            this.Controls.Add(this.checkEnds);
            this.Controls.Add(this.textEdits);
            this.Controls.Add(this.checkRender);
            this.Controls.Add(this.checkPlay);
            this.Controls.Add(this.buttonStep);
            this.Controls.Add(this.pictureBox1);
            this.Name = "SubtractionMethod";
            this.Text = "SubtractionMethod";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SubtractionMethod_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkRender;
        private System.Windows.Forms.CheckBox checkPlay;
        private System.Windows.Forms.Button buttonStep;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textEdits;
        private System.Windows.Forms.CheckBox checkEnds;
        private System.Windows.Forms.Button buttonTrim;
    }
}