namespace 에라번역
{
    partial class 작성자설정창
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
            this.이름 = new System.Windows.Forms.TextBox();
            this.설명 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.적용버튼 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // 이름
            // 
            this.이름.Location = new System.Drawing.Point(12, 39);
            this.이름.Name = "이름";
            this.이름.Size = new System.Drawing.Size(100, 21);
            this.이름.TabIndex = 0;
            // 
            // 설명
            // 
            this.설명.Location = new System.Drawing.Point(12, 67);
            this.설명.Name = "설명";
            this.설명.Size = new System.Drawing.Size(100, 21);
            this.설명.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBox1.Font = new System.Drawing.Font("굴림", 11F);
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(100, 17);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "이름,번역설명";
            // 
            // 적용버튼
            // 
            this.적용버튼.Location = new System.Drawing.Point(118, 39);
            this.적용버튼.Name = "적용버튼";
            this.적용버튼.Size = new System.Drawing.Size(87, 49);
            this.적용버튼.TabIndex = 3;
            this.적용버튼.Text = "적용";
            this.적용버튼.UseVisualStyleBackColor = true;
            this.적용버튼.Click += new System.EventHandler(this.적용버튼_Click);
            // 
            // 작성자설정창
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 113);
            this.Controls.Add(this.적용버튼);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.설명);
            this.Controls.Add(this.이름);
            this.Name = "작성자설정창";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "제작자정보";
            this.Load += new System.EventHandler(this.작성자설정창_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox 이름;
        private System.Windows.Forms.TextBox 설명;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button 적용버튼;
    }
}