namespace 에라번역
{
    partial class 단어추가창
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
            this.추천단어_tb = new System.Windows.Forms.TextBox();
            this.원본_tb = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.추가버튼 = new System.Windows.Forms.Button();
            this.설명_tb = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // 추천단어_tb
            // 
            this.추천단어_tb.Location = new System.Drawing.Point(138, 39);
            this.추천단어_tb.Name = "추천단어_tb";
            this.추천단어_tb.Size = new System.Drawing.Size(100, 21);
            this.추천단어_tb.TabIndex = 0;
            this.추천단어_tb.Text = "추천단어";
            this.추천단어_tb.Click += new System.EventHandler(this.text_Clicked);
            this.추천단어_tb.Leave += new System.EventHandler(this.text_leaved);
            // 
            // 원본_tb
            // 
            this.원본_tb.Location = new System.Drawing.Point(138, 12);
            this.원본_tb.Name = "원본_tb";
            this.원본_tb.Size = new System.Drawing.Size(100, 21);
            this.원본_tb.TabIndex = 1;
            this.원본_tb.Text = "원본";
            this.원본_tb.Click += new System.EventHandler(this.text_Clicked);
            this.원본_tb.Leave += new System.EventHandler(this.text_leaved);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(12, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 112);
            this.listBox1.TabIndex = 2;
            // 
            // 추가버튼
            // 
            this.추가버튼.Location = new System.Drawing.Point(138, 93);
            this.추가버튼.Name = "추가버튼";
            this.추가버튼.Size = new System.Drawing.Size(75, 23);
            this.추가버튼.TabIndex = 3;
            this.추가버튼.Text = "추가";
            this.추가버튼.UseVisualStyleBackColor = true;
            this.추가버튼.Click += new System.EventHandler(this.추가버튼_Click);
            // 
            // 설명_tb
            // 
            this.설명_tb.Location = new System.Drawing.Point(138, 66);
            this.설명_tb.Name = "설명_tb";
            this.설명_tb.Size = new System.Drawing.Size(100, 21);
            this.설명_tb.TabIndex = 4;
            this.설명_tb.Text = "설명";
            this.설명_tb.Click += new System.EventHandler(this.text_Clicked);
            this.설명_tb.Leave += new System.EventHandler(this.text_leaved);
            // 
            // 단어추가창
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 138);
            this.ControlBox = false;
            this.Controls.Add(this.설명_tb);
            this.Controls.Add(this.추가버튼);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.원본_tb);
            this.Controls.Add(this.추천단어_tb);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "단어추가창";
            this.ShowInTaskbar = false;
            this.Text = "단어추가";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox 추천단어_tb;
        private System.Windows.Forms.TextBox 원본_tb;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button 추가버튼;
        private System.Windows.Forms.TextBox 설명_tb;
    }
}