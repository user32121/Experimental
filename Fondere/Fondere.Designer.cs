namespace Experimental
{
    partial class Fondere
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
            this.textSample = new System.Windows.Forms.TextBox();
            this.buttonBuild = new System.Windows.Forms.Button();
            this.textMaxChar = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textRngRange = new System.Windows.Forms.TextBox();
            this.textOutput = new System.Windows.Forms.TextBox();
            this.textPrevChar = new System.Windows.Forms.TextBox();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.buttonAuto = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textSample
            // 
            this.textSample.Location = new System.Drawing.Point(13, 13);
            this.textSample.Multiline = true;
            this.textSample.Name = "textSample";
            this.textSample.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textSample.Size = new System.Drawing.Size(300, 425);
            this.textSample.TabIndex = 0;
            this.textSample.Text = "test setstses";
            // 
            // buttonBuild
            // 
            this.buttonBuild.Location = new System.Drawing.Point(319, 42);
            this.buttonBuild.Name = "buttonBuild";
            this.buttonBuild.Size = new System.Drawing.Size(75, 23);
            this.buttonBuild.TabIndex = 2;
            this.buttonBuild.Text = "rebuild";
            this.buttonBuild.UseVisualStyleBackColor = true;
            this.buttonBuild.Click += new System.EventHandler(this.ButtonBuild_Click);
            // 
            // textMaxChar
            // 
            this.textMaxChar.Location = new System.Drawing.Point(319, 71);
            this.textMaxChar.Name = "textMaxChar";
            this.textMaxChar.Size = new System.Drawing.Size(50, 20);
            this.textMaxChar.TabIndex = 3;
            this.textMaxChar.Text = "10";
            this.textMaxChar.TextChanged += new System.EventHandler(this.TextMaxChar_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(375, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 65);
            this.label1.TabIndex = 2;
            this.label1.Text = "maxCharacters\r\n\r\nrngRange\r\n\r\nprevCharacters";
            // 
            // textRngRange
            // 
            this.textRngRange.Location = new System.Drawing.Point(319, 97);
            this.textRngRange.Name = "textRngRange";
            this.textRngRange.Size = new System.Drawing.Size(50, 20);
            this.textRngRange.TabIndex = 3;
            this.textRngRange.Text = "3";
            this.textRngRange.TextChanged += new System.EventHandler(this.TextRngRange_TextChanged);
            // 
            // textOutput
            // 
            this.textOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textOutput.Location = new System.Drawing.Point(500, 13);
            this.textOutput.Multiline = true;
            this.textOutput.Name = "textOutput";
            this.textOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textOutput.Size = new System.Drawing.Size(288, 425);
            this.textOutput.TabIndex = 10;
            // 
            // textPrevChar
            // 
            this.textPrevChar.Location = new System.Drawing.Point(319, 124);
            this.textPrevChar.Name = "textPrevChar";
            this.textPrevChar.Size = new System.Drawing.Size(50, 20);
            this.textPrevChar.TabIndex = 3;
            this.textPrevChar.Text = "2";
            this.textPrevChar.TextChanged += new System.EventHandler(this.TextPrevChar_TextChanged);
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(401, 43);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(75, 23);
            this.buttonGenerate.TabIndex = 2;
            this.buttonGenerate.Text = "generate";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.ButtonGenerate_Click);
            // 
            // buttonAuto
            // 
            this.buttonAuto.Location = new System.Drawing.Point(319, 13);
            this.buttonAuto.Name = "buttonAuto";
            this.buttonAuto.Size = new System.Drawing.Size(75, 23);
            this.buttonAuto.TabIndex = 1;
            this.buttonAuto.Text = "auto";
            this.buttonAuto.UseVisualStyleBackColor = true;
            this.buttonAuto.Click += new System.EventHandler(this.ButtonAuto_Click);
            // 
            // Fondere
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonAuto);
            this.Controls.Add(this.buttonBuild);
            this.Controls.Add(this.buttonGenerate);
            this.Controls.Add(this.textOutput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textMaxChar);
            this.Controls.Add(this.textSample);
            this.Controls.Add(this.textRngRange);
            this.Controls.Add(this.textPrevChar);
            this.Name = "Fondere";
            this.Text = "Fondere";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textSample;
        private System.Windows.Forms.Button buttonBuild;
        private System.Windows.Forms.TextBox textMaxChar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textRngRange;
        private System.Windows.Forms.TextBox textOutput;
        private System.Windows.Forms.TextBox textPrevChar;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.Button buttonAuto;
    }
}

