namespace Life
{
    partial class Life
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
            this.checkRunning = new System.Windows.Forms.CheckBox();
            this.textSpeed = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.comboWidth = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // checkRunning
            // 
            this.checkRunning.AutoSize = true;
            this.checkRunning.Location = new System.Drawing.Point(12, 12);
            this.checkRunning.Name = "checkRunning";
            this.checkRunning.Size = new System.Drawing.Size(88, 24);
            this.checkRunning.TabIndex = 0;
            this.checkRunning.Text = "running";
            this.checkRunning.UseVisualStyleBackColor = true;
            this.checkRunning.CheckedChanged += new System.EventHandler(this.CheckRunning_CheckedChanged);
            // 
            // textSpeed
            // 
            this.textSpeed.Location = new System.Drawing.Point(106, 12);
            this.textSpeed.Name = "textSpeed";
            this.textSpeed.Size = new System.Drawing.Size(50, 26);
            this.textSpeed.TabIndex = 1;
            this.textSpeed.Text = "1";
            this.textSpeed.TextChanged += new System.EventHandler(this.TextSpeed_TextChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 44);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseUp);
            // 
            // comboWidth
            // 
            this.comboWidth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboWidth.FormattingEnabled = true;
            this.comboWidth.Items.AddRange(new object[] {
            "1",
            "3",
            "5",
            "7",
            "9"});
            this.comboWidth.Location = new System.Drawing.Point(163, 13);
            this.comboWidth.Name = "comboWidth";
            this.comboWidth.Size = new System.Drawing.Size(121, 28);
            this.comboWidth.TabIndex = 3;
            // 
            // Life
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.comboWidth);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textSpeed);
            this.Controls.Add(this.checkRunning);
            this.Name = "Life";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Life_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkRunning;
        private System.Windows.Forms.TextBox textSpeed;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox comboWidth;
    }
}

