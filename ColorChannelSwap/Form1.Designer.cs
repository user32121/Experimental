namespace ColorChannelSwap
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboRed = new System.Windows.Forms.ComboBox();
            this.comboGreen = new System.Windows.Forms.ComboBox();
            this.comboBlue = new System.Windows.Forms.ComboBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 100);
            this.label1.TabIndex = 0;
            this.label1.Text = "Red\r\n\r\nGreen\r\n\r\nBlue\r\n";
            // 
            // comboRed
            // 
            this.comboRed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRed.FormattingEnabled = true;
            this.comboRed.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue"});
            this.comboRed.Location = new System.Drawing.Point(72, 6);
            this.comboRed.Name = "comboRed";
            this.comboRed.Size = new System.Drawing.Size(121, 28);
            this.comboRed.TabIndex = 1;
            // 
            // comboGreen
            // 
            this.comboGreen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboGreen.FormattingEnabled = true;
            this.comboGreen.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue"});
            this.comboGreen.Location = new System.Drawing.Point(72, 45);
            this.comboGreen.Name = "comboGreen";
            this.comboGreen.Size = new System.Drawing.Size(121, 28);
            this.comboGreen.TabIndex = 2;
            // 
            // comboBlue
            // 
            this.comboBlue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBlue.FormattingEnabled = true;
            this.comboBlue.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue"});
            this.comboBlue.Location = new System.Drawing.Point(72, 85);
            this.comboBlue.Name = "comboBlue";
            this.comboBlue.Size = new System.Drawing.Size(121, 28);
            this.comboBlue.TabIndex = 3;
            // 
            // buttonStart
            // 
            this.buttonStart.AutoSize = true;
            this.buttonStart.Location = new System.Drawing.Point(12, 119);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(100, 30);
            this.buttonStart.TabIndex = 4;
            this.buttonStart.Text = "choose file";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "png";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "png";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.comboBlue);
            this.Controls.Add(this.comboGreen);
            this.Controls.Add(this.comboRed);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboRed;
        private System.Windows.Forms.ComboBox comboGreen;
        private System.Windows.Forms.ComboBox comboBlue;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

