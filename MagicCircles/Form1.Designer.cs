namespace MagicCircles
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
            this.buttonRune = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonCircle = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.button_effect = new System.Windows.Forms.Button();
            this.buttonImportCircle = new System.Windows.Forms.Button();
            this.buttonExportCircle = new System.Windows.Forms.Button();
            this.buttonExportRune = new System.Windows.Forms.Button();
            this.buttonImportRune = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonRune
            // 
            this.buttonRune.Location = new System.Drawing.Point(12, 12);
            this.buttonRune.Name = "buttonRune";
            this.buttonRune.Size = new System.Drawing.Size(75, 23);
            this.buttonRune.TabIndex = 0;
            this.buttonRune.Text = "rune";
            this.buttonRune.UseVisualStyleBackColor = true;
            this.buttonRune.Click += new System.EventHandler(this.buttonRune_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(93, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // buttonCircle
            // 
            this.buttonCircle.Location = new System.Drawing.Point(12, 41);
            this.buttonCircle.Name = "buttonCircle";
            this.buttonCircle.Size = new System.Drawing.Size(75, 23);
            this.buttonCircle.TabIndex = 2;
            this.buttonCircle.Text = "circle";
            this.buttonCircle.UseVisualStyleBackColor = true;
            this.buttonCircle.Click += new System.EventHandler(this.buttonCircle_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(12, 70);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // button_effect
            // 
            this.button_effect.Location = new System.Drawing.Point(12, 99);
            this.button_effect.Name = "button_effect";
            this.button_effect.Size = new System.Drawing.Size(75, 23);
            this.button_effect.TabIndex = 4;
            this.button_effect.Text = "effect";
            this.button_effect.UseVisualStyleBackColor = true;
            this.button_effect.Click += new System.EventHandler(this.Button_effect_Click);
            // 
            // buttonImportCircle
            // 
            this.buttonImportCircle.Location = new System.Drawing.Point(12, 128);
            this.buttonImportCircle.Name = "buttonImportCircle";
            this.buttonImportCircle.Size = new System.Drawing.Size(75, 23);
            this.buttonImportCircle.TabIndex = 5;
            this.buttonImportCircle.Text = "import circle";
            this.buttonImportCircle.UseVisualStyleBackColor = true;
            this.buttonImportCircle.Click += new System.EventHandler(this.ButtonImportCircle_Click);
            // 
            // buttonExportCircle
            // 
            this.buttonExportCircle.Location = new System.Drawing.Point(12, 157);
            this.buttonExportCircle.Name = "buttonExportCircle";
            this.buttonExportCircle.Size = new System.Drawing.Size(75, 23);
            this.buttonExportCircle.TabIndex = 6;
            this.buttonExportCircle.Text = "export circle";
            this.buttonExportCircle.UseVisualStyleBackColor = true;
            this.buttonExportCircle.Click += new System.EventHandler(this.ButtonExportCircle_Click);
            // 
            // buttonExportRune
            // 
            this.buttonExportRune.Location = new System.Drawing.Point(12, 215);
            this.buttonExportRune.Name = "buttonExportRune";
            this.buttonExportRune.Size = new System.Drawing.Size(75, 23);
            this.buttonExportRune.TabIndex = 8;
            this.buttonExportRune.Text = "export rune";
            this.buttonExportRune.UseVisualStyleBackColor = true;
            this.buttonExportRune.Click += new System.EventHandler(this.ButtonExportRune_Click);
            // 
            // buttonImportRune
            // 
            this.buttonImportRune.Location = new System.Drawing.Point(12, 186);
            this.buttonImportRune.Name = "buttonImportRune";
            this.buttonImportRune.Size = new System.Drawing.Size(75, 23);
            this.buttonImportRune.TabIndex = 7;
            this.buttonImportRune.Text = "import rune";
            this.buttonImportRune.UseVisualStyleBackColor = true;
            this.buttonImportRune.Click += new System.EventHandler(this.ButtonImportRune_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonExportRune);
            this.Controls.Add(this.buttonImportRune);
            this.Controls.Add(this.buttonExportCircle);
            this.Controls.Add(this.buttonImportCircle);
            this.Controls.Add(this.button_effect);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonCircle);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonRune);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRune;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonCircle;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button button_effect;
        private System.Windows.Forms.Button buttonImportCircle;
        private System.Windows.Forms.Button buttonExportCircle;
        private System.Windows.Forms.Button buttonExportRune;
        private System.Windows.Forms.Button buttonImportRune;
    }
}

