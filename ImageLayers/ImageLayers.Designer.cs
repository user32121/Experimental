namespace ImageLayers
{
    partial class ImageLayers
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
            this.buttonLoad = new System.Windows.Forms.Button();
            this.pictureR = new System.Windows.Forms.PictureBox();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.buttonSave = new System.Windows.Forms.Button();
            this.pictureG = new System.Windows.Forms.PictureBox();
            this.pictureB = new System.Windows.Forms.PictureBox();
            this.pictureH = new System.Windows.Forms.PictureBox();
            this.pictureS = new System.Windows.Forms.PictureBox();
            this.pictureV = new System.Windows.Forms.PictureBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.textScale = new System.Windows.Forms.TextBox();
            this.buttonTest = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureV)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(10, 10);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 40);
            this.buttonLoad.TabIndex = 0;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.ButtonLoad_Click);
            // 
            // pictureR
            // 
            this.pictureR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureR.Location = new System.Drawing.Point(0, 50);
            this.pictureR.Name = "pictureR";
            this.pictureR.Size = new System.Drawing.Size(100, 100);
            this.pictureR.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureR.TabIndex = 1;
            this.pictureR.TabStop = false;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Bitmap Image|*.bmp;";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(100, 10);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 40);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // pictureG
            // 
            this.pictureG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureG.Location = new System.Drawing.Point(100, 50);
            this.pictureG.Name = "pictureG";
            this.pictureG.Size = new System.Drawing.Size(100, 100);
            this.pictureG.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureG.TabIndex = 3;
            this.pictureG.TabStop = false;
            // 
            // pictureB
            // 
            this.pictureB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureB.Location = new System.Drawing.Point(200, 50);
            this.pictureB.Name = "pictureB";
            this.pictureB.Size = new System.Drawing.Size(100, 100);
            this.pictureB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureB.TabIndex = 4;
            this.pictureB.TabStop = false;
            // 
            // pictureH
            // 
            this.pictureH.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureH.Location = new System.Drawing.Point(0, 150);
            this.pictureH.Name = "pictureH";
            this.pictureH.Size = new System.Drawing.Size(100, 100);
            this.pictureH.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureH.TabIndex = 5;
            this.pictureH.TabStop = false;
            // 
            // pictureS
            // 
            this.pictureS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureS.Location = new System.Drawing.Point(100, 150);
            this.pictureS.Name = "pictureS";
            this.pictureS.Size = new System.Drawing.Size(100, 100);
            this.pictureS.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureS.TabIndex = 6;
            this.pictureS.TabStop = false;
            // 
            // pictureV
            // 
            this.pictureV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureV.Location = new System.Drawing.Point(200, 150);
            this.pictureV.Name = "pictureV";
            this.pictureV.Size = new System.Drawing.Size(100, 100);
            this.pictureV.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureV.TabIndex = 7;
            this.pictureV.TabStop = false;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Bitmap Images|*.bmp;*.gif;*.jpg;*.jpeg;*.jpe;*.jif;*.jfif;*.png;*.tiff*.tif;";
            // 
            // textScale
            // 
            this.textScale.Location = new System.Drawing.Point(200, 10);
            this.textScale.Name = "textScale";
            this.textScale.Size = new System.Drawing.Size(100, 26);
            this.textScale.TabIndex = 8;
            this.textScale.Text = "1,1";
            this.textScale.TextChanged += new System.EventHandler(this.TextScale_TextChanged);
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(350, 10);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(75, 40);
            this.buttonTest.TabIndex = 10;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.textScale);
            this.Controls.Add(this.pictureV);
            this.Controls.Add(this.pictureS);
            this.Controls.Add(this.pictureH);
            this.Controls.Add(this.pictureB);
            this.Controls.Add(this.pictureG);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.pictureR);
            this.Controls.Add(this.buttonLoad);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.PictureBox pictureR;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.PictureBox pictureG;
        private System.Windows.Forms.PictureBox pictureB;
        private System.Windows.Forms.PictureBox pictureH;
        private System.Windows.Forms.PictureBox pictureS;
        private System.Windows.Forms.PictureBox pictureV;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TextBox textScale;
        private System.Windows.Forms.Button buttonTest;
    }
}

