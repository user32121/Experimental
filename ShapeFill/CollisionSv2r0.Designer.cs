namespace ShapeFill
{
    partial class CollisionSv2r0
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
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonConfirm = new System.Windows.Forms.Button();
            this.pictureDisplay = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.buttonTetro = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(10, 10);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(75, 30);
            this.buttonReset.TabIndex = 1;
            this.buttonReset.Text = "reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.ButtonReset_Click);
            // 
            // buttonConfirm
            // 
            this.buttonConfirm.Location = new System.Drawing.Point(100, 10);
            this.buttonConfirm.Name = "buttonConfirm";
            this.buttonConfirm.Size = new System.Drawing.Size(75, 30);
            this.buttonConfirm.TabIndex = 3;
            this.buttonConfirm.Text = "confirm";
            this.buttonConfirm.UseVisualStyleBackColor = true;
            this.buttonConfirm.Click += new System.EventHandler(this.ButtonConfirm_Click);
            // 
            // pictureDisplay
            // 
            this.pictureDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureDisplay.Location = new System.Drawing.Point(12, 46);
            this.pictureDisplay.Name = "pictureDisplay";
            this.pictureDisplay.Size = new System.Drawing.Size(400, 400);
            this.pictureDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureDisplay.TabIndex = 4;
            this.pictureDisplay.TabStop = false;
            this.pictureDisplay.Click += new System.EventHandler(this.PictureDisplay_Click);
            this.pictureDisplay.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureDisplay_MouseMove);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // buttonTetro
            // 
            this.buttonTetro.Location = new System.Drawing.Point(190, 10);
            this.buttonTetro.Name = "buttonTetro";
            this.buttonTetro.Size = new System.Drawing.Size(75, 30);
            this.buttonTetro.TabIndex = 5;
            this.buttonTetro.Text = "tetro";
            this.buttonTetro.UseVisualStyleBackColor = true;
            this.buttonTetro.Click += new System.EventHandler(this.ButtonTetro_Click);
            // 
            // Collision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 694);
            this.Controls.Add(this.buttonTetro);
            this.Controls.Add(this.pictureDisplay);
            this.Controls.Add(this.buttonConfirm);
            this.Controls.Add(this.buttonReset);
            this.Name = "Collision";
            this.Text = "Collision";
            ((System.ComponentModel.ISupportInitialize)(this.pictureDisplay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonConfirm;
        private System.Windows.Forms.PictureBox pictureDisplay;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button buttonTetro;
    }
}