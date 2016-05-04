namespace 에라번역
{
    partial class Batch_Trans
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.원본 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.번역본 = new System.Windows.Forms.TextBox();
            this.적용버튼 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(12, 13);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(100, 16);
            this.textBox1.TabIndex = 0;
            this.textBox1.TabStop = false;
            this.textBox1.Text = "원본";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // 원본
            // 
            this.원본.Location = new System.Drawing.Point(12, 37);
            this.원본.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.원본.Name = "원본";
            this.원본.Size = new System.Drawing.Size(100, 23);
            this.원본.TabIndex = 0;
            // 
            // textBox3
            // 
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Location = new System.Drawing.Point(12, 68);
            this.textBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(100, 16);
            this.textBox3.TabIndex = 2;
            this.textBox3.TabStop = false;
            this.textBox3.Text = "번역본";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // 번역본
            // 
            this.번역본.Location = new System.Drawing.Point(12, 92);
            this.번역본.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.번역본.Name = "번역본";
            this.번역본.Size = new System.Drawing.Size(100, 23);
            this.번역본.TabIndex = 1;
            // 
            // 적용버튼
            // 
            this.적용버튼.Location = new System.Drawing.Point(136, 80);
            this.적용버튼.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.적용버튼.Name = "적용버튼";
            this.적용버튼.Size = new System.Drawing.Size(91, 35);
            this.적용버튼.TabIndex = 4;
            this.적용버튼.TabStop = false;
            this.적용버튼.Text = "적용";
            this.적용버튼.UseVisualStyleBackColor = true;
            this.적용버튼.Click += new System.EventHandler(this.적용버튼_Click);
            // 
            // Batch_Trans
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 147);
            this.ControlBox = false;
            this.Controls.Add(this.적용버튼);
            this.Controls.Add(this.번역본);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.원본);
            this.Controls.Add(this.textBox1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Batch_Trans";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "일괄번역";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox 원본;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox 번역본;
        private System.Windows.Forms.Button 적용버튼;
    }
}